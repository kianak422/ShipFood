using ShipFood.API.Models;

namespace ShipFood.API.ViewModels
{
    public class AdminDashboardViewModel
    {
        public int TotalUsers { get; set; }
        public int TotalOrders { get; set; }
        public int TotalProducts { get; set; }
        public decimal TotalRevenue { get; set; }
        public List<TbDonHang> RecentOrders { get; set; } = new List<TbDonHang>();
    }
}
