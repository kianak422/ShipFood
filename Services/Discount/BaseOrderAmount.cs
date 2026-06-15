using ShipFood.API.Models;

namespace ShipFood.API.Services.Discount
{
    public class BaseOrderAmount : IOrderAmount
    {
        public decimal CalculateTotal(TbDonHang dh)
        {
            return dh.Tongtien;
        }
    }
}
