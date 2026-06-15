using Microsoft.AspNetCore.Mvc;
using ShipFood.API.Models;
using ShipFood.API.Repositories;

namespace ShipFood.API.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiOrderController : ControllerBase
    {
        private readonly IRepository<TbDonHang> _orderRepo;

        public ApiOrderController(IRepository<TbDonHang> orderRepo)
        {
            _orderRepo = orderRepo;
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
            {
                return BadRequest("Order is already cancelled.");
            }

            order.Trangthai = "Hủy bỏ";
            await _orderRepo.UpdateAsync(order);

            return Ok(new { message = "Order cancelled successfully.", orderId = id, status = order.Trangthai });
        }
    }
}
