using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShipFood.API.Filters;
using ShipFood.API.Models;
using ShipFood.API.Repositories;
using ShipFood.API.Services.Inventory;
using ShipFood.API.Data;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Eventing.Reader;

namespace ShipFood.API.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AdminAuth]
    public class OrderController : Controller
    {
        private readonly IRepository<TbDonHang> _orderRepo;
        private readonly IInventoryService _inventoryService;
        private readonly AppDbContext _context;

        public OrderController(
            IRepository<TbDonHang> orderRepo,
            AppDbContext context,
            IInventoryService inventoryService)
        {
            _orderRepo = orderRepo;
            _context = context;
            _inventoryService = inventoryService;
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

            // Lưu trạng thái cũ
            string oldStatus = existingOrder.Trangthai;

            //Cập nhật trạng thái mới
            existingOrder.Trangthai = order.Trangthai;
            existingOrder.Ghichu = order.Ghichu;

            // Khi đơn chuyển sang Hoàn thành => Trừ kho
            if (oldStatus != "Hoàn thành" && order.Trangthai == "Hoàn thành")
            {
                var details = await _context.TbChiTietDonHang
                    .Where(d => d.Madh == id)
                    .ToListAsync();

                foreach (var item in details)
                {
                    bool ok = await _inventoryService.ExportAsync(item.Mamon, item.Soluong);

                    if (!ok)
                    {
                        ModelState.AddModelError("", $"Không đủ hàng cho món {item.Mamon}. Vui lòng kiểm tra kho.");
                        // Reload the status dropdown
                        var statuses = new List<string> 
                        { 
                            "Đã đặt", 
                            "Đang xử lý", 
                            "Đang giao", 
                            "Đã giao", 
                            "Hoàn thành", 
                            "Hủy bỏ" 
                        };
                        ViewData["Trangthai"] = new SelectList(statuses, existingOrder.Trangthai);
                        return View(existingOrder);
                    }
                }
            }
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

            // Nếu đơn đã hoàn thành thì mới hoàn kho
            if (order.Trangthai == "Hoàn thành")
            {
                var details = await _context.TbChiTietDonHang
                    .Where(d => d.Madh == id)
                    .ToListAsync();

                foreach (var item in details)
                {
                    await _inventoryService.CancelOrderAsync(item.Mamon, item.Soluong);
                }
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
