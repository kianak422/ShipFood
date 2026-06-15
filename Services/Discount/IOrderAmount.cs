using ShipFood.API.Models;

namespace ShipFood.API.Services.Discount
{
    public interface IOrderAmount
    {
        decimal CalculateTotal(TbDonHang dh);
    }
}
