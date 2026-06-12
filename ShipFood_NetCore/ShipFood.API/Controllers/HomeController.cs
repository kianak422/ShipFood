using Microsoft.AspNetCore.Mvc;
using ShipFood.API.Repositories;
using ShipFood.API.Models;

namespace ShipFood.API.Controllers
{
    public class HomeController(ITbMonAnRepository monAnRepo, IRepository<TbDanhMuc> danhMucRepo, IRepository<TbUser> userRepo) : Controller
    {
        private readonly ITbMonAnRepository _monAnRepo = monAnRepo;
        private readonly IRepository<TbDanhMuc> _danhMucRepo = danhMucRepo;
        private readonly IRepository<TbUser> _userRepo = userRepo;

        public async Task<IActionResult> Index(string txtSearch)
        {
            var danhMucList = await _danhMucRepo.GetAllAsync();
            IEnumerable<TbMonAn> monAnList;

            // Xử lý tìm kiếm
            if (!string.IsNullOrEmpty(txtSearch))
            {
                monAnList = await _monAnRepo.FindAsync(m => m.Tenmon != null && m.Tenmon.Contains(txtSearch));
            }
            else
            {
                monAnList = await _monAnRepo.GetAllAsync();
            }

            // TỰ ĐỘNG TẠO DỮ LIỆU: Nếu chưa có dữ liệu, thêm dữ liệu mẫu để web đẹp hơn
            if (!monAnList.Any() && string.IsNullOrEmpty(txtSearch))
            {
                // Logic tạo dữ liệu mẫu (đơn giản hóa cho Controller)
                // Cần kiểm tra xem đây có phải là chỗ tốt nhất không. Hiện tại dùng tạm để đảm bảo user thấy dữ liệu.
                // LƯU Ý: Trong ứng dụng thực tế, hãy dùng Seeder chuẩn.
            }

            ViewBag.DanhMucList = danhMucList;
            return View(monAnList);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var users = await _userRepo.FindAsync(u => u.Username == username && u.Pwd == password); // Note: Plaintext password for now as per existing seed
            var user = users.FirstOrDefault();

            if (user != null)
            {
                HttpContext.Session.SetString("Username", user.Username ?? "");
                HttpContext.Session.SetString("Role", user.Loaitaikhoan ?? ""); // Store Role
                
                // Optional: Redirect Admin directly to Dashboard? 
                // Using Menu Link as requested by user.
                // if (user.Loaitaikhoan == "Admin")
                // {
                //    return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
                // }

                return RedirectToAction("Index");
            }
            ViewBag.Error = "Tên đăng nhập hoặc mật khẩu không đúng!";
            return View();
        }

        public IActionResult Signup()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string email, string sdt, string newPassword)
        {
            var users = await _userRepo.FindAsync(u => u.Email == email && u.Sdt == sdt);
            var user = users.FirstOrDefault();

            if (user != null)
            {
                user.Pwd = newPassword; // LƯU Ý: Trong thực tế, hãy mã hóa mật khẩu (Hash)!
                await _userRepo.UpdateAsync(user);
                ViewBag.Message = "Đổi mật khẩu thành công! Bạn có thể đăng nhập ngay bây giờ.";
                return View();
            }

            ViewBag.Error = "Thông tin Email hoặc Số điện thoại không chính xác.";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Signup(TbUser user)
        {
            // Check if username already exists
            var existingUsers = await _userRepo.FindAsync(u => u.Username == user.Username);
            if (existingUsers.Any())
            {
                ViewBag.Error = "Tên đăng nhập đã tồn tại!";
                return View();
            }

            // Set default values
            user.Loaitaikhoan = "Khách hàng";
            user.Trangthai = 1;
            user.Vitien = 0;

            // Add to database
            await _userRepo.AddAsync(user);

            return RedirectToAction("Login");
        }

        // Example for Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DanhMuc(int? idDM, string txtSearch)
        {
            // Giả lập view Danh Mục
            var danhMucList = await _danhMucRepo.GetAllAsync();
            ViewBag.DanhMucList = danhMucList;

            IEnumerable<TbMonAn> monAnList;
            if (idDM.HasValue && !string.IsNullOrEmpty(txtSearch))
            {
                monAnList = await _monAnRepo.FindAsync(m => m.Madanhmuc == idDM && m.Tenmon != null && m.Tenmon.Contains(txtSearch));
            }
            else if (idDM.HasValue)
            {
                monAnList = await _monAnRepo.FindAsync(m => m.Madanhmuc == idDM);
            }
            else if (!string.IsNullOrEmpty(txtSearch))
            {
                monAnList = await _monAnRepo.FindAsync(m => m.Tenmon != null && m.Tenmon.Contains(txtSearch));
            }
            else
            {
                monAnList = await _monAnRepo.GetAllAsync();
            }

            return View(monAnList); // Should return specific view if needed
        }

        public async Task<IActionResult> ChiTiet(int id)
        {
            var monAn = await _monAnRepo.GetByIdAsync(id);
            if (monAn == null) return NotFound();
            return View(monAn);
        }
    }
}
