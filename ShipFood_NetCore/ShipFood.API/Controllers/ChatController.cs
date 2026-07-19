using Microsoft.AspNetCore.Mvc;

namespace ShipFood.API.Controllers
{
    public class ChatController : Controller
    {
        private static readonly Dictionary<string, string[]> _responses = new Dictionary<string, string[]>
        {
            ["xin chào"] = new[] { "Xin chào! 👋 Tôi là trợ lý của Fastship. Tôi có thể giúp gì cho bạn?", "Chào bạn! Rất vui được hỗ trợ bạn hôm nay.", "Hello! Bạn cần tìm món ăn hay đặt hàng ạ?" },
            ["chào"] = new[] { "Xin chào! 👋 Tôi có thể giúp gì cho bạn?", "Chào bạn! Fastship sẵn sàng phục vụ.", "Hi! Bạn muốn tìm món gì?" },
            ["menu"] = new[] { "Menu của chúng tôi có nhiều món ngon: Cơm, Phở, Bún, Đồ uống, và các set ăn. Bạn muốn xem món nào?", "Chúng tôi có đầy đủ món từ cơm, phở, bún đến đồ uống. Bạn muốn đặt món gì?", "Menu đa dạng với nhiều món Việt Nam. Bạn muốn tìm món gì?" },
            ["đặt hàng"] = new[] { "Bạn có thể đặt hàng trực tiếp trên website hoặc gọi hotline 0123 456 789 để được hỗ trợ.", "Để đặt hàng, hãy chọn món bạn thích và thêm vào giỏ hàng nhé!", "Quy trình đặt hàng rất đơn giản: Chọn món → Thêm giỏ → Thanh toán." },
            ["giá"] = new[] { "Giá các món từ 25.000đ - 150.000đ tùy loại. Bạn muốn xem món nào?", "Chúng tôi có nhiều mức giá phù hợp với mọi nhu cầu. Bạn đang quan tâm món nào?", "Giá rất hợp lý! Món cơm từ 25k, món đặc biệt từ 80k." },
            ["giao hàng"] = new[] { "Chúng tôi giao hàng trong khu vực Đà Nẵng, thời gian 30-45 phút.", "Shipper của Fastship rất nhanh chóng! Bạn ở khu vực nào?", "Miễn phí ship cho đơn hàng từ 150k trở lên." },
            ["thanh toán"] = new[] { "Bạn có thể thanh toán bằng tiền mặt, chuyển khoản hoặc ví điện tử.", "Hỗ trợ nhiều hình thức thanh toán: Tiền mặt, Banking, Momo, ZaloPay.", "Thanh toán linh hoạt và an toàn." },
            ["địa chỉ"] = new[] { "Địa chỉ: 48 Cao Thắng, Hải Châu, Đà Nẵng", "Cửa hàng tại 48 Cao Thắng, Hải Châu, Đà Nẵng. Bạn ghé nhé!", "48 Cao Thắng, Hải Châu, Đà Nẵng - Rất dễ tìm!" },
            ["hotline"] = new[] { "Hotline: 0123 456 789", "Gọi ngay 0123 456 789 để được hỗ trợ nhanh nhất!", "Hotline 0123 456 789 luôn sẵn sàng phục vụ." },
            ["khuyến mãi"] = new[] { "Hiện tại có giảm giá 10% cho đơn hàng đầu tiên!", "Nhiều voucher hấp dẫn đang chờ bạn. Đơn hàng trên 150k miễn phí ship!", "Khuyến mãi: Giảm 10% đơn đầu, Free ship đơn > 150k." },
            ["món"] = new[] { "Chúng tôi có nhiều món ngon: Cơm tấm, Phở bò, Bún chả, và nhiều món khác.", "Menu phong phú với món Việt Nam truyền thống.", "Bạn muốn ăn món gì? Tôi sẽ gợi ý cho bạn!" },
            ["cảm ơn"] = new[] { "Rất vui được giúp đỡ bạn! Cần gì cứ nhắn nhé.", "Không có chi! Chúc bạn ăn ngon miệng!", "Cảm ơn bạn đã tin tưởng Fastship!" },
            ["tạm biệt"] = new[] { "Tạm biệt! Hẹn gặp lại bạn nhé! 👋", "Bye bye! Chúc bạn một ngày tốt lành!", "Hẹn gặp lại! Fastship luôn sẵn sàng phục vụ." }
        };

        [HttpPost]
        public IActionResult SendMessage([FromBody] ChatRequest request)
        {
            if (string.IsNullOrEmpty(request?.Message))
                return BadRequest("Tin nhắn không được để trống.");

            var message = request.Message.ToLower().Trim();
            string reply = GetResponse(message);

            return Json(new { reply });
        }

        private string GetResponse(string message)
        {
            // Check for keywords
            foreach (var keyword in _responses.Keys)
            {
                if (message.Contains(keyword))
                {
                    var responses = _responses[keyword];
                    var random = new Random();
                    return responses[random.Next(responses.Length)];
                }
            }

            // Default responses for unknown messages
            var defaultResponses = new[]
            {
                "Xin lỗi, tôi chưa hiểu rõ. Bạn có thể hỏi về menu, đặt hàng, giá cả, hoặc giao hàng không?",
                "Tôi có thể giúp bạn tìm món ăn, đặt hàng, hoặc tư vấn. Bạn cần hỗ trợ gì?",
                "Vui lòng hỏi về	menu, giá, đặt hàng, hoặc giao hàng để tôi hỗ trợ tốt hơn nhé!",
                "Bạn có thể thử hỏi: 'Menu', 'Đặt hàng', 'Giá', 'Giao hàng', hoặc 'Khuyến mãi'?"
            };

            var randomDefault = new Random();
            return defaultResponses[randomDefault.Next(defaultResponses.Length)];
        }
    }

    public class ChatRequest
    {
        public string Message { get; set; }
    }
}