using ShipFood.API.Services.Logger;
using ShipFood.API.Models;

namespace ShipFood.API.Services.Discount
{
    public class ShippingFeeDecorator : OrderDecorator
    {
        private readonly decimal _shippingFee;
        private readonly ILoggerService _logger;

        public ShippingFeeDecorator(IOrderAmount orderAmount, decimal shippingFee, ILoggerService logger) 
            : base(orderAmount)
        {
            _shippingFee = shippingFee;
            _logger = logger;
        }

        public override decimal CalculateTotal(TbDonHang dh)
        {
            var currentTotal = base.CalculateTotal(dh);
            var newTotal = currentTotal + _shippingFee;
            
            _logger.LogInfo($"[Decorator] Added Shipping Fee {_shippingFee:C}. Original: {currentTotal:C}, New: {newTotal:C}");
            return newTotal;
        }
    }
}
