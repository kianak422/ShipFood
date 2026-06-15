using Microsoft.AspNetCore.Mvc;
using ShipFood.API.Models;
using ShipFood.API.Repositories;
using ShipFood.API.Services.Order;
using ShipFood.API.Services.Payment;
using ShipFood.API.Services.Pricing;
using ShipFood.API.Services.Discount;
using ShipFood.API.Services.Logger;
using ShipFood.API.Services.Notification;
using Newtonsoft.Json;

namespace ShipFood.API.Controllers
{
    public class CartController(
        OrderInvoker orderInvoker,
        IRepository<TbDonHang> donHangRepo,
        IRepository<TbChiTietDonHang> chiTietRepo,
        ITbMonAnRepository monAnRepo,
        IRepository<TbTonKho> tonKhoRepo,
        PricingService pricingService,
        PaymentFactory paymentFactory,
        NotificationSubject notificationSubject,
        ILoggerService logger) : Controller
    {
        private readonly OrderInvoker _orderInvoker = orderInvoker;
        private readonly IRepository<TbDonHang> _donHangRepo = donHangRepo;
        private readonly IRepository<TbChiTietDonHang> _chiTietRepo = chiTietRepo;
        private readonly ITbMonAnRepository _monAnRepo = monAnRepo;
        private readonly IRepository<TbTonKho> _tonKhoRepo = tonKhoRepo;
        private readonly PricingService _pricingService = pricingService;
        private readonly PaymentFactory _paymentFactory = paymentFactory;
        private readonly NotificationSubject _notificationSubject = notificationSubject;
        private readonly ILoggerService _logger = logger;

        public IActionResult Index()
        {
            var cart = GetCartFromSession();
            // Tự động sửa lỗi Session cũ do JsonIgnore (SoLuong = 0 gây màn hình trắng xoá số lượng và lỗi tính tiền)
            bool needsHealing = false;
            foreach (var item in cart.MonAns)
            {
                if (item.SoLuong <= 0)
                {
                    item.SoLuong = 1;
                    needsHealing = true;
                }
            }
            if (needsHealing)
            {
                cart.TongTien = cart.MonAns.Sum(m => m.Giatien * m.SoLuong);
                SaveCartToSession(cart);
            }
            return View(cart);
        }

        public async Task<IActionResult> History()
        {
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("Login", "Home");
            }

            var allOrders = await _donHangRepo.GetAllAsync();
            // Lọc các đơn hàng của user hiện tại
            var userOrders = allOrders.Where(o => o.Ghichu != null && 
                (o.Ghichu.Contains($"[User:{username}]") || o.Ghichu.Contains(username)))
                .OrderByDescending(o => o.Ngaydathang)
                .ToList();

            return View(userOrders);
        }

