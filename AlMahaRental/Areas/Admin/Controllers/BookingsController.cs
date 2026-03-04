using AlMahaRental.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace AlMahaRental.Areas.Admin.Controllers
{
    // كنترولر مخصص للإدارة لمتابعة وإدارة حجوزات العملاء
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class BookingsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BookingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1. عرض جميع الحجوزات
        public async Task<IActionResult> Index()
        {
            // جلب الحجوزات مع بيانات المستخدم والسيارة مرتبة من الأحدث للأقدم
            var bookings = await _context.Bookings
                .Include(b => b.User)
                .Include(b => b.Car)
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();

            return View(bookings);
        }

        // 2. تغيير حالة الحجز (مثلاً من Pending إلى Confirmed)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeStatus(int id, string newStatus)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }

            // التأكد من أن الحالة المرسلة صحيحة
            string[] validStatuses = { "Pending", "Confirmed", "Completed", "Cancelled" };
            if (validStatuses.Contains(newStatus))
            {
                booking.Status = newStatus;

                // تحديث حالة توفر السيارة إذا لزم الأمر
                // مثلاً إذا تم الإلغاء نعود ونجعل السيارة متاحة
                // (في نظام متطور يتم التحقق بناءً على التواريخ، هنا سنبسطها)

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}