namespace Domain.Services;

public sealed class DefaultMenuPricingPolicy : IMenuPricingPolicy
{
    public decimal GetSuggestedPrice(string sku) =>
        sku.ToUpperInvariant() switch
        {
            "RIBEYE" => 950000m,
            "WINE" => 1200000m,
            "WATER" => 45000m,
            _ => 0m
        };
}
