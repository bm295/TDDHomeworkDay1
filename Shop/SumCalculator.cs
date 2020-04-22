using System.Collections.Generic;

namespace Shop
{
    public class SumCalculator
    {
        private List<Product> _Products;

        public SumCalculator()
        {
            _Products = new List<Product>
            {
                new Product {Id = 1, Cost = 1, Revenue = 11},
                new Product {Cost = 2, Revenue = 12},
                new Product {Cost = 3, Revenue = 13},
                new Product {Cost = 4, Revenue = 14},
                new Product {Cost = 5, Revenue = 15},
                new Product {Cost = 6, Revenue = 16},
                new Product {Cost = 7, Revenue = 17},
                new Product {Cost = 8, Revenue = 18},
                new Product {Cost = 9, Revenue = 19},
                new Product {Cost = 10, Revenue = 20},
                new Product {Cost = 11, Revenue = 21}
            };
        }
        public IEnumerable<int> Sum(int numberOfProducts, string optionToSum)
        {
            if (optionToSum == "cost")
                return SumOfCost();
            if (optionToSum == "revenue")
                return SumOfRevenue();

            return new List<int>();
        }

        private List<int> SumOfRevenue()
        {
            var sumOfRevenue = new List<int>
            {
                _Products[0].Revenue + _Products[1].Revenue + _Products[2].Revenue + _Products[3].Revenue,
                _Products[4].Revenue + _Products[5].Revenue + _Products[6].Revenue + _Products[7].Revenue,
                _Products[8].Revenue + _Products[9].Revenue + _Products[10].Revenue
            };
            return sumOfRevenue;
        }

        private List<int> SumOfCost()
        {
            var sumOfCost = new List<int>
            {
                _Products[0].Cost + _Products[1].Cost + _Products[2].Cost,
                _Products[3].Cost + _Products[4].Cost + _Products[5].Cost,
                _Products[6].Cost + _Products[7].Cost + _Products[8].Cost,
                _Products[9].Cost + _Products[10].Cost
            };
            return sumOfCost;
        }

        public decimal ApplyDiscount(decimal price, AccountStatus accountStatus, int timeOfHavingAccountInYears)
        {
            decimal priceAfterDiscount = 0;
            decimal discountForLoyaltyInPercentage = (timeOfHavingAccountInYears > 5) ? (decimal)5 / 100 : (decimal)timeOfHavingAccountInYears / 100;
            switch (accountStatus)
            {
                case AccountStatus.NotRegistered:
                    priceAfterDiscount = price;
                    break;
                case AccountStatus.SimpleCustomer:
                    priceAfterDiscount = price - 0.1m * price;
                    priceAfterDiscount = priceAfterDiscount - discountForLoyaltyInPercentage * priceAfterDiscount;
                    break;
                case AccountStatus.ValuableCustomer:
                    priceAfterDiscount = 0.7m * price;
                    priceAfterDiscount = priceAfterDiscount - discountForLoyaltyInPercentage * priceAfterDiscount;
                    break;
                case AccountStatus.MostValuableCustomer:
                    priceAfterDiscount = price - 0.5m * price;
                    priceAfterDiscount = priceAfterDiscount - discountForLoyaltyInPercentage * priceAfterDiscount;
                    break;
            }
            return priceAfterDiscount;
        }

        public enum AccountStatus
        {
            Unknown = 0,
            NotRegistered = 1,
            SimpleCustomer = 2,
            ValuableCustomer = 3,
            MostValuableCustomer = 4
        }
    }
}
