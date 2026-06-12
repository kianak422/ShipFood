using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShipFood.API.Models
{
    [Table("tbShipper")]
    public class TbShipper
    {
        [Key]
        [Column("userid")]
        [ForeignKey("TbUser")]
        public int Userid { get; set; }

        [Column("tenshipper")]
        public string? Tenshipper { get; set; }

        [Column("diachi")]
        public string? Diachi { get; set; }

        [Column("diemdanhgia")]
        public decimal? Diemdanhgia { get; set; }

        [Column("soluotdanhgia")]
        public int? Soluotdanhgia { get; set; }

        [Column("trangthai")]
        public string? Trangthai { get; set; }

        [Column("hinhanh")]
        public string? Hinhanh { get; set; }

        public virtual TbUser? TbUser { get; set; }
    }
}
