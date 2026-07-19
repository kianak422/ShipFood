using System.Collections.Generic;
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

        // Reorder Point methods
        Task<bool> ShouldReorderAsync(int foodId);
        Task<List<Models.TbTonKho>> GetLowStockItemsAsync();

        // FIFO/LIFO cost calculation
        Task<decimal> CalculateCostFIFOAsync(int foodId, int quantity);
        Task<decimal> CalculateCostLIFOAsync(int foodId, int quantity);

        // Profit calculation
        Task<decimal> CalculateProfitAsync(int orderId);
    }
}