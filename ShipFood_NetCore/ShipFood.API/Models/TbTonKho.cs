using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShipFood.API.Models
{
    [Table("tbTonKho")]
    public class TbTonKho
    {
        [Key]
        [Column("makho")]
        public int MaKho { get; set; }

        [Column("mamon")]
        public int Mamon { get; set; }

        [Column("soluongton")]
        public int SoLuongTon { get; set; }

        [Column("soluongnhap")]
        public int SoLuongNhap { get; set; }

        [Column("ngaycapnhat")]
        public DateTime? NgayCapNhat { get; set; }

        public decimal GiaNhap { get; set; }

        public decimal GiaBan { get; set; }

        [ForeignKey(nameof(Mamon))]
        public TbMonAn? MonAn { get; set; }
    }
}