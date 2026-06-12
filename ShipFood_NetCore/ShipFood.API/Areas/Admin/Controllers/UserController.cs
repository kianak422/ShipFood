using Microsoft.AspNetCore.Mvc;
using ShipFood.API.Filters;
using ShipFood.API.Models;
using ShipFood.API.Repositories;

namespace ShipFood.API.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AdminAuth] // Proxy Pattern: Bảo vệ trang này
    public class UserController : Controller
    {
        private readonly IRepository<TbUser> _userRepo;

        public UserController(IRepository<TbUser> userRepo)
        {
            _userRepo = userRepo;
        }

        // Hiển thị danh sách người dùng
        public async Task<IActionResult> Index()
        {
            var users = await _userRepo.GetAllAsync();
            return View(users);
        }

        // Hiện form sửa Role
        public async Task<IActionResult> Edit(int id)
        {
            var user = await _userRepo.GetByIdAsync(id);
            if (user == null) return NotFound();
            return View(user);
        }

        // Xử lý cập nhật Role
        [HttpPost]
        public async Task<IActionResult> Edit(int id, string loaitaikhoan, string trangthai)
        {
            var user = await _userRepo.GetByIdAsync(id);
            if (user == null) return NotFound();

            user.Loaitaikhoan = loaitaikhoan;
            // user.Trangthai = trangthai; // Nếu có trường trạng thái

            await _userRepo.UpdateAsync(user);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Delete(int id)
        {
            await _userRepo.DeleteAsync(id);
            return RedirectToAction("Index");
        }
    }
}
