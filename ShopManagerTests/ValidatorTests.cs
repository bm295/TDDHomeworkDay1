using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ShopManager.Tests
{
    [TestClass]
    public class ValidatorTests
    {
        [TestMethod]
        public void Validator_If_Number_Of_Product_From_1_To_11_Should_Be_Valid()
        {
            var validator = new Validator();
            var numberOfProducts = 5;
            var isValid = validator.IsValidNumberOfProducts(numberOfProducts);
            Assert.IsTrue(isValid);
        }

        [TestMethod]
        public void Validator_If_Number_Of_Product_Less_Than_1_Should_Be_Invalid()
        {
            var validator = new Validator();
            var numberOfProducts = -1;
            var isValid = validator.IsValidNumberOfProducts(numberOfProducts);
            Assert.IsFalse(isValid);
        }

        [TestMethod]
        public void Validator_If_Number_Of_Product_Greater_Than_11_Should_Be_Invalid()
        {
            var validator = new Validator();
            var numberOfProducts = 12;
            var isValid = validator.IsValidNumberOfProducts(numberOfProducts);
            Assert.IsFalse(isValid);
        }

        [TestMethod]
        public void Validator_If_Option_To_Sum_Is_Cost_Should_Be_Valid()
        {
            var validator = new Validator();
            var optionToSum = "cost";
            var isValid = validator.IsValidOptionToSum(optionToSum);
            Assert.IsTrue(isValid);
        }

        [TestMethod]
        public void Validator_If_Option_To_Sum_Is_Revenue_Should_Be_Valid()
        {
            var validator = new Validator();
            var optionToSum = "revenue";
            var isValid = validator.IsValidOptionToSum(optionToSum);
            Assert.IsTrue(isValid);
        }

        [TestMethod]
        public void Validator_If_Option_To_Sum_Is_SellPrice_Should_Be_Valid()
        {
            var validator = new Validator();
            var optionToSum = "sellprice";
            var isValid = validator.IsValidOptionToSum(optionToSum);
            Assert.IsTrue(isValid);
        }

        [TestMethod]
        public void Validator_If_Option_To_Sum_Is_Empty_Should_Be_Invalid()
        {
            var validator = new Validator();
            var optionToSum = string.Empty;
            var isValid = validator.IsValidOptionToSum(optionToSum);
            Assert.IsFalse(isValid);
        }

        [TestMethod]
        public void Validator_If_Option_To_Sum_Is_Id_Should_Be_Invalid()
        {
            var validator = new Validator();
            var optionToSum = "id";
            var isValid = validator.IsValidOptionToSum(optionToSum);
            Assert.IsFalse(isValid);
        }
    }
}
