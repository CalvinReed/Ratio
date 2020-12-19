using System;

namespace CalvinReed
{
    public readonly struct Ratio : IEquatable<Ratio>, IComparable<Ratio>, IComparable
    {
        private readonly long falseDenominator;

        private Ratio(long numerator, long falseDenominator)
        {
            if (falseDenominator < 0)
            {
                throw new Exception();
            }

            Numerator = numerator;
            this.falseDenominator = falseDenominator;
        }

        public long Numerator { get; }

        public long Denominator => falseDenominator + 1;

        public override string ToString()
        {
            return falseDenominator != 0 ? $"{Numerator}/{Denominator}" : Numerator.ToString();
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var a = (int) Numerator;
                var b = (int) (Numerator >> 32);
                var c = (int) falseDenominator;
                var d = (int) (falseDenominator >> 32);
                return HashCode.Combine(a, b, c, d);
            }
        }

        public override bool Equals(object? obj)
        {
            return obj is Ratio other && Equals(other);
        }

        public bool Equals(Ratio other)
        {
            return Numerator == other.Numerator && falseDenominator == other.falseDenominator;
        }

        public int CompareTo(Ratio other)
        {
            var difference = this - other;
            return Math.Sign(difference.Numerator);
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

        private Ratio GetReciprocal()
        {
            var n = Denominator * Math.Sign(Numerator);
            var d = Math.Abs(Numerator) - 1;
            return new Ratio(n, d);
        }

        public static Ratio operator +(Ratio ratio)
        {
            return ratio;
        }

        public static Ratio operator -(Ratio ratio)
        {
            return new(-ratio.Numerator, ratio.falseDenominator);
        }

        public static Ratio operator +(Ratio left, Ratio right)
        {
            var lcm = Lcm(left.Denominator, right.Denominator, out var gcd);
            var n1 = left.Numerator * (right.Denominator / gcd);
            var n2 = right.Numerator * (left.Denominator / gcd);
            return Create(n1 + n2, lcm);
        }

        public static Ratio operator -(Ratio left, Ratio right)
        {
            return left + -right;
        }

        public static Ratio operator *(Ratio left, Ratio right)
        {
            var n = left.Numerator * right.Numerator;
            var d = left.Denominator * right.Denominator;
            return Create(n, d);
        }

        public static Ratio operator /(Ratio left, Ratio right)
        {
            return left * right.GetReciprocal();
        }

        public static bool operator ==(Ratio left, Ratio right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Ratio left, Ratio right)
        {
            return !left.Equals(right);
        }

        public static bool operator <(Ratio left, Ratio right)
        {
            return left.CompareTo(right) < 0;
        }

        public static bool operator >(Ratio left, Ratio right)
        {
            return left.CompareTo(right) > 0;
        }

        public static bool operator <=(Ratio left, Ratio right)
        {
            return left.CompareTo(right) <= 0;
        }

        public static bool operator >=(Ratio left, Ratio right)
        {
            return left.CompareTo(right) >= 0;
        }

        public static implicit operator Ratio(long n)
        {
            return new(n, 0);
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

        private static long Lcm(long a, long b, out long gcd)
        {
            gcd = Gcd(a, b);
            return a / gcd * b;
        }

        private static long Gcd(long a, long b)
        {
            var max = Math.Max(a, b);
            var min = Math.Min(a, b);
            while (min != 0)
            {
                var mod = max % min;
                max = min;
                min = mod;
            }
            return max;
        }
    }
}
