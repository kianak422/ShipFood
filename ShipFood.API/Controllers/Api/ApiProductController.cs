using Microsoft.AspNetCore.Mvc;
using ShipFood.API.Models;
using ShipFood.API.Repositories;

namespace ShipFood.API.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiProductController : ControllerBase
    {
        private readonly ITbMonAnRepository _foodRepo;

        public ApiProductController(ITbMonAnRepository foodRepo)
        {
            _foodRepo = foodRepo;
        }

        // GET: api/ApiProduct?search=ga
        [HttpGet]
        public async Task<IActionResult> GetProducts(string? search)
        {
            var foods = await _foodRepo.GetAllAsync();

            if (!string.IsNullOrEmpty(search))
            {
                foods = foods.Where(f => f.Tenmon != null && f.Tenmon.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            return Ok(foods);
        }

        // GET: api/ApiProduct/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var food = await _foodRepo.GetByIdAsync(id);

            if (food == null)
            {
                return NotFound();
            }

            return Ok(food);
        }

        // POST: api/ApiProduct
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromQuery] TbMonAn food)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _foodRepo.AddAsync(food);
            return CreatedAtAction("GetProduct", new { id = food.Mamon }, food);
        }

        // PUT: api/ApiProduct/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromQuery] TbMonAn food)
        {
            if (id != food.Mamon)
            {
                return BadRequest();
            }

            var existingFood = await _foodRepo.GetByIdAsync(id);
            if (existingFood == null)
            {
                return NotFound();
            }

            // Simple update mapping
            existingFood.Tenmon = food.Tenmon;
            existingFood.Giatien = food.Giatien;
            existingFood.Mota = food.Mota;
            existingFood.Madanhmuc = food.Madanhmuc;
            existingFood.Hinhanh = food.Hinhanh; // Optional: Handle if null?

            await _foodRepo.UpdateAsync(existingFood);

            return NoContent();
        }

        // DELETE: api/ApiProduct/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var food = await _foodRepo.GetByIdAsync(id);
            if (food == null)
            {
                return NotFound();
            }

            await _foodRepo.DeleteAsync(id);
            return NoContent();
        }
    }
}
