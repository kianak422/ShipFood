using System.ComponentModel.DataAnnotations;

namespace ShipFood.API.Models
{
    public class TbVoucher
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Code { get; set; } = string.Empty;

        public decimal DiscountValue { get; set; }

        // Thêm dòng này
        public bool IsPercent { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public bool IsActive { get; set; } = true;
    }
}