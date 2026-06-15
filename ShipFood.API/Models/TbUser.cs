using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShipFood.API.Models
{
    [Table("tbUser")]
    public class TbUser
    {
        [Key]
        [Column("userid")]
        public int Userid { get; set; }

        [Column("username")]
        public string? Username { get; set; }

        [Column("pwd")]
        public string? Pwd { get; set; }

        [Column("loaitaikhoan")]
        public string? Loaitaikhoan { get; set; }

        [Column("sdt")]
        public string? Sdt { get; set; }

        [Column("vitien")]
        public decimal? Vitien { get; set; }

        [Column("email")]
        public string? Email { get; set; }

        [Column("trangthai")]
        public int Trangthai { get; set; }
    }
}
