using ShipFood.API.Services.Logger;

namespace ShipFood.API.Services.Notification
{
    // Concrete Observer 1: Gửi Email
    public class EmailNotification : INotificationObserver
    {
        private readonly ILoggerService _logger;

        public EmailNotification(ILoggerService logger)
        {
            _logger = logger;
        }

        public void Update(string message)
        {
            _logger.LogInfo($"[Email Service] Sending email: {message}");
        }
    }
}
