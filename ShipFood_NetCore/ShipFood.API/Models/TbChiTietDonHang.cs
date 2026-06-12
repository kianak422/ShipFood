using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShipFood.API.Models
{
    [Table("tbChiTietDonHang")]
    public class TbChiTietDonHang
    {
        [Key]
        [Column("mactdh")]
        public int Mactdh { get; set; }

        [Column("madh")]
        public int Madh { get; set; }

        [Column("mamon")]
        public int Mamon { get; set; }

        [Column("soluong")]
        public int Soluong { get; set; }

        [Column("dongia")]
        public decimal Dongia { get; set; }

        // Relationships (Optional but good for navigation)
        [ForeignKey("Madh")]
        public virtual TbDonHang? DonHang { get; set; }

        [ForeignKey("Mamon")]
        public virtual TbMonAn? MonAn { get; set; }
    }
}
