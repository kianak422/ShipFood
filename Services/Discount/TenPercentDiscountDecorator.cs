using ShipFood.API.Services.Logger;
using ShipFood.API.Models;

namespace ShipFood.API.Services.Discount
{
    public class TenPercentDiscountDecorator : OrderDecorator
    {
        private readonly ILoggerService _logger;

        public TenPercentDiscountDecorator(IOrderAmount orderAmount, ILoggerService logger) 
            : base(orderAmount)
        {
            _logger = logger;
        }

        public override decimal CalculateTotal(TbDonHang dh)
        {
            var currentTotal = base.CalculateTotal(dh);
            var discount = currentTotal * 0.10m;
            var newTotal = currentTotal - discount;

            _logger.LogInfo($"[Decorator] Applied 10% Discount. Original: {currentTotal:C}, New: {newTotal:C}");
            return newTotal;
        }
    }
}
