namespace ShipFood.API.Services.Order
{
    // Command Interface
    public interface IOrderCommand
    {
        Task ExecuteAsync();
    }
}
