namespace ShipFood.API.Services.Pricing
{
    public interface IPricingStrategy
    {
        decimal CalculateTotal(decimal subTotal);
    }
}
