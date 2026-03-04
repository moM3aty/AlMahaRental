using AlMahaRental.Data;
using AlMahaRental.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. إعداد الاتصال بقاعدة البيانات (تأكد من تعديل الـ ConnectionString في appsettings.json)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// 2. إعداد الـ Identity وتفعيل الـ Roles (Admin, User)
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => {
    options.SignIn.RequireConfirmedAccount = false; // للتسهيل حاليا
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// 3. إضافة خدمات الـ MVC
builder.Services.AddControllersWithViews();

var app = builder.Build();

// إعداد الـ Pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles(); // للسماح باستخدام الـ CSS, JS, Images

app.UseRouting();

app.UseAuthentication(); // تفعيل تسجيل الدخول
app.UseAuthorization();  // تفعيل الصلاحيات (الـ Roles)

// إعداد الـ Routes الافتراضية ودعم الـ Areas لوحة التحكم (Admin Panel)
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();