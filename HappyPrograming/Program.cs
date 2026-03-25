using HappyPrograming.Models;
using HappyPrograming.Repository;
using HappyPrograming.Service;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("Default");

// 2. Đăng ký DbContext vào Dependency Injection (DI) container
builder.Services.AddDbContext<HappyprogrammingContext>(options =>
    options.UseSqlServer(connectionString));

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<MenteeRepository>();
builder.Services.AddScoped<MenteeService>();
builder.Services.AddScoped<MentorRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Mentor}/{action=Profile}/{id=1}");

app.Run();
