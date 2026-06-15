using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShipFood.API.Models
{
    [Table("tbKhachHang")]
    public class TbKhachHang
    {
        [Key]
        [Column("userid")]
        [ForeignKey("TbUser")]
        public int Userid { get; set; }

        [Column("tenkh")]
        public string? Tenkh { get; set; }

        public virtual TbUser? TbUser { get; set; }
    }
}
