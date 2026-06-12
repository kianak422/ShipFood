using ShipFood.API.Services.Logger;

namespace ShipFood.API.Services.Pricing
{
    // Context class để sử dụng Strategy
    public class PricingService
    {
        private IPricingStrategy _strategy;
        private readonly ILoggerService _logger;

        // Mặc định là giá thường
        public PricingService(ILoggerService logger)
        {
            _logger = logger;
            _strategy = new RegularPricingStrategy(logger);
        }

        public void SetStrategy(IPricingStrategy strategy)
        {
            _strategy = strategy;
            _logger.LogInfo($"[Pricing] Strategy switched to: {strategy.GetType().Name}");
        }

        public decimal CalculateFinalPrice(decimal subTotal)
        {
            return _strategy.CalculateTotal(subTotal);
        }
    }
}
 