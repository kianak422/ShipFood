using System.Threading.Tasks;

namespace ShipFood.API.Services.Inventory
{
    public interface IInventoryService
    {
        Task<int> GetCurrentStockAsync(int foodId);

        Task<bool> CheckStockAsync(int foodId, int quantity);

        Task<bool> ExportAsync(int foodId, int quantity);

        Task<bool> ImportAsync(int foodId, int quantity);

        Task<bool> CancelOrderAsync(int foodId, int quantity);
    }
}