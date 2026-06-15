using ShipFood.API.Services.Logger;

namespace ShipFood.API.Services.Payment
{
    // Factory Pattern: Tạo object thanh toán dựa trên loại hình thức thanh toán
    public class PaymentFactory
    {
        private readonly ILoggerService _logger;

        public PaymentFactory(ILoggerService logger)
        {
            _logger = logger;
        }

        public IPaymentProcessor CreatePaymentProcessor(string paymentMethod)
        {
            switch (paymentMethod.ToLower())
            {
                case "cash":
                    return new CashPayment(_logger);
                case "card":
                case "credit_card":
                    return new CreditCardPayment(_logger);
                default:
                    throw new ArgumentException("Invalid payment method", nameof(paymentMethod));
            }
        }
    }
}
