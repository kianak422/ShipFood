using ShipFood.API.Repositories;
using ShipFood.API.Services.Logger;
using ShipFood.API.Services.Notification;
using ShipFood.API.Models;

namespace ShipFood.API.Services.Order
{
    // Concrete Command: Hủy đơn hàng
    public class CancelOrderCommand : IOrderCommand
    {
        private readonly int _orderId;
        private readonly IRepository<TbDonHang> _donHangRepository;
        private readonly NotificationSubject _notificationSubject;
        private readonly ILoggerService _logger;

        public CancelOrderCommand(
            int orderId,
            IRepository<TbDonHang> donHangRepository,
            NotificationSubject notificationSubject,
            ILoggerService logger)
        {
            _orderId = orderId;
            _donHangRepository = donHangRepository;
            _notificationSubject = notificationSubject;
            _logger = logger;
        }

        public async Task ExecuteAsync()
        {
            _logger.LogInfo($"[Command] Executing CancelOrderCommand for Order ID: {_orderId}");
            
            var order = await _donHangRepository.GetByIdAsync(_orderId);
            if (order != null)
            {
                if (order.Trangthai != "Đã đặt")
                {
                    _logger.LogWarning($"Order {_orderId} cannot be cancelled because its status is {order.Trangthai}.");
                    return;
                }
                
                order.Trangthai = "Đã hủy";
                await _donHangRepository.UpdateAsync(order);
                _logger.LogInfo($"Order {_orderId} has been cancelled.");
                _notificationSubject.Notify($"Order {_orderId} cancelled.");
            }
            else
            {
                _logger.LogWarning($"Order {_orderId} not found to cancel.");
            }
        }
    }
}
