using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShipFood.API.Models;
using ShipFood.API.Repositories;
using ShipFood.API.Data;
using ShipFood.API.Services.Inventory;

namespace ShipFood.API.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiOrderController : ControllerBase
    {
        private readonly IRepository<TbDonHang> _orderRepo;

        private readonly AppDbContext _context;

        private readonly IInventoryService _inventoryService;

        public ApiOrderController(
            IRepository<TbDonHang> orderRepo, 
            AppDbContext context, 
            IInventoryService inventoryService)
        {
            _orderRepo = orderRepo;
            _context = context;
            _inventoryService = inventoryService;
        }

        // GET: api/ApiOrder?search=hanoi
        [HttpGet]
        public async Task<IActionResult> GetOrders(string? search)
        {
            var orders = await _orderRepo.GetAllAsync();

            if (!string.IsNullOrEmpty(search))
            {
                // Search by Status or Note
                orders = orders.Where(o => 
                    (o.Trangthai != null && o.Trangthai.Contains(search, StringComparison.OrdinalIgnoreCase)) ||
                    (o.Ghichu != null && o.Ghichu.Contains(search, StringComparison.OrdinalIgnoreCase))
                ).ToList();
            }

            return Ok(orders);
        }

        // GET: api/ApiOrder/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrder(int id)
        {
            var order = await _orderRepo.GetByIdAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }

        // POST: api/ApiOrder/5/cancel
        [HttpPost("{id}/cancel")]
        public async Task<IActionResult> CancelOrder(int id)
        {
            var order = await _orderRepo.GetByIdAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            
            if (order.Trangthai == "Hủy bỏ")
               return BadRequest("Order is already cancelled.");
            
            // Lấy các món trong đơn hàng
            var details = await _context.TbChiTietDonHang
                .Where(d => d.Madh == id)
                .ToListAsync();

            foreach (var item in details)
            {
                // Hoàn trả số lượng món ăn vào kho
                await _inventoryService.ImportAsync(item.Mamon, item.Soluong);
            }

            order.Trangthai = "Hủy bỏ";
            await _orderRepo.UpdateAsync(order);

            return Ok(new 
            { 
                message = "Order cancelled successfully.", 
                orderId = id, 
                status = order.Trangthai 
            });
        }
    }
}
