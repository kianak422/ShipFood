using Microsoft.EntityFrameworkCore;
using ShipFood.API.Data;
using ShipFood.API.Repositories;
using ShipFood.API.Services.Logger; // Singleton
using ShipFood.API.Services.Payment; // Factory
using ShipFood.API.Services.Pricing; // Strategy
using ShipFood.API.Services.Notification; // Observer
using ShipFood.API.Services.Order; // Command

var builder = WebApplication.CreateBuilder(args);

// Thêm các dịch vụ vào container (hộp chứa).
// 1. Singleton Logger (Ghi log - Dùng chung 1 bản sao duy nhất)
builder.Services.AddSingleton<ILoggerService>(_ => LoggerService.Instance);

// 2. Repository Pattern (Mẫu Kho chứa dữ liệu - Tách biệt xử lý dữ liệu)
// Sử dụng SQLite Database (tạo file shipfood.db)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=shipfood.db"));

// Đăng ký Kho chung (Generic) và Kho riêng (Specific)
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<ITbMonAnRepository, TbMonAnRepository>();

// 3. Factory Pattern (Mẫu Nhà máy - Tạo đối tượng thanh toán linh hoạt)
builder.Services.AddTransient<PaymentFactory>();

// 4. Strategy Pattern (Mẫu Chiến lược - Thay đổi cách tính giá linh hoạt)
builder.Services.AddScoped<PricingService>();

// 5. Observer Pattern (Mẫu Người quan sát - Gửi thông báo tự động)
builder.Services.AddSingleton<NotificationSubject>();
builder.Services.AddSingleton<EmailNotification>();
builder.Services.AddSingleton<SmsNotification>();

// 6. Command Pattern (Mẫu Mệnh lệnh - Đóng gói yêu cầu đặt hàng)
builder.Services.AddScoped<OrderInvoker>();

// 7. Facade Pattern (Mẫu Mặt tiền - Gom nhóm chức năng thống kê Admin)
builder.Services.AddScoped<ShipFood.API.Services.Facade.AdminDashboardFacade>();

// 8. Abstract Factory (Mẫu Siêu nhà máy - Tạo giao diện động)
builder.Services.AddScoped<ShipFood.API.Services.UI.IUIFactory, ShipFood.API.Services.UI.PrimaryUIFactory>();

// 9. Flyweight (Mẫu Hạng ruồi - Chia sẻ Icon dùng chung)
builder.Services.AddSingleton<ShipFood.API.Services.Flyweight.IconFlyweightFactory>();

// 10. Visitor (Mẫu Người viếng thăm - Mở rộng tính năng không sửa code cũ)
// Mẫu Visitor nay được nhúng trực tiếp nên ta không cần Dependency Injection cho Scoped của nó nữa
// Để lại comment cho biết ta đang dùng Visitor.

// Chuyển từ API sang MVC (Mô hình View - Controller)
builder.Services.AddControllersWithViews();
// Thêm bộ nhớ Cache
builder.Services.AddDistributedMemoryCache();
// Thêm hỗ trợ Session (Lưu phiên làm việc của user)
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Thêm hỗ trợ tài liệu API (Swagger)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Thêm HttpClient cho ChatController gọi Gemini API
builder.Services.AddHttpClient();

var app = builder.Build();

// Tự động tạo cơ sở dữ liệu để chạy thử (dễ dàng cho đồ án)
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        context.Database.EnsureCreated();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Có lỗi khi tạo cơ sở dữ liệu.");
    }
}

// Cấu hình luồng xử lý HTTP.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.DefaultModelsExpandDepth(-1);
    });
}

app.UseHttpsRedirection();
app.UseStaticFiles(); // Cho phép dùng file tĩnh (thư mục wwwroot: css, js, ảnh)

app.UseRouting();

app.UseSession(); // Bật tính năng Session

app.UseAuthorization();

app.MapControllerRoute(
    name: "MyArea",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Seed Data
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();
    // Auto migration if needed
    context.Database.EnsureCreated();
    ShipFood.API.Services.Data.DataSeeder.Seed(context);
}

app.Run();