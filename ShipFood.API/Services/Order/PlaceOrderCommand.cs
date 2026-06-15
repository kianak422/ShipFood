using ShipFood.API.Repositories;
using ShipFood.API.Services.Logger;
using ShipFood.API.Services.Notification;
using ShipFood.API.Models;

namespace ShipFood.API.Services.Order
{
    // Concrete Command: Đặt hàng
    public class PlaceOrderCommand(
        TbDonHang donHang,
        List<TbMonAn> cartItems,
        IRepository<TbDonHang> donHangRepository,
        IRepository<TbChiTietDonHang> chiTietRepository,
        NotificationSubject notificationSubject,
        ILoggerService logger) : IOrderCommand
    {
        private readonly TbDonHang _donHang = donHang;
        private readonly List<TbMonAn> _cartItems = cartItems;
        private readonly IRepository<TbDonHang> _donHangRepository = donHangRepository;
        private readonly IRepository<TbChiTietDonHang> _chiTietRepository = chiTietRepository;
        private readonly NotificationSubject _notificationSubject = notificationSubject;
        private readonly ILoggerService _logger = logger;

        public async Task ExecuteAsync()
        {
            _logger.LogInfo($"[Command] Executing PlaceOrderCommand for Order ID: {_donHang.Madh}");
            
            // 1. Lưu đơn hàng vào DB
            await _donHangRepository.AddAsync(_donHang);
            
            // 2. Lưu chi tiết đơn hàng
            foreach (var item in _cartItems)
            {
                var chiTiet = new TbChiTietDonHang
                {
                    Madh = _donHang.Madh,
                    Mamon = item.Mamon,
                    Soluong = item.SoLuong,
                    Dongia = item.Giatien
                };
                await _chiTietRepository.AddAsync(chiTiet);
            }

            // 3. Gửi thông báo
            _notificationSubject.Notify($"New order placed! ID: {_donHang.Madh}, Amount: {_donHang.Tongtien:C}");
        }
    }
}
