using ShipFood.API.Services.Logger;

namespace ShipFood.API.Services.Pricing
{
    public class HappyHourPricingStrategy : IPricingStrategy
    {
        private readonly ILoggerService _logger;
        private const decimal DISCOUNT_RATE = 0.20m; // Giảm 20%

        public HappyHourPricingStrategy(ILoggerService logger)
        {
            _logger = logger;
        }

        public decimal CalculateTotal(decimal subTotal)
        {
            decimal discount = subTotal * DISCOUNT_RATE;
            decimal finalPrice = subTotal - discount;
            _logger.LogInfo($"[Pricing] HAPPY HOUR! Original: {subTotal:C}, Discount: {discount:C}, Final: {finalPrice:C}");
            return finalPrice;
        }
    }
}