        public async Task<IActionResult> ThemMonAn(int maMonAn, int soLuong = 1)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("Username")))
            {
                return RedirectToAction("Login", "Home");
            }

            // Dùng await để lấy dữ liệu bất đồng bộ
            var monAn = await _monAnRepo.GetByIdAsync(maMonAn);
            if (monAn != null)
            {
                var cart = GetCartFromSession();
                cart.ThemMon(monAn, soLuong);
                SaveCartToSession(cart);
            }
            return RedirectToAction("Index");
        }

        public IActionResult GiamSoLuong(int maMonAn)
        {
            var cart = GetCartFromSession();
            cart.GiamMon(maMonAn);
            SaveCartToSession(cart);
            return RedirectToAction("Index");
        }

        public IActionResult TangSoLuong(int maMonAn, int soLuong = 1)
        {
            // Tái sử dụng logic thêm món
            return RedirectToAction("ThemMonAn", new { maMonAn, soLuong });
        }

        public IActionResult XoaMon(int maMonAn)
        {
            var cart = GetCartFromSession();
            cart.XoaMon(maMonAn);
            SaveCartToSession(cart);
            return RedirectToAction("Index");
        }

        // GET: /cart/checkout (Hiển thị form thanh toán)
        [HttpGet]
        public IActionResult Checkout()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("Username")))
            {
                return RedirectToAction("Login", "Home");
            }
            
            var cart = GetCartFromSession();
            if (cart.MonAns.Count == 0)
            {
                return RedirectToAction("Index");
            }
            
            // Lấy thông tin có sẵn trong session để fill tự động
            ViewBag.Hoten = HttpContext.Session.GetString("Username");
            // Trong thực tế sẽ lookup DB lấy SDT, Email. Demo sẽ truyền qua ViewBag tĩnh
            
            return View(cart);
        }

        // AJAX POST: Lấy tiền giảm giá từ Coupon trực tiếp từ Client
        [HttpPost]
        public IActionResult GetDiscountAmount(string couponCode)
        {
            var cart = GetCartFromSession();
            if (cart == null || cart.MonAns.Count == 0 || string.IsNullOrEmpty(couponCode))
            {
                return Json(new { success = false, discount = 0, message = "Mã rỗng hoặc không hợp lệ" });
            }

            var couponVisitor = new ShipFood.API.Services.Visitor.CouponDiscountVisitor(couponCode);
            cart.Accept(couponVisitor);
            decimal totalDiscount = couponVisitor.TotalDiscount;

            if (totalDiscount > 0)
            {
                return Json(new { success = true, discount = totalDiscount, message = $"Áp dụng thẻ thành công! Giảm {totalDiscount:N0} đ" });
            }
            else
            {
                return Json(new { success = false, discount = 0, message = "Mã không hợp lệ hoặc không áp dụng cho món trong giỏ." });
            }
        }

        // POST: /cart/checkout (Xử lý đặt hàng thực tế)
        [HttpPost]
        public async Task<IActionResult> Checkout(string hoten, string diachi, string sdt, string couponCode)
        {
            _logger.LogInfo("Received Checkout request");

            var cart = GetCartFromSession();
            if (cart.MonAns.Count == 0)
            {
                return RedirectToAction("Index");
            }

            var username = HttpContext.Session.GetString("Username") ?? "Guest";

            // 1. Create basics
            var donHang = new TbDonHang
            {
                Ngaydathang = DateTime.Now,
                Trangthai = "Đã đặt",
                Ghichu = $"[User:{username}] Order for {hoten}, {diachi}, {sdt}"
            };

            // 2. Pricing Strategy
            decimal subTotal = cart.TongTien;

            // Apply Strategy (Happy Hour check could be here)
            _pricingService.SetStrategy(new HappyHourPricingStrategy(_logger));
            decimal priceAfterStrategy = _pricingService.CalculateFinalPrice(subTotal);

            // Bổ sung: Pattern Visitor (Nhập mã giảm giá) theo yêu cầu mới
            decimal couponDiscount = 0;
            if (!string.IsNullOrEmpty(couponCode))
            {
                var couponVisitor = new ShipFood.API.Services.Visitor.CouponDiscountVisitor(couponCode);
                cart.Accept(couponVisitor);
                couponDiscount = couponVisitor.TotalDiscount;
                
                _logger.LogInfo($"[Visitor] Applied Coupon {couponCode}. Discount: {couponDiscount:C}");
            }
            
            priceAfterStrategy -= couponDiscount;
            if (priceAfterStrategy < 0) priceAfterStrategy = 0;

            donHang.Tongtien = priceAfterStrategy;

            // 3. Decorator Pattern (Discount + Shipping)
            IOrderAmount orderAmount = new BaseOrderAmount();
            // Wrap with 10% Discount
            orderAmount = new TenPercentDiscountDecorator(orderAmount, _logger);
            // Wrap with Shipping Fee
            orderAmount = new ShippingFeeDecorator(orderAmount, 15000, _logger);

            // Calculate final total
            donHang.Tongtien = orderAmount.CalculateTotal(donHang);
            donHang.Phiship = 15000;

            // 4. Command Pattern (Place Order)
            var placeOrderCommand = new PlaceOrderCommand(donHang, cart.MonAns, _donHangRepo, _chiTietRepo, _monAnRepo, _tonKhoRepo, _notificationSubject, _logger);
            await _orderInvoker.ExecuteCommandAsync(placeOrderCommand);

            // 5. Update Ton Kho
            foreach (var item in cart.MonAns)
            {
                //Tăng số lượng bán ra
                var monAn = await _monAnRepo.GetByIdAsync(item.Mamon);
                if (monAn != null)
                {
                    monAn.Soluongban += item.SoLuong;
                    await _monAnRepo.UpdateAsync(monAn);
                }
                //T giảm số lượng tồn kho
                var tonKhoList = await _tonKhoRepo.FindAsync(t => t.Mamon == item.Mamon);
                var tonKho = tonKhoList.FirstOrDefault();
                if (tonKho != null)
                {
                    tonKho.SoLuongTon -= item.SoLuong;
                    if(tonKho.SoLuongTon < 0)
                    {
                        tonKho.SoLuongTon = 0;
                    }
                    await _tonKhoRepo.UpdateAsync(tonKho);  
                }
            }

            // Clear Cart
            HttpContext.Session.Remove("Cart");

            return RedirectToAction("Success", new { id = donHang.Madh });
        }

        public IActionResult Success(int id)
        {
            return View(id); // Pass Order ID to view
        }

        // POST: /cart/cancel/{id}
        [HttpPost]
        public async Task<IActionResult> Cancel(int id)
        {
            var cancelCommand = new CancelOrderCommand(id, _donHangRepo, _notificationSubject, _logger);
            await _orderInvoker.ExecuteCommandAsync(cancelCommand);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> OrderDetail(int id)
        {
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("Login", "Home");
            }

            var order = await _donHangRepo.GetByIdAsync(id);
            if (order == null || order.Ghichu == null || 
               (!order.Ghichu.Contains($"[User:{username}]") && !order.Ghichu.Contains(username)))
            {
                return NotFound("Đơn hàng không tồn tại hoặc bạn không có quyền xem.");
            }

            var chiTiets = (await _chiTietRepo.FindAsync(c => c.Madh == id)).ToList();
            
            foreach (var item in chiTiets)
            {
                item.MonAn = await _monAnRepo.GetByIdAsync(item.Mamon);
            }

            ViewBag.ChiTiets = chiTiets;
            return View(order);
        }

        // Các hàm hỗ trợ Session (Phiên làm việc)
        private Cart GetCartFromSession()
        {
            var cartJson = HttpContext.Session.GetString("Cart");
            if (string.IsNullOrEmpty(cartJson))
            {
                return new Cart();
            }
            return JsonConvert.DeserializeObject<Cart>(cartJson) ?? new Cart();
        }

        private void SaveCartToSession(Cart cart)
        {
            var cartJson = JsonConvert.SerializeObject(cart);
            HttpContext.Session.SetString("Cart", cartJson);
        }
    }
}
