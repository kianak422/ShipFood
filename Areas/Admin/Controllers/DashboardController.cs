using Microsoft.AspNetCore.Mvc;
using ShipFood.API.Filters;
using ShipFood.API.Services.Facade;

namespace ShipFood.API.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AdminAuth] // Proxy Pattern: Xác thực trước khi truy cập
    public class DashboardController : Controller
    {
        private readonly AdminDashboardFacade _dashboardFacade;

        public DashboardController(AdminDashboardFacade dashboardFacade)
        {
            _dashboardFacade = dashboardFacade;
        }

        public async Task<IActionResult> Index()
        {
            // Hàm GetDashboardStatsAsync có dùng await bên trong, nên ở đây dùng await là đúng
            var stats = await _dashboardFacade.GetDashboardStatsAsync();
            return View(stats);
        }
    }
}
