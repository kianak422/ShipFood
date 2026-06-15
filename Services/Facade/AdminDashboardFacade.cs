using ShipFood.API.Models;
using ShipFood.API.Repositories;
using ShipFood.API.ViewModels;

namespace ShipFood.API.Services.Facade
{
    public class AdminDashboardFacade
    {
        private readonly IRepository<TbUser> _userRepo;
        private readonly IRepository<TbDonHang> _orderRepo;
        private readonly ITbMonAnRepository _foodRepo;

        public AdminDashboardFacade(
            IRepository<TbUser> userRepo,
            IRepository<TbDonHang> orderRepo,
            ITbMonAnRepository foodRepo)
        {
            _userRepo = userRepo;
            _orderRepo = orderRepo;
            _foodRepo = foodRepo;
        }

        public async Task<AdminDashboardViewModel> GetDashboardStatsAsync()
        {
            var users = await _userRepo.GetAllAsync();
            var orders = await _orderRepo.GetAllAsync();
            var foods = await _foodRepo.GetAllAsync();

            return new AdminDashboardViewModel
            {
                TotalUsers = users.Count(),
                TotalOrders = orders.Count(),
                TotalProducts = foods.Count(),
                // Tính tổng doanh thu 
                // Tính tổng doanh thu (Trừ đơn đã hủy)
                TotalRevenue = orders.Where(o => o.Trangthai != "Hủy bỏ" && o.Trangthai != "Đã hủy").Sum(o => o.Tongtien),
                // Lấy 10 đơn hàng mới nhất (nhiều hơn để tránh nhầm lẫn)
                RecentOrders = orders.OrderByDescending(o => o.Ngaydathang).Take(10).ToList()
            };
        }
    }
}
