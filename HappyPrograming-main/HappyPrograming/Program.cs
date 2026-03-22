using HappyPrograming.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("Default");

// =====================
// 1. DB CONTEXT
// =====================
builder.Services.AddDbContext<HappyprogrammingContext>(options =>
    options.UseSqlServer(connectionString));

// =====================
// 2. AUTHENTICATION (QUAN TRỌNG)
// =====================
builder.Services.AddAuthentication("Cookies")
    .AddCookie("Cookies", options =>
    {
        options.LoginPath = "/Account/Login";           // khi chưa login
        options.AccessDeniedPath = "/Account/Denied";   // khi không có quyền
    });

// =====================
// 3. AUTHORIZATION
// =====================
builder.Services.AddAuthorization();

// =====================
// 4. MVC
// =====================
builder.Services.AddControllersWithViews();

var app = builder.Build();

// =====================
// PIPELINE
// =====================
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// 🔥 PHẢI ĐẶT ĐÚNG THỨ TỰ
app.UseAuthentication();   // ✅ thêm dòng này
app.UseAuthorization();

// =====================
// ROUTING
// =====================
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Admin}/{action=Dashboard}/{id?}");

app.Run();