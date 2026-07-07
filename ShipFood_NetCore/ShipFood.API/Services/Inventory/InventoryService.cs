using Microsoft.EntityFrameworkCore;
using ShipFood.API.Data;

namespace ShipFood.API.Services.Inventory
{
    public class InventoryService : IInventoryService
    {
        private readonly AppDbContext _context;

        public InventoryService(AppDbContext context)
        {
            _context = context;
        }

        // Kiểm tra tồn
        public async Task<int> GetCurrentStockAsync(int foodId)
        {
            var stock = await _context.TbTonKho
                .FirstOrDefaultAsync(x => x.Mamon == foodId);

            if (stock == null)
                return 0;

            return stock.SoLuongTon;
        }

        // Kiểm tra đủ hàng
        public async Task<bool> CheckStockAsync(int foodId, int quantity)
        {
            return await GetCurrentStockAsync(foodId) >= quantity;
        }

        // Trừ kho
        public async Task<bool> ExportAsync(int foodId, int quantity)
        {
            var stock = await _context.TbTonKho
                .FirstOrDefaultAsync(x => x.Mamon == foodId);

            if (stock == null)
                return false;

            if (stock.SoLuongTon < quantity)
                return false;

            stock.SoLuongTon -= quantity;

            await _context.SaveChangesAsync();

            return true;
        }

        // Nhập kho
        public async Task<bool> ImportAsync(int foodId, int quantity)
        {
            var stock = await _context.TbTonKho
                .FirstOrDefaultAsync(x => x.Mamon == foodId);

            if (stock == null)
                return false;

            stock.SoLuongTon += quantity;
            stock.SoLuongNhap += quantity;

            await _context.SaveChangesAsync();

            return true;
        }

        // Hoàn kho
        public async Task<bool> CancelOrderAsync(int foodId, int quantity)
        {
            var stock = await _context.TbTonKho
                .FirstOrDefaultAsync(x => x.Mamon == foodId);

            if (stock == null)
                return false;

            stock.SoLuongTon += quantity;

            await _context.SaveChangesAsync();

            return true;
        }
    }
}