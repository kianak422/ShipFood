using Microsoft.EntityFrameworkCore;
using ShipFood.API.Data;
using ShipFood.API.Models;
using System.Collections.Generic;
using System.Linq;

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
            stock.NgayCapNhat = DateTime.Now;

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
            stock.NgayCapNhat = DateTime.Now;

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
            stock.NgayCapNhat = DateTime.Now;

            await _context.SaveChangesAsync();

            return true;
        }

        // Reorder Point: Kiểm tra xem có cần đặt hàng lại không
        public async Task<bool> ShouldReorderAsync(int foodId)
        {
            var stock = await _context.TbTonKho
                .FirstOrDefaultAsync(x => x.Mamon == foodId);

            if (stock == null)
                return false;

            return stock.SoLuongTon <= stock.ReorderPoint;
        }

        // Lấy danh sách món cần đặt hàng lại
        public async Task<List<TbTonKho>> GetLowStockItemsAsync()
        {
            return await _context.TbTonKho
                .Include(x => x.MonAn)
                .Where(x => x.SoLuongTon <= x.ReorderPoint)
                .ToListAsync();
        }

        // FIFO: First In First Out - Tính giá vốn theo phương pháp nhập trước xuất trước
        public async Task<decimal> CalculateCostFIFOAsync(int foodId, int quantity)
        {
            var stock = await _context.TbTonKho
                .FirstOrDefaultAsync(x => x.Mamon == foodId);

            if (stock == null)
                return 0;

            // FIFO: Giá vốn = Giá nhập hiện tại (đơn giản hóa cho mô hình hiện tại)
            // Trong thực tế cần theo dõi từng lô nhập hàng
            return stock.GiaNhap * quantity;
        }

        // LIFO: Last In First Out - Tính giá vốn theo phương pháp nhập sau xuất trước
        public async Task<decimal> CalculateCostLIFOAsync(int foodId, int quantity)
        {
            var stock = await _context.TbTonKho
                .FirstOrDefaultAsync(x => x.Mamon == foodId);

            if (stock == null)
                return 0;

            // LIFO: Giá vốn = Giá nhập hiện tại (đơn giản hóa cho mô hình hiện tại)
            // Trong thực tế cần theo dõi từng lô nhập hàng
            return stock.GiaNhap * quantity;
        }

        // Tính lãi cho đơn hàng đã thanh toán (không tính đơn hủy)
        public async Task<decimal> CalculateProfitAsync(int orderId)
        {
            var order = await _context.TbDonHang
                .FirstOrDefaultAsync(x => x.Madh == orderId);

            if (order == null)
                return 0;

            // Chỉ tính lãi cho đơn hàng đã thanh toán (không phải "Hủy bỏ")
            if (order.Trangthai == "Hủy bỏ")
                return 0;

            var details = await _context.TbChiTietDonHang
                .Include(x => x.MonAn)
                .Where(x => x.Madh == orderId)
                .ToListAsync();

            decimal totalRevenue = 0;
            decimal totalCost = 0;

            foreach (var detail in details)
            {
                // Doanh thu: Giá bán * Số lượng
                totalRevenue += detail.Dongia * detail.Soluong;

                // Giá vốn: Giá nhập * Số lượng
                var stock = await _context.TbTonKho
                    .FirstOrDefaultAsync(x => x.Mamon == detail.Mamon);
                
                if (stock != null)
                {
                    totalCost += stock.GiaNhap * detail.Soluong;
                }
            }

            // Lãi = Doanh thu - Giá vốn - Phí ship
            return totalRevenue - totalCost - order.Phiship;
        }
    }
}