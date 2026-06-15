using Microsoft.AspNetCore.Mvc;
using ShipFood.API.Data;
using ShipFood.API.Models;

namespace ShipFood.API.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class VoucherController : Controller
    {
        private readonly AppDbContext _context;

        public VoucherController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View(_context.TbVoucher.ToList());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(TbVoucher voucher)
        {
            if (!ModelState.IsValid)
                return View(voucher);

            _context.TbVoucher.Add(voucher);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var voucher = _context.TbVoucher.Find(id);

            if (voucher != null)
            {
                _context.TbVoucher.Remove(voucher);
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}