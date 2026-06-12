using ShipFood.API.Services.Logger;

namespace ShipFood.API.Services.Pricing
{
    public class RegularPricingStrategy : IPricingStrategy
    {
        private readonly ILoggerService _logger;

        public RegularPricingStrategy(ILoggerService logger)
        {
            _logger = logger;
        }

        public decimal CalculateTotal(decimal subTotal)
        {
            _logger.LogInfo($"[Pricing] Calculating REGULAR price for {subTotal:C}");
            // Giá thường: Không giảm giá
            return subTotal;
        }
    }
}
