using AlMahaRental.Data;
using AlMahaRental.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AlMahaRental.Controllers
{
    // هذا الكنترولر مسؤول عن عمليات الحجز الخاصة بالعملاء
    [Authorize] // يجب أن يكون المستخدم مسجلاً للدخول ليتمكن من الحجز
    public class BookingController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public BookingController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // 1. عرض صفحة الحجز لسيارة محددة
        [AllowAnonymous] // نسمح للزائر برؤية الصفحة، ولكن عند الضغط على تأكيد سيطلب منه تسجيل الدخول
        public async Task<IActionResult> Reserve(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var car = await _context.Cars.FirstOrDefaultAsync(m => m.Id == id);
            if (car == null || !car.IsAvailable)
            {
                return NotFound("السيارة غير موجودة أو غير متاحة حالياً.");
            }

            var booking = new Booking
            {
                CarId = car.Id,
                Car = car,
                PickupDate = DateTime.Today.AddDays(1),
                ReturnDate = DateTime.Today.AddDays(3)
            };

            return View(booking);
        }

        // 2. استلام بيانات الحجز من العميل وحفظها
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reserve(Booking booking)
        {
            // استخراج بيانات المستخدم الحالي
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Challenge(); // توجيه لصفحة تسجيل الدخول إذا لم يكن مسجلاً
            }

            // إزالة التحقق من الخصائص المرتبطة لتجنب أخطاء الـ ModelState
            ModelState.Remove("User");
            ModelState.Remove("Car");
            ModelState.Remove("BookingReference");
            ModelState.Remove("Status");

            if (ModelState.IsValid)
            {
                var car = await _context.Cars.FindAsync(booking.CarId);
                if (car == null || !car.IsAvailable)
                {
                    ModelState.AddModelError("", "عذراً، هذه السيارة غير متاحة للحجز حالياً.");
                    booking.Car = car;
                    return View(booking);
                }

                // حساب عدد الأيام
                var days = (booking.ReturnDate - booking.PickupDate).Days;
                if (days < 1) days = 1; // الحد الأدنى يوم واحد

                // إعداد بيانات الحجز النهائية
                booking.UserId = user.Id;
                booking.TotalPrice = car.DailyPrice * days;
                booking.Status = "Pending"; // حالة الحجز المبدئية: قيد الانتظار
                booking.BookingReference = Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper(); // رقم مرجعي عشوائي
                booking.CreatedAt = DateTime.UtcNow;

                _context.Bookings.Add(booking);
                await _context.SaveChangesAsync();

                // التوجيه إلى صفحة تأكيد الحجز
                return RedirectToAction(nameof(Confirmation), new { refId = booking.BookingReference });
            }

            // في حال وجود خطأ، نعيد عرض الصفحة مع بيانات السيارة
            booking.Car = await _context.Cars.FindAsync(booking.CarId);
            return View(booking);
        }

        // 3. صفحة تأكيد الحجز
        public async Task<IActionResult> Confirmation(string refId)
        {
            if (string.IsNullOrEmpty(refId))
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            var booking = await _context.Bookings
                .Include(b => b.Car)
                .FirstOrDefaultAsync(b => b.BookingReference == refId && b.UserId == user.Id);

            if (booking == null)
            {
                return NotFound("لم يتم العثور على الحجز.");
            }

            return View(booking);
        }

        // 4. بحث سريع من الصفحة الرئيسية (اختياري للربط مع فورم الرئيسية)
        [AllowAnonymous]
        public IActionResult Search(string PickupLocation, string DropoffLocation, DateTime PickupDate, DateTime ReturnDate)
        {
            // يمكنك هنا تخزين هذه البيانات في Session وتوجيه المستخدم لصفحة الأسطول (Fleet)
            // ليختار السيارة، ثم تمرير هذه التواريخ لصفحة الحجز.
            // للتبسيط، سنوجهه لصفحة الأسطول مباشرة
            return RedirectToAction("Fleet", "Home");
        }
    }
}