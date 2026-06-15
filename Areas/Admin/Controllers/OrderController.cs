using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShipFood.API.Filters;
using ShipFood.API.Models;
using ShipFood.API.Repositories;

namespace ShipFood.API.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AdminAuth]
    public class OrderController : Controller
    {
        private readonly IRepository<TbDonHang> _orderRepo;

        public OrderController(IRepository<TbDonHang> orderRepo)
        {
            _orderRepo = orderRepo;
        }

        // List Orders
        public async Task<IActionResult> Index()
        {
            var orders = await _orderRepo.GetAllAsync();
            return View(orders.OrderByDescending(o => o.Ngaydathang).ToList());
        }

        // Edit Status (We only allow editing status and maybe notes)
        public async Task<IActionResult> Edit(int id)
        {
            var order = await _orderRepo.GetByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            // Define possible statuses
            var statuses = new List<string> { "Đã đặt", "Đang xử lý", "Đang giao", "Đã giao", "Hoàn thành", "Hủy bỏ" };
            ViewData["Trangthai"] = new SelectList(statuses, order.Trangthai);

            return View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TbDonHang order)
        {
            if (id != order.Madh)
            {
                return NotFound();
            }

            // Load existing to protect other fields if needed, or just update allowed fields
            var existingOrder = await _orderRepo.GetByIdAsync(id);
            if (existingOrder == null)
            {
                return NotFound();
            }

            // Update allowed fields
            existingOrder.Trangthai = order.Trangthai;
            existingOrder.Ghichu = order.Ghichu;
            // Optionally update shipper if we implemented assignment
            
            await _orderRepo.UpdateAsync(existingOrder);
            return RedirectToAction(nameof(Index));
        }

        // POST: Cancel Order
        public async Task<IActionResult> Cancel(int id)
        {
            var order = await _orderRepo.GetByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            order.Trangthai = "Hủy bỏ";
            await _orderRepo.UpdateAsync(order);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            var order = await _orderRepo.GetByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            // For now, details view just shows the same info + maybe items if we had navigation property working
            return View(order);
        }
    }
}
