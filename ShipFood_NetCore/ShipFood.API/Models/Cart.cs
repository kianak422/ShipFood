using System.Text.Json.Serialization;

namespace ShipFood.API.Models
{
    // Class Cart quản lý giỏ hàng
    // Tương ứng với Cart.java
    public class Cart
    {
        public int? Userid { get; set; }
        public decimal TongTien { get; set; }
        public int? Maquanan { get; set; }
        public int? MaKM { get; set; }
        public List<TbMonAn> MonAns { get; set; }
        
        // Constructor
        public Cart()
        {
            MonAns = [];
            TongTien = 0;
        }

        // Thêm món ăn
        public void ThemMon(TbMonAn monAn, int soLuong)
        {
            // Check existent
            var item = MonAns.FirstOrDefault(m => m.Mamon == monAn.Mamon);
            if (item != null)
            {
                item.SoLuong += soLuong;
                decimal themTien = monAn.Giatien * soLuong;
                TongTien += themTien;
            }
            else
            {
                monAn.SoLuong = soLuong;
                MonAns.Add(monAn);
                decimal themTien = monAn.Giatien * soLuong;
                TongTien += themTien;
            }
        }

        // Xóa món
        public void XoaMon(int maMon)
        {
            var item = MonAns.FirstOrDefault(m => m.Mamon == maMon);
            if (item != null)
            {
                decimal giamTien = item.Giatien * item.SoLuong;
                TongTien -= giamTien;
                MonAns.Remove(item);
            }
        }

        // Giảm món
        public void GiamMon(int maMon)
        {
            var item = MonAns.FirstOrDefault(m => m.Mamon == maMon);
            if (item != null)
            {
                if (item.SoLuong <= 1)
                {
                    TongTien -= item.Giatien;
                    MonAns.Remove(item);
                }
                else
                {
                    item.SoLuong -= 1;
                    TongTien -= item.Giatien;
                }
            }
        }

        // Mẫu Visitor: Duyệt qua tất cả các món trong giỏ để "Khách" thăm viếng
        public void Accept(Services.Visitor.ICartVisitor visitor)
        {
            foreach (var item in MonAns)
            {
                // Mỗi món ăn sẽ có thể tự số lượng n lần, ta cần Accept n lần hoặc Visitor tự xử lý SoLuong.
                // Ở đây ta gọi n lần (mô phỏng từng item rời rạc) hoặc truyền nguyên item.
                // Vì TbMonAn có thuộc tính SoLuong, Visitor có thể tự nhân lên.
                visitor.Visit(item);
            }
        }
    }
}
