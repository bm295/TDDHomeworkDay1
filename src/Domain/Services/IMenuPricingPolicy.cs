namespace Domain.Services;

public interface IMenuPricingPolicy
{
    decimal GetSuggestedPrice(string sku);
}
