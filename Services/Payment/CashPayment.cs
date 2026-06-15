using ShipFood.API.Services.Logger;

namespace ShipFood.API.Services.Payment
{
    public class CashPayment : IPaymentProcessor
    {
        private readonly ILoggerService _logger;

        public CashPayment(ILoggerService logger)
        {
            _logger = logger;
        }

        public void ProcessPayment(decimal amount)
        {
            // Logic xử lý thanh toán tiền mặt (ví dụ: in hóa đơn)
            _logger.LogInfo($"[Payment] Processing CASH payment of {amount:C}");
        }
    }
}
