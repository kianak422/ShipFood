using ShipFood.API.Services.Logger;

namespace ShipFood.API.Services.Order
{
    // Invoker: Người điều phối các lệnh
    public class OrderInvoker
    {
        private readonly ILoggerService _logger;

        public OrderInvoker(ILoggerService logger)
        {
            _logger = logger;
        }

        public async Task ExecuteCommandAsync(IOrderCommand command)
        {
            _logger.LogInfo($"[Invoker] Received command: {command.GetType().Name}");
            await command.ExecuteAsync();
        }
    }
}
