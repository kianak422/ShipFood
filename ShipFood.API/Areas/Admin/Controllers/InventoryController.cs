using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShipFood.API.Data;
using ShipFood.API.Models;

namespace ShipFood.API.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class InventoryController : Controller
    {
        private readonly AppDbContext _context;

        public InventoryController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(string keyword, string status)
        {
           var query = _context.TbTonKho
           .Include(x => x.MonAn)
           .AsQueryable();

          // Tìm kiếm tên món
            if (!string.IsNullOrEmpty(keyword))
            {
               query = query.Where(x =>
                x.MonAn != null &&
                x.MonAn.Tenmon != null &&
                x.MonAn.Tenmon.Contains(keyword));
            }

        // Lọc trạng thái
         switch (status)
           {
            case "many":
            query = query.Where(x => x.SoLuongTon > 30);
            break;

            case "medium":
            query = query.Where(x =>
                x.SoLuongTon >= 10 &&
                x.SoLuongTon <= 30);
            break;

            case "low":
                query = query.Where(x =>
                    x.SoLuongTon > 0 &&
                    x.SoLuongTon < 10);
                break;

             case "empty":
                query = query.Where(x =>
                   x.SoLuongTon == 0);
                break;
            }

            var data = query
            .OrderBy(x => x.MonAn!.Tenmon)
            .ToList();

            return View(data);
        }

        public IActionResult Create()
        {
            ViewBag.MonAn = _context.TbMonAn.ToList();
            return View();
        }

     [HttpPost]
    public IActionResult Create(TbTonKho model)
    {
        var kho = _context.TbTonKho
        .FirstOrDefault(x => x.Mamon == model.Mamon);

        if (kho == null)
        {
        kho = new TbTonKho
        {
           Mamon = model.Mamon,
           SoLuongNhap = model.SoLuongNhap,
           SoLuongTon = model.SoLuongNhap,
           NgayCapNhat = DateTime.Now
        };

          _context.TbTonKho.Add(kho);
       }
       else
       {
           kho.SoLuongNhap += model.SoLuongNhap;
           kho.SoLuongTon += model.SoLuongNhap;
           kho.NgayCapNhat = DateTime.Now;
        }

      _context.SaveChanges();

      return RedirectToAction(nameof(Index));
    }

        public IActionResult Delete(int MaKho)
        {
            var item = _context.TbTonKho.Find(MaKho);

            if (item != null)
            {
                _context.TbTonKho.Remove(item);
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Edit
        public IActionResult Edit(int MaKho)
        {
            var item = _context.TbTonKho
                .Include(x => x.MonAn)
                .FirstOrDefault(x => x.MaKho == MaKho);

            if (item == null)
            return NotFound();

            ViewBag.MonAn = _context.TbMonAn.ToList();

            return View(item);
        }

         // POST: Edit
        [HttpPost]
        public IActionResult Edit(TbTonKho model)
        {
           var item = _context.TbTonKho
           .FirstOrDefault(x => x.MaKho == model.MaKho);

           if (item == null)
               return NotFound();

            item.Mamon = model.Mamon;
            item.SoLuongTon = model.SoLuongTon;
            item.SoLuongNhap = model.SoLuongNhap;
            item.NgayCapNhat = DateTime.Now;

            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}