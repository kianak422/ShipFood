using ShipFood.API.Services.Logger;

namespace ShipFood.API.Services.Notification
{
    // Concrete Observer 2: Gửi SMS
    public class SmsNotification : INotificationObserver
    {
        private readonly ILoggerService _logger;

        public SmsNotification(ILoggerService logger)
        {
            _logger = logger;
        }

        public void Update(string message)
        {
            _logger.LogInfo($"[SMS Service] Sending SMS: {message}");
        }
    }
}
