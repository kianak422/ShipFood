using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShipFood.API.Models
{
    [Table("tbDonHang")]
    public class TbDonHang
    {
        [Key]
        [Column("madh")]
        public int Madh { get; set; }

        [Column("ngaydathang")]
        public DateTime Ngaydathang { get; set; }

        [Column("trangthai")]
        public string Trangthai { get; set; } = "Đã đặt";

        [Column("tongtien")]
        public decimal Tongtien { get; set; }

        [Column("ghichu")]
        public string? Ghichu { get; set; }

        [Column("phiship")]
        public decimal Phiship { get; set; }

        [Column("mashipper")]
        public int? Mashipper { get; set; }

        // Relationships
        // Note: For strict mirroring, we might need full relationship mapping
        // Keeping it simple for now to ensure compile
    }
}
