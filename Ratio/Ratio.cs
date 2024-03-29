﻿using System;
using System.Diagnostics;
using System.Numerics;

namespace CalvinReed
{
    public readonly struct Ratio : IEquatable<Ratio>, IComparable<Ratio>, IComparable
    {
        private readonly long numerator;
        private readonly long falseDenominator;

        private Ratio(long numerator, long falseDenominator)
        {
            if (falseDenominator < 0) throw new DivideByZeroException();
            if (numerator == long.MinValue || falseDenominator == long.MaxValue) throw new OverflowException();

            this.numerator = numerator;
            this.falseDenominator = falseDenominator;
        }

        private long Denominator => falseDenominator + 1;

        public override string ToString()
        {
            return $"{numerator}/{Denominator}";
        }

        public override int GetHashCode()
        {
            var l = unchecked(numerator * 397);
            return (l ^ falseDenominator).GetHashCode();
        }

        public override bool Equals(object? obj)
        {
            return obj is Ratio other && Equals(other);
        }

        public bool Equals(Ratio other)
        {
            return numerator == other.numerator && falseDenominator == other.falseDenominator;
        }

        public int CompareTo(Ratio other)
        {
            var difference = this - other;
            return Math.Sign(difference.numerator);
        }

        public int CompareTo(object? obj)
        {
            return obj switch
            {
                null => 1,
                Ratio other => CompareTo(other),
                _ => throw new ArgumentException($"Object must be of type {nameof(Ratio)}")
            };
        }

        public static Ratio Create(long numerator, long denominator)
        {
            var sign = Math.Sign(numerator) * Math.Sign(denominator);
            var nAbs = Math.Abs(numerator);
            var dAbs = Math.Abs(denominator);
            var gcd = Gcd(nAbs, dAbs);
            var n = nAbs / gcd;
            var d = dAbs / gcd;
            return new Ratio(sign * n, d - 1);
        }

        public static Ratio Reciprocal(Ratio ratio)
        {
            var n = ratio.Denominator * Math.Sign(ratio.numerator);
            var d = Math.Abs(ratio.numerator) - 1;
            return new Ratio(n, d);
        }

        // https://en.wikipedia.org/wiki/Binary_GCD_algorithm
        private static long Gcd(long a, long b)
        {
            Debug.Assert(a >= 0);
            Debug.Assert(b >= 0);
            var aTrailing = BitOperations.TrailingZeroCount(a);
            var bTrailing = BitOperations.TrailingZeroCount(b);
            var aReduced = a >> aTrailing;
            var bReduced = b >> bTrailing;
            var max = Math.Max(aReduced, bReduced);
            var min = Math.Min(aReduced, bReduced);
            while (min != 0)
            {
                max -= min;
                max >>= BitOperations.TrailingZeroCount(max);
                if (max < min)
                {
                    (max, min) = (min, max);
                }
            }

            return max << Math.Min(aTrailing, bTrailing);
        }

        public static Ratio operator ++(Ratio ratio)
        {
            var n = checked(ratio.numerator + ratio.Denominator);
            return new Ratio(n, ratio.falseDenominator);
        }

        public static Ratio operator --(Ratio ratio)
        {
            var n = checked(ratio.numerator - ratio.Denominator);
            return new Ratio(n, ratio.falseDenominator);
        }

        public static Ratio operator +(Ratio left, Ratio right)
        {
            checked
            {
                var gcd = Gcd(left.Denominator, right.Denominator);
                var leftFactor = left.Denominator / gcd;
                var rightFactor = right.Denominator / gcd;
                var lcm = leftFactor * right.Denominator;
                var n1 = left.numerator * rightFactor;
                var n2 = right.numerator * leftFactor;
                return Create(n1 + n2, lcm);
            }
        }

        public static Ratio operator *(Ratio left, Ratio right)
        {
            checked
            {
                var n = left.numerator * right.numerator;
                var d = left.Denominator * right.Denominator;
                return Create(n, d);
            }
        }

        public static Ratio operator +(Ratio ratio) => ratio;
        public static Ratio operator -(Ratio ratio) => new(-ratio.numerator, ratio.falseDenominator);
        public static Ratio operator -(Ratio left, Ratio right) => left + -right;
        public static Ratio operator /(Ratio left, Ratio right) => left * Reciprocal(right);
        public static bool operator ==(Ratio left, Ratio right) => left.Equals(right);
        public static bool operator !=(Ratio left, Ratio right) => !left.Equals(right);
        public static bool operator <(Ratio left, Ratio right) => left.CompareTo(right) < 0;
        public static bool operator >(Ratio left, Ratio right) => left.CompareTo(right) > 0;
        public static bool operator <=(Ratio left, Ratio right) => left.CompareTo(right) <= 0;
        public static bool operator >=(Ratio left, Ratio right) => left.CompareTo(right) >= 0;
        public static implicit operator Ratio(long n) => new(n, 0);
        public static explicit operator double(Ratio ratio) => (double) ratio.numerator / ratio.Denominator;
    }
}
