using Shop;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using static Shop.SumCalculator;

namespace UnitTest
{
    public class SumCalculatorTests
    {
        private SumCalculator _sumCalculator = new SumCalculator();

        [Fact]
        public void SumTest_If_Group_3_Products_And_Option_Cost_Should_Sum_Of_Cost_Of_Each_Group()
        {
            var numberOfProducts = 3;
            var optionToSum = "cost";
            var expected = new List<int> {6, 15, 24, 21};
            
            var actual = _sumCalculator.Sum(numberOfProducts, optionToSum).ToList();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SumTest_If_Group_4_Products_And_Option_Revenue_Should_Sum_Of_Revenue_Of_Each_Group()
        {
            var numberOfProducts = 4;
            var optionToSum = "revenue";
            var expected = new List<int> { 50, 66, 60 };

            var actual = _sumCalculator.Sum(numberOfProducts, optionToSum).ToList();

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(1000, AccountStatus.Unknown, 0, 0)]
        [InlineData(1000, AccountStatus.Unknown, 6, 0)]
        [InlineData(1000, AccountStatus.NotRegistered, 0, 1000)]
        [InlineData(1000, AccountStatus.NotRegistered, 6, 1000)]
        [InlineData(1000, AccountStatus.SimpleCustomer, 0, 900)]
        [InlineData(1000, AccountStatus.SimpleCustomer, 6, 855)]
        [InlineData(1000, AccountStatus.ValuableCustomer, 0, 700)]
        [InlineData(1000, AccountStatus.ValuableCustomer, 6, 665)]
        [InlineData(1000, AccountStatus.MostValuableCustomer, 0, 500)]
        [InlineData(1000, AccountStatus.MostValuableCustomer, 6, 475)]
        public void CalculateTest(decimal price, AccountStatus accountStatus, int timeOfHavingAccountInYears, decimal expected)
        {
            var actual = _sumCalculator.ApplyDiscount(price, accountStatus, timeOfHavingAccountInYears);
            Assert.Equal(expected, actual);
        }
    }
}
