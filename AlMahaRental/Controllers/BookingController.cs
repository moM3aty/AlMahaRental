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
    [Authorize]
    public class BookingController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public BookingController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // 1. عرض صفحة الحجز
        [AllowAnonymous]
        public async Task<IActionResult> Reserve(int? id)
        {
            ViewBag.Locations = await _context.BranchLocations.ToListAsync();
            if (id == null) return NotFound();

            var car = await _context.Cars.FirstOrDefaultAsync(m => m.Id == id);
            if (car == null || !car.IsAvailable) return NotFound("السيارة غير متاحة.");

            var booking = new Booking { CarId = car.Id, Car = car };
            return View(booking);
        }

        // 2. إتمام الحجز (حل مشكلة SQL IDENTITY_INSERT)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reserve(Booking booking)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            ModelState.Remove("User");
            ModelState.Remove("UserId");
            ModelState.Remove("Car");
            ModelState.Remove("BookingReference");
            ModelState.Remove("Status");
            ModelState.Remove("CreatedAt");

            if (ModelState.IsValid)
            {
                var car = await _context.Cars.FindAsync(booking.CarId);

                // الحل هنا: تصفير الـ Id لكي لا يحدث خطأ SQL
                booking.Id = 0;
                booking.UserId = user.Id;
                booking.BookingReference = Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper();
                booking.Status = "Pending";
                booking.CreatedAt = DateTime.UtcNow;

                var days = (booking.ReturnDate - booking.PickupDate).Days;
                booking.TotalPrice = car.DailyPrice * (days < 1 ? 1 : days);

                _context.Bookings.Add(booking);

                // إضافة إشعار للإدارة
                _context.SystemNotifications.Add(new SystemNotification
                {
                    Title = "حجز جديد",
                    Message = $"حجز جديد للسيارة {car.Name} من {user.FirstName}",
                    Type = "Booking",
                    LinkUrl = "/Admin/Bookings",
                    CreatedAt = DateTime.UtcNow
                });

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Confirmation), new { refId = booking.BookingReference });
            }
            ViewBag.Locations = await _context.BranchLocations.ToListAsync();
            return View(booking);
        }

        // 3. الأكشن المفقود الذي سبب خطأ 404 (إدارة الحجز)
        [AllowAnonymous]
        public async Task<IActionResult> Manage(string bookingRef, string idNumber)
        {
            if (string.IsNullOrEmpty(bookingRef) || string.IsNullOrEmpty(idNumber))
            {
                TempData["ErrorMessage"] = "يرجى إدخال البيانات المطلوبة.";
                return RedirectToAction("Index", "Home");
            }

            var booking = await _context.Bookings
                .Include(b => b.Car)
                .Include(b => b.User)
                .FirstOrDefaultAsync(b => b.BookingReference == bookingRef && b.User.IdNumber == idNumber);

            if (booking == null)
            {
                TempData["ErrorMessage"] = "لم يتم العثور على الحجز، تأكد من صحة البيانات.";
                return RedirectToAction("Index", "Home");
            }

            return View(booking);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Confirmation(string refId)
        {
            var user = await _userManager.GetUserAsync(User);
            var booking = await _context.Bookings.Include(b => b.Car)
                .FirstOrDefaultAsync(b => b.BookingReference == refId && b.UserId == user.Id);
            return View(booking);
        }
    }
}