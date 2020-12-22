using System.Collections.Generic;
using Xunit;

namespace CalvinReed.Testing
{
    public class RatioShould
    {
        [Fact]
        public void HaveCorrectDefault()
        {
            Assert.Equal(0, default(Ratio));
        }

        [Theory]
        [MemberData(nameof(AdditionData))]
        public void AddCorrectly(Ratio a, Ratio b, Ratio c)
        {
            Assert.Equal(c, a + b);
        }

        [Theory]
        [MemberData(nameof(ReductionData))]
        public void ReduceCorrectly(Ratio expected, Ratio actual)
        {
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(1, 2)]
        [InlineData(2, 4)]
        [InlineData(6, 5)]
        [InlineData(long.MaxValue, 649657)]
        public void HaveCorrectReciprocal(long n, long d)
        {
            var a = Ratio.Create(n, d);
            var b = Ratio.Create(d, n);
            var r = Ratio.Reciprocal(a);
            Assert.Equal(b, r);
            Assert.Equal(1, a * r);
        }

        public static IEnumerable<object[]> AdditionData()
        {
            yield return new object[] {Ratio.Create(1, 3), Ratio.Create(1, 3), Ratio.Create(2, 3)};
            yield return new object[] {Ratio.Create(1, 2), Ratio.Create(1, 3), Ratio.Create(5, 6)};
            yield return new object[] {Ratio.Create(1, 3), Ratio.Create(-1, 3), Ratio.Create(0, 1)};
            yield return new object[] {Ratio.Create(9, 7), Ratio.Create(-2, 7), Ratio.Create(1, 1)};
        }

        public static IEnumerable<object[]> ReductionData()
        {
            yield return new object[] {Ratio.Create(1, 3), Ratio.Create(-1, -3)};
            yield return new object[] {Ratio.Create(1, 3), Ratio.Create(2, 6)};
            yield return new object[] {Ratio.Create(1, 3), Ratio.Create(-2, -6)};
            yield return new object[] {Ratio.Create(-1, 3), Ratio.Create(1, -3)};
            yield return new object[] {Ratio.Create(-1, 3), Ratio.Create(-2, 6)};
            yield return new object[] {Ratio.Create(-1, 3), Ratio.Create(2, -6)};
            yield return new object[] {Ratio.Create(1, 289_740_712_999), Ratio.Create(31_833_193, long.MaxValue)};
        }
    }
}
