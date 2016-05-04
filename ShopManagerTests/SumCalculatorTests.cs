using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ShopManager.Tests
{
    [TestClass]
    public class SumCalculatorTests
    {
        [TestMethod]
        public void SumTest_If_Group_3_Products_And_Option_Cost_Should_Sum_Of_Cost_Of_Each_Group()
        {
            var sumCalculator = new SumCalculator();
            var numberOfProducts = 3;
            var optionToSum = "cost";
            var expected = new List<int> {6, 15, 24, 21};
            
            var actual = sumCalculator.Sum(numberOfProducts, optionToSum).ToList();

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void SumTest_If_Group_4_Products_And_Option_Revenue_Should_Sum_Of_Revenue_Of_Each_Group()
        {
            var sumCalculator = new SumCalculator();
            var numberOfProducts = 4;
            var optionToSum = "revenue";
            var expected = new List<int> { 50, 66, 60 };

            var actual = sumCalculator.Sum(numberOfProducts, optionToSum).ToList();

            CollectionAssert.AreEqual(expected, actual);
        }
    }
}
