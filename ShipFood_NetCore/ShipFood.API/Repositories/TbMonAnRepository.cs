using Microsoft.EntityFrameworkCore;
using ShipFood.API.Data;
using ShipFood.API.Models;

namespace ShipFood.API.Repositories
{
    public interface ITbMonAnRepository : IRepository<TbMonAn>
    {
        Task<IEnumerable<TbMonAn>> GetMonAnByDanhMucAsync(int maDanhMuc);
    }

    public class TbMonAnRepository : Repository<TbMonAn>, ITbMonAnRepository
    {
        public TbMonAnRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<TbMonAn>> GetMonAnByDanhMucAsync(int maDanhMuc)
        {
            return await _context.TbMonAn.Where(f => f.Madanhmuc == maDanhMuc).ToListAsync();
        }
    }
}
