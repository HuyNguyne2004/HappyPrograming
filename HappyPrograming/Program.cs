using HappyPrograming.Models;
using HappyPrograming.Repository;
using HappyPrograming.Repository.Interface;
using HappyPrograming.Service;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. Connection String
var connectionString = builder.Configuration.GetConnectionString("Default");

// 2. DbContext
builder.Services.AddDbContext<HappyprogrammingContext>(options =>
    options.UseSqlServer(connectionString));

// 3. Đăng ký Cấu hình Session (Để lưu OTP tạm thời)
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(5); // OTP hết hạn sau 5 phút
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// 4. Add MVC
builder.Services.AddControllersWithViews();

// 5. Đăng ký Dependency Injection (DI)
// Repository
builder.Services.AddScoped<MenteeRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Services
builder.Services.AddScoped<MenteeService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<OTPService>();       // Đã có
builder.Services.AddScoped<SendMailService>(); // Đã có

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// --- QUAN TRỌNG: Thêm dòng này để dùng được Session ---
app.UseSession();

app.UseAuthorization();

// Route mặc định: Chạy thẳng vào trang Login để test
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=SignIn}/{action=Index}/{id?}");

app.Run();