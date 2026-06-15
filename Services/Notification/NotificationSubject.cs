using ShipFood.API.Services.Logger;

namespace ShipFood.API.Services.Notification
{
    // Subject (Publisher): Nơi phát đi thông báo
    public class NotificationSubject
    {
        private readonly List<INotificationObserver> _observers = new List<INotificationObserver>();
        private readonly ILoggerService _logger;

        public NotificationSubject(ILoggerService logger)
        {
            _logger = logger;
        }

        public void Attach(INotificationObserver observer)
        {
            _observers.Add(observer);
            _logger.LogInfo($"[NotificationList] Attached observer: {observer.GetType().Name}");
        }

        public void Detach(INotificationObserver observer)
        {
            _observers.Remove(observer);
            _logger.LogInfo($"[NotificationList] Detached observer: {observer.GetType().Name}");
        }

        // Khi có sự kiện (ví dụ: Đơn hàng mới), hàm này sẽ báo cho tất cả Observers
        public void Notify(string message)
        {
            _logger.LogInfo("[NotificationList] Notifying all observers...");
            foreach (var observer in _observers)
            {
                observer.Update(message);
            }
        }
    }
}
