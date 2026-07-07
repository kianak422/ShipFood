using Microsoft.EntityFrameworkCore;
using ShipFood.API.Data;
using ShipFood.API.Services.Inventory;

namespace ShipFood.API.Services.Order
{
    public class OrderService : IOrderService
    {
        private readonly AppDbContext _context;
        private readonly IInventoryService _inventory;

        public OrderService(
            AppDbContext context,
            IInventoryService inventory)
        {
            _context = context;
            _inventory = inventory;
        }

        public async Task<bool> CompleteOrderAsync(int orderId)
        {
            var order = await _context.TbDonHang
                .FirstOrDefaultAsync(x => x.Madh == orderId);

            if (order == null)
                return false;

            if (order.Trangthai == "Hoàn thành")
                return true;

            var details = _context.TbChiTietDonHang
                .Where(x => x.Madh == orderId)
                .ToList();

            foreach (var item in details)
            {
                bool ok = await _inventory.ExportAsync(
                    item.Mamon,
                    item.Soluong);

                if (!ok)
                    return false;
            }

            order.Trangthai = "Hoàn thành";

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> CancelOrderAsync(int orderId)
        {
            var order = await _context.TbDonHang
                .FirstOrDefaultAsync(x => x.Madh == orderId);

            if (order == null)
                return false;

            if (order.Trangthai == "Hủy bỏ")
                return true;

            if (order.Trangthai == "Hoàn thành")
            {
                var details = _context.TbChiTietDonHang
                    .Where(x => x.Madh == orderId)
                    .ToList();

                foreach (var item in details)
                {
                    await _inventory.CancelOrderAsync(
                        item.Mamon,
                        item.Soluong);
                }
            }

            order.Trangthai = "Hủy bỏ";

            await _context.SaveChangesAsync();

            return true;
        }
    }
}