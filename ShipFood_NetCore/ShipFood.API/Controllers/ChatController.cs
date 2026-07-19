using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace ShipFood.API.Controllers
{
    public class ChatController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public ChatController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage([FromBody] ChatRequest request)
        {
            if (string.IsNullOrEmpty(request?.Message))
                return BadRequest("Tin nhắn không được để trống.");

            var apiKey = _configuration["GeminiApiKey"];
            var url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent?key={apiKey}";

            // Add new message to history for API call
            var apiHistory = request.History ?? new List<GeminiMessage>();
            apiHistory.Add(new GeminiMessage { role = "user", parts = new List<GeminiPart> { new GeminiPart { text = request.Message } } });

            var body = new
            {
                system_instruction = new
                {
                    parts = new[] { new { text = "Bạn là trợ lý AI của Fastship - nền tảng đặt hàng và giao hàng thức ăn. Trả lời ngắn gọn, thân thiện bằng tiếng Việt. Địa chỉ: 48 Cao Thắng, Hải Châu, Đà Nẵng. Hotline: 0123 456 789." } }
                },
                contents = apiHistory
            };

            var client = _httpClientFactory.CreateClient();
            var json = JsonSerializer.Serialize(body);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(url, content);
            var responseStr = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                var errorMsg = $"Lỗi API Gemini: {response.StatusCode} - {responseStr}";
                return StatusCode((int)response.StatusCode, errorMsg);
            }

            using var doc = JsonDocument.Parse(responseStr);
            var reply = doc.RootElement
                .GetProperty("candidates")[0]
                .GetProperty("content")
                .GetProperty("parts")[0]
                .GetProperty("text")
                .GetString();

            return Json(new { reply });
        }
    }

    public class ChatRequest
    {
        public string Message { get; set; }
        public List<GeminiMessage> History { get; set; }
    }

    public class GeminiMessage
    {
        public string role { get; set; }
        public List<GeminiPart> parts { get; set; }
    }

    public class GeminiPart
    {
        public string text { get; set; }
    }
}