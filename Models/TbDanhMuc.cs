using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShipFood.API.Models
{
    [Table("tbDanhMuc")]
    public class TbDanhMuc
    {
        [Key]
        [Column("madanhmuc")]
        public int Madanhmuc { get; set; }

        [Column("tendanhmuc")]
        public string? Tendanhmuc { get; set; }

        [Column("mota")]
        public string? Mota { get; set; }

        [Column("hinhanh")]
        public string? Hinhanh { get; set; }
    }
}
