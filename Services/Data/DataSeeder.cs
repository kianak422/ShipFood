using ShipFood.API.Data;
using ShipFood.API.Models;

namespace ShipFood.API.Services.Data
{
    public static class DataSeeder
    {
        public static void Seed(AppDbContext context)
        {
            if (!context.TbDanhMuc.Any())
            {
                context.TbDanhMuc.AddRange(
                    new TbDanhMuc { Tendanhmuc = "Món Việt", Mota = "Hương vị truyền thống" },
                    new TbDanhMuc { Tendanhmuc = "Đồ ăn nhanh", Mota = "Nhanh gọn, tiện lợi" },
                    new TbDanhMuc { Tendanhmuc = "Đồ uống", Mota = "Giải khát" }
                );
                context.SaveChanges();
            }

            if (!context.TbMonAn.Any())
            {
                // Get Categories
                var monViet = context.TbDanhMuc.FirstOrDefault(c => c.Tendanhmuc == "Món Việt");
                var fastFood = context.TbDanhMuc.FirstOrDefault(c => c.Tendanhmuc == "Đồ ăn nhanh");

                context.TbMonAn.AddRange(
                    // Món Việt
                    new TbMonAn { Tenmon = "Phở Bò", Giatien = 45000, Mota = "Phở bò tái nạm gia truyền, nước dùng ngọt thanh", Madanhmuc = monViet?.Madanhmuc, Hinhanh = "/home/img/pho_bo.jpg" },
                    new TbMonAn { Tenmon = "Phở Gà", Giatien = 45000, Mota = "Phở gà ta da giòn thịt dai", Madanhmuc = monViet?.Madanhmuc, Hinhanh = "/home/img/pho_ga.jpg" },
                    new TbMonAn { Tenmon = "Bún Bò Huế", Giatien = 40000, Mota = "Bún bò Huế cay nồng chuẩn vị miền Trung", Madanhmuc = monViet?.Madanhmuc, Hinhanh = "/home/img/bun_bo_hue.jpg" },
                    new TbMonAn { Tenmon = "Cơm Chiên Dương Châu", Giatien = 55000, Mota = "Cơm chiên đầy ắp topping tôm thịt", Madanhmuc = monViet?.Madanhmuc, Hinhanh = "/home/img/com_chien_duong_chau.jpg" },
                    new TbMonAn { Tenmon = "Cơm Chiên Hải Sản", Giatien = 60000, Mota = "Cơm chiên hải sản tươi ngon", Madanhmuc = monViet?.Madanhmuc, Hinhanh = "/home/img/com_chien_hai_san.jpg" },
                    new TbMonAn { Tenmon = "Bánh Mì Ốp La", Giatien = 20000, Mota = "Bánh mì ốp la kèm pate xíu mại", Madanhmuc = monViet?.Madanhmuc, Hinhanh = "/home/img/banh_mi_op_la.jpg" },
                    new TbMonAn { Tenmon = "Bún Đậu Mắm Tôm", Giatien = 55000, Mota = "Mẹt bún đậu đầy đủ chả cốm nem rán", Madanhmuc = monViet?.Madanhmuc, Hinhanh = "/home/img/bundaugiadi.jpg" },
                    new TbMonAn { Tenmon = "Gà Hấp Hành", Giatien = 90000, Mota = "Gà ta hấp hành thơm lừng", Madanhmuc = monViet?.Madanhmuc, Hinhanh = "/home/img/ga_hap_hanh.jpg" },

                    // Đồ ăn nhanh
                    new TbMonAn { Tenmon = "Pizza Pepperoni", Giatien = 150000, Mota = "Pizza phô mai và xúc xích pepperoni", Madanhmuc = fastFood?.Madanhmuc, Hinhanh = "/home/img/pizza_pepperoni.jpg" },
                    new TbMonAn { Tenmon = "Pizza Hải Sản", Giatien = 160000, Mota = "Pizza hải sản tôm mực đầy ắp", Madanhmuc = fastFood?.Madanhmuc, Hinhanh = "/home/img/pizza.jpg" },
                    new TbMonAn { Tenmon = "Gà Rán Mật Ong", Giatien = 35000, Mota = "Gà rán giòn tan sốt mật ong Hàn Quốc", Madanhmuc = fastFood?.Madanhmuc, Hinhanh = "/home/img/ga_ran_mat_ong.jpg" },
                    new TbMonAn { Tenmon = "Khoai Tây Chiên", Giatien = 25000, Mota = "Khoai tây chiên giòn rụm", Madanhmuc = fastFood?.Madanhmuc, Hinhanh = "/home/img/khoai_tay_chien.jpg" },
                    new TbMonAn { Tenmon = "Bò Bít Tết", Giatien = 75000, Mota = "Bò bít tết sốt tiêu đen kèm khoai tây", Madanhmuc = fastFood?.Madanhmuc, Hinhanh = "/home/img/bo_bit_tet.jpg" },
                    new TbMonAn { Tenmon = "Gà Rán Cay", Giatien = 38000, Mota = "Gà rán sốt cay cấp độ 7", Madanhmuc = fastFood?.Madanhmuc, Hinhanh = "/home/img/ga_ran_mat_ong.jpg" },
                    new TbMonAn { Tenmon = "Tôm Chiên Xù", Giatien = 50000, Mota = "Tôm lăn bột chiên xù giòn tan", Madanhmuc = fastFood?.Madanhmuc, Hinhanh = "/home/img/tom_chien_xu.jpg" },

                    // Đồ uống
                    new TbMonAn { Tenmon = "Trà Sữa Thái Xanh", Giatien = 20000, Mota = "Trà sữa thái xanh đậm đà, full topping", Madanhmuc = context.TbDanhMuc.FirstOrDefault(c => c.Tendanhmuc == "Đồ uống")?.Madanhmuc, Hinhanh = "/home/img/ts_thai_xanh.jpg" },
                    new TbMonAn { Tenmon = "Cà Phê Sữa Đá", Giatien = 25000, Mota = "Cà phê Robusta đậm chất Việt Nam", Madanhmuc = context.TbDanhMuc.FirstOrDefault(c => c.Tendanhmuc == "Đồ uống")?.Madanhmuc, Hinhanh = "/home/img/ca_phe_sua.jpg" },
                    new TbMonAn { Tenmon = "Trà Đào Cam Sả", Giatien = 35000, Mota = "Trà đào thanh mát giải nhiệt", Madanhmuc = context.TbDanhMuc.FirstOrDefault(c => c.Tendanhmuc == "Đồ uống")?.Madanhmuc, Hinhanh = "/home/img/tra_trai_cay.jpg" },
                    new TbMonAn { Tenmon = "Sinh Tố Bơ", Giatien = 40000, Mota = "Sinh tố bơ sáp béo ngậy", Madanhmuc = context.TbDanhMuc.FirstOrDefault(c => c.Tendanhmuc == "Đồ uống")?.Madanhmuc, Hinhanh = "/home/img/sinh_to_bo.jpg" },
                    new TbMonAn { Tenmon = "Trà Sen Vàng", Giatien = 35000, Mota = "Trà sen vàng macchiato béo ngậy", Madanhmuc = context.TbDanhMuc.FirstOrDefault(c => c.Tendanhmuc == "Đồ uống")?.Madanhmuc, Hinhanh = "/home/img/tra_sen.jpg" },
                    new TbMonAn { Tenmon = "Sinh Tố Dâu", Giatien = 35000, Mota = "Sinh tố dâu Đà Lạt chua ngọt", Madanhmuc = context.TbDanhMuc.FirstOrDefault(c => c.Tendanhmuc == "Đồ uống")?.Madanhmuc, Hinhanh = "/home/img/sinh_to_dau.jpg" }
                );
                context.SaveChanges();
                context.SaveChanges();
            }

            // Seed Users if empty
            if (!context.TbUser.Any())
            {
                context.TbUser.AddRange(
                    new TbUser { Username = "admin", Pwd = "123", Loaitaikhoan = "Admin", Email = "admin@fastship.com", Sdt = "0909000000", Trangthai = 1 },
                    new TbUser { Username = "shipper1", Pwd = "123", Loaitaikhoan = "Shipper", Email = "ship1@fastship.com", Sdt = "0909111222", Trangthai = 1 },
                    new TbUser { Username = "khach1", Pwd = "123", Loaitaikhoan = "KhachHang", Email = "khach1@fastship.com", Sdt = "0909333444", Trangthai = 1 },
                    new TbUser { Username = "restaurant1", Pwd = "123", Loaitaikhoan = "Restaurant", Email = "quan1@fastship.com", Sdt = "0909555666", Trangthai = 1 }
                );
                context.SaveChanges();
            }

            if (!context.TbTonKho.Any())
            {
               foreach (var mon in context.TbMonAn)
                {            
                    context.TbTonKho.Add(new TbTonKho
                    {
                    Mamon = mon.Mamon,
                    SoLuongTon = 100,
                    SoLuongNhap = 100,
                    NgayCapNhat = DateTime.Now
                    });
                }

            context.SaveChanges();
            }
        }
    }
}
