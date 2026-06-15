using Microsoft.EntityFrameworkCore;
using ShipFood.API.Models;

namespace ShipFood.API.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {

        public DbSet<TbUser> TbUser { get; set; } = default!;
        public DbSet<TbDanhMuc> TbDanhMuc { get; set; } = default!;
        public DbSet<TbMonAn> TbMonAn { get; set; } = default!;
        public DbSet<TbDonHang> TbDonHang { get; set; } = default!;
        public DbSet<TbChiTietDonHang> TbChiTietDonHang { get; set; } = default!;
        public DbSet<TbKhachHang> TbKhachHang { get; set; } = default!;
        public DbSet<TbShipper> TbShipper { get; set; } = default!;
        public DbSet<TbQuanAn> TbQuanAn { get; set; } = default!;
        public DbSet<TbVoucher> TbVoucher { get; set; } = default!;
        public DbSet<TbTonKho> TbTonKho { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure entity relationships and properties if needed
            // Example:
            // modelBuilder.Entity<Food>()
            //    .HasOne(f => f.Category)
            //    .WithMany(c => c.Foods)
            //    .HasForeignKey(f => f.CategoryId);
        }
    }
}
