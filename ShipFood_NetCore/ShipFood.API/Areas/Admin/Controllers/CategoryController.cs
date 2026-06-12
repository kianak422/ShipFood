using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using ShipFood.API.Filters;
using ShipFood.API.Models;
using ShipFood.API.Repositories;

namespace ShipFood.API.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AdminAuth]
    public class CategoryController(IRepository<TbDanhMuc> categoryRepo) : Controller
    {
        private readonly IRepository<TbDanhMuc> _categoryRepo = categoryRepo;

        public async Task<IActionResult> Index()
        {
            var categories = await _categoryRepo.GetAllAsync();
            return View(categories);
        }

        // GET: Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TbDanhMuc category, IFormFile? image)
        {
            if (ModelState.IsValid)
            {
                if (image != null && image.Length > 0)
                {
                    // Save image
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/upload/categories", fileName);

                    // Create directory if not exists
                    Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await image.CopyToAsync(stream);
                    }
                    category.Hinhanh = "/upload/categories/" + fileName;
                }

                await _categoryRepo.AddAsync(category);
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Edit
        public async Task<IActionResult> Edit(int id)
        {
            var category = await _categoryRepo.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TbDanhMuc category, IFormFile? image)
        {
            if (id != category.Madanhmuc)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Fetch existing entity (tracked)
                    var existingCategory = await _categoryRepo.GetByIdAsync(id);
                    if (existingCategory == null)
                    {
                        return NotFound();
                    }

                    // Update properties
                    existingCategory.Tendanhmuc = category.Tendanhmuc;
                    existingCategory.Mota = category.Mota;

                    if (image != null && image.Length > 0)
                    {
                        // Save image
                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/upload/categories", fileName);

                        // Create directory if not exists
                        Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await image.CopyToAsync(stream);
                        }
                        existingCategory.Hinhanh = "/upload/categories/" + fileName;
                    }
                    // Else: keep existingCategory.Hinhanh

                    await _categoryRepo.UpdateAsync(existingCategory);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (await _categoryRepo.GetByIdAsync(id) == null)
                    {
                        return NotFound();
                    }
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Delete
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _categoryRepo.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _categoryRepo.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
