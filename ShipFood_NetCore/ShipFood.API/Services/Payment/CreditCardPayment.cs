using ShipFood.API.Services.Logger;

namespace ShipFood.API.Services.Payment
{
    public class CreditCardPayment : IPaymentProcessor
    {
        private readonly ILoggerService _logger;

        public CreditCardPayment(ILoggerService logger)
        {
            _logger = logger;
        }

        public void ProcessPayment(decimal amount)
        {
            // Logic xử lý thanh toán thẻ (ví dụ: gọi API ngân hàng)
            _logger.LogInfo($"[Payment] Processing CREDIT CARD payment of {amount:C}");
        }
    }
}
