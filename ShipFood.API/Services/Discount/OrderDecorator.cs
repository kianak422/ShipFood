using ShipFood.API.Models;

namespace ShipFood.API.Services.Discount
{
    public abstract class OrderDecorator : IOrderAmount
    {
        protected readonly IOrderAmount _orderAmount;

        public OrderDecorator(IOrderAmount orderAmount)
        {
            _orderAmount = orderAmount;
        }

        public virtual decimal CalculateTotal(TbDonHang dh)
        {
            return _orderAmount.CalculateTotal(dh);
        }
    }
}
