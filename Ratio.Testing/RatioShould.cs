using System.Collections.Generic;
using Xunit;

namespace CalvinReed.Testing
{
    public class RatioShould
    {
        [Theory]
        [MemberData(nameof(AdditionData))]
        public void AddCorrectly(Ratio a, Ratio b, Ratio c)
        {
            Assert.Equal(c, a + b);
        }

        [Fact]
        public void HaveCorrectDefault()
        {
            Assert.Equal(0, default(Ratio));
        }

        public static IEnumerable<object[]> AdditionData()
        {
            yield return new object[] {Ratio.Create(1, 3), Ratio.Create(1, 3), Ratio.Create(2, 3)};
            yield return new object[] {Ratio.Create(1, 3), Ratio.Create(-1, 3), Ratio.Create(0, 1)};
            yield return new object[] {Ratio.Create(9, 7), Ratio.Create(-2, 7), Ratio.Create(1, 1)};
        }
    }
}
