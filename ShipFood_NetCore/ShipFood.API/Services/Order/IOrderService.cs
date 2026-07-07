using ShipFood.API.Models;

namespace ShipFood.API.Services.Order
{
    public interface IOrderService
    {
        Task<bool> CompleteOrderAsync(int orderId);

        Task<bool> CancelOrderAsync(int orderId);
    }
}