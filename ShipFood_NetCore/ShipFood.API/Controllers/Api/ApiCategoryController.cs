using Microsoft.AspNetCore.Mvc;
using ShipFood.API.Models;
using ShipFood.API.Repositories;

namespace ShipFood.API.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiCategoryController(IRepository<TbDanhMuc> categoryRepo) : ControllerBase
    {
        private readonly IRepository<TbDanhMuc> _categoryRepo = categoryRepo;

        // GET: api/ApiCategory?search=com
        [HttpGet]
        public async Task<IActionResult> GetCategories(string? search)
        {
            IEnumerable<TbDanhMuc> categories;

            if (!string.IsNullOrEmpty(search))
            {
                categories = await _categoryRepo.FindAsync(c => c.Tendanhmuc != null && c.Tendanhmuc.Contains(search));
            }
            else
            {
                categories = await _categoryRepo.GetAllAsync();
            }

            return Ok(categories);
        }

        // GET: api/ApiCategory/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategory(int id)
        {
            var category = await _categoryRepo.GetByIdAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }
    }
}
