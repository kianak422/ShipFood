namespace ShipFood.API.Services.Payment
{
    public interface IPaymentProcessor
    {
        void ProcessPayment(decimal amount);
    }
}
