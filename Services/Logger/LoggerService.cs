using System;
using System.IO;

namespace ShipFood.API.Services.Logger
{
    // Áp dụng Singleton Pattern: Chỉ có duy nhất một instance của LoggerService tồn tại.
    public class LoggerService : ILoggerService
    {
        // 1. Biến static private để giữ instance duy nhất.
        private static LoggerService? _instance;
        
        // 2. Một object để lock (khóa) đảm bảo thread-safety (an toàn đa luồng).
        private static readonly object _lock = new object();

        private readonly string _logFilePath;

        // 3. Constructor là PRIVATE để ngăn chặn việc tạo instance từ bên ngoài (new LoggerService()).
        private LoggerService()
        {
            // Tạo thư mục logs nếu chưa tồn tại
            string logDirectory = Path.Combine(Directory.GetCurrentDirectory(), "logs");
            if (!Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }
            _logFilePath = Path.Combine(logDirectory, $"log_{DateTime.Now:yyyyMMdd}.txt");
        }

        // 4. Method static public để truy cập instance duy nhất này.
        public static LoggerService Instance
        {
            get
            {
                // Double-check locking để tối ưu hiệu suất và đảm bảo thread-safety
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new LoggerService();
                        }
                    }
                }
                return _instance;
            }
        }

        public void LogInfo(string message)
        {
            WriteLog("INFO", message);
        }

        public void LogWarning(string message)
        {
            WriteLog("WARNING", message);
        }

        public void LogError(string message, Exception? ex = null)
        {
            string errorMessage = message;
            if (ex != null)
            {
                errorMessage += $" | Exception: {ex.Message}";
            }
            WriteLog("ERROR", errorMessage);
        }

        private void WriteLog(string level, string message)
        {
            try
            {
                string logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{level}] {message}";
                
                // Ghi ra console
                Console.WriteLine(logEntry);

                // Ghi vào file (append)
                lock (_lock) // Lock khi ghi file để tránh tranh chấp
                {
                    File.AppendAllText(_logFilePath, logEntry + Environment.NewLine);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to write log: {ex.Message}");
            }
        }
    }
}
