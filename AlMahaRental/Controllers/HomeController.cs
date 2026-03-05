using AlMahaRental.Data;
using AlMahaRental.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;

namespace AlMahaRental.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        // إضافة الـ DbContext للكنترولر
        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AboutUs()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ContactUs()
        {
            return View();
        }

        // الأكشن الجديد لاستقبال الرسالة من الفورم وحفظها في الداتا بيز
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ContactUs(ContactMessage message)
        {
            if (ModelState.IsValid)
            {
                message.CreatedAt = DateTime.UtcNow;
                _context.ContactMessages.Add(message);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "تم إرسال رسالتك بنجاح. سنتواصل معك قريباً!";
                return RedirectToAction(nameof(ContactUs));
            }
            return View(message);
        }

        public IActionResult Services()
        {
            return View();
        }

        public IActionResult Fleet()
        {
            return View();
        }

        public IActionResult Offers()
        {
            return View();
        }

        public IActionResult Jobs()
        {
            return View();
        }

        public IActionResult FAQ()
        {
            return View();
        }

        public IActionResult OurLocations()
        {
            return View();
        }
    }
}