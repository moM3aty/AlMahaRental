using AlMahaRental.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AlMahaRental.Areas.Admin.Controllers
{
    // حماية لوحة التحكم لتكون متاحة فقط لمن يمتلك صلاحية Admin
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // جلب إحصائيات سريعة لعرضها في لوحة القيادة
            ViewBag.TotalCars = await _context.Cars.CountAsync();
            ViewBag.ActiveBookings = await _context.Bookings.CountAsync(b => b.Status == "Confirmed" || b.Status == "Pending");
            ViewBag.TotalUsers = await _context.Users.CountAsync();

            // حساب الأرباح التقريبية (للحجوزات المكتملة والمؤكدة)
            ViewBag.TotalRevenue = await _context.Bookings
                .Where(b => b.Status == "Confirmed" || b.Status == "Completed")
                .SumAsync(b => b.TotalPrice);

            return View();
        }
    }
}