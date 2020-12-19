using Xunit;

namespace CalvinReed.Testing
{
    public class RatioShould
    {
        [Theory]
        [InlineData(1, 3, 1, 3)]
        [InlineData(-1, -3, 1, 3)]
        [InlineData(1, -3, -1, 3)]
        [InlineData(-1, 3, -1, 3)]
        [InlineData(2, 3, 2, 3)]
        [InlineData(2, 6, 1, 3)]
        [InlineData(2, -6, -1, 3)]
        [InlineData(-2, 6, -1, 3)]
        [InlineData(3, 3, 1, 1)]
        public void BeReduced(int n, int d, int reducedN, int reducedD)
        {
            var ratio = Ratio.Create(n, d);
            Assert.Equal(reducedN, ratio.Numerator);
            Assert.Equal(reducedD, ratio.Denominator);
        }
    }
}
