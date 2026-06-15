using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShipFood.API.Filters;
using ShipFood.API.Models;
using ShipFood.API.Repositories;

namespace ShipFood.API.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AdminAuth]
    public class ProductController(ITbMonAnRepository foodRepo, IRepository<TbDanhMuc> categoryRepo) : Controller
    {
        private readonly ITbMonAnRepository _foodRepo = foodRepo;
        private readonly IRepository<TbDanhMuc> _categoryRepo = categoryRepo;

        public async Task<IActionResult> Index()
        {
            var foods = await _foodRepo.GetAllAsync();
            // Nạp thông tin danh mục nếu cần hiển thị tên danh mục
            var categories = await _categoryRepo.GetAllAsync();
            ViewData["Categories"] = categories.ToDictionary(c => c.Madanhmuc, c => c.Tendanhmuc);

            return View(foods);
        }

        // GET: Create
        public async Task<IActionResult> Create()
        {
            ViewData["Madanhmuc"] = new SelectList(await _categoryRepo.GetAllAsync(), "Madanhmuc", "Tendanhmuc");
            return View();
        }

        // POST: Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TbMonAn food, IFormFile? image)
        {
            if (ModelState.IsValid)
            {
                if (image != null && image.Length > 0)
                {
                    // Save image
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/upload/products", fileName);
                    
                    // Create directory if not exists
                    Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await image.CopyToAsync(stream);
                    }
                    food.Hinhanh = "/upload/products/" + fileName;
                }

                await _foodRepo.AddAsync(food);
                return RedirectToAction(nameof(Index));
            }
            ViewData["Madanhmuc"] = new SelectList(await _categoryRepo.GetAllAsync(), "Madanhmuc", "Tendanhmuc", food.Madanhmuc);
            return View(food);
        }

        // GET: Edit
        public async Task<IActionResult> Edit(int id)
        {
            var food = await _foodRepo.GetByIdAsync(id);
            if (food == null)
            {
                return NotFound();
            }
            ViewData["Madanhmuc"] = new SelectList(await _categoryRepo.GetAllAsync(), "Madanhmuc", "Tendanhmuc", food.Madanhmuc);
            return View(food);
        }

        // POST: Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TbMonAn food, IFormFile? image)
        {
            if (id != food.Mamon)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Fetch existing entity (tracked)
                    var existingFood = await _foodRepo.GetByIdAsync(id);
                    if (existingFood == null)
                    {
                        return NotFound();
                    }

                    // Update properties
                    existingFood.Tenmon = food.Tenmon;
                    existingFood.Giatien = food.Giatien;
                    existingFood.Mota = food.Mota;
                    existingFood.Madanhmuc = food.Madanhmuc;
                    // Note: Update other properties if necessary

                    if (image != null && image.Length > 0)
                    {
                        // Save new image
                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/upload/products", fileName);

                        // Create directory if not exists
                        Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await image.CopyToAsync(stream);
                        }
                        // Delete old image
                        if (!string.IsNullOrEmpty(existingFood.Hinhanh))
                        {
                            var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", existingFood.Hinhanh.TrimStart('/'));
                            if (System.IO.File.Exists(oldFilePath))
                            {
                                System.IO.File.Delete(oldFilePath);
                            }
                        }

                        existingFood.Hinhanh = "/upload/products/" + fileName;
                    }
                    // Else: keep existingFood.Hinhanh as is

                    await _foodRepo.UpdateAsync(existingFood);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (await _foodRepo.GetByIdAsync(id) == null)
                    {
                        return NotFound();
                    }
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["Madanhmuc"] = new SelectList(await _categoryRepo.GetAllAsync(), "Madanhmuc", "Tendanhmuc", food.Madanhmuc);
            return View(food);
        }

        // GET: Delete
        public async Task<IActionResult> Delete(int id)
        {
            var food = await _foodRepo.GetByIdAsync(id);
            if (food == null)
            {
                return NotFound();
            }
            return View(food);
        }

        // POST: Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _foodRepo.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
