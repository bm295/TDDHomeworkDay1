namespace Shop
{
    public class Validator
    {
        public bool IsValidNumberOfProducts(int numberOfProducts)
        {
            return numberOfProducts >= 1 && numberOfProducts <= 11;
        }

        public bool IsValidOptionToSum(string optionToSum)
        {
            return !string.IsNullOrEmpty(optionToSum) &&
                   (optionToSum == "cost" || optionToSum == "revenue" || optionToSum == "sellprice");
        }
    }
}
