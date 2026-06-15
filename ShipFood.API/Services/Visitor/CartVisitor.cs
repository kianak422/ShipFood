using ShipFood.API.Models;

namespace ShipFood.API.Services.Visitor
{
    // The Visitor Interface
    public interface ICartVisitor
    {
        void Visit(TbMonAn item);
        // If we had a TbCombo class, we would add: void Visit(TbCombo combo);
    }

    // Concrete Visitor: Áp dụng Mã giảm giá (Coupon Discount) do người dùng chọn
    public class CouponDiscountVisitor(string couponCode) : ICartVisitor
    {
        private readonly string _couponCode = couponCode?.ToUpper().Trim() ?? "";
        public decimal TotalDiscount { get; private set; } = 0;

        public void Visit(TbMonAn item)
        {
            // Mã "MAMOI": Giảm 10% các món ăn Cơm/Salad (Madanhmuc = 1)
            if (_couponCode == "MAMOI" && item.Madanhmuc == 1)
            {
                TotalDiscount += (item.Giatien * 0.1m) * item.SoLuong;
            }

            // Mã "FREETRASUA": Miễn phí (trừ hết tiền) Trà/Đồ uống (Madanhmuc = 2) 
            else if (_couponCode == "FREETRASUA" && item.Madanhmuc == 2)
            {
                TotalDiscount += item.Giatien * item.SoLuong; 
            }
            
            // Mã "GIAM20K": Giảm giá 20K cho Đồ ăn Vặt/Nhanh (Madanhmuc = 3)
            else if (_couponCode == "GIAM20K" && item.Madanhmuc == 3)
            {
                TotalDiscount += 20000 * item.SoLuong;
            }
            
            // Giới hạn giảm giá để TotalDiscount không vượt quá giá món. Hàm này gọi mỗi món, nên an toàn nhất là tính xong mới check
            // Nếu khách nhập mã bậy bạ, Visitor sẽ không cộng thêm Discount nào.
        }
    }
}
