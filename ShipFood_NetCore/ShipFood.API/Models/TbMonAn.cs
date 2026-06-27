using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShipFood.API.Models
{
    [Table("tbMonAn")]
    public class TbMonAn
    {
        [Key]
        [Column("mamon")]
        public int Mamon { get; set; }

        [Required]
        [Column("tenmon")]
        public string? Tenmon { get; set; }

        [Column("mota")]
        public string? Mota { get; set; }

        [Required]
        [Column("giatien")]
        public decimal Giatien { get; set; }

        [Column("hinhanh")]
        public string? Hinhanh { get; set; }

        [Column("maquanan")]
        public int? Maquanan { get; set; }

        [Column("madanhmuc")]
        public int? Madanhmuc { get; set; }

        [Column("soluongban")]
        public int Soluongban { get; set; }

        [Column("phantramgiam")]
        public int Phantramgiam { get; set; }

        [Column("ngaytao")]
        public DateTime Ngaytao { get; set; }

        [Column("noibat")]
        public bool Noibat { get; set; }

        public decimal GiaNhap { get; set; }

        public decimal GiaBan { get; set; }

        [NotMapped]
        [Column("soluong")]
        [Required]
        public int SoLuong { get; set; }

        // Mẫu Visitor: Cho phép "Khách" (Visitor) viếng thăm để thực hiện tính toán
        public void Accept(Services.Visitor.ICartVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
