using Microsoft.AspNetCore.Mvc;

namespace AlMahaRental.Controllers
{
    // هذا الكنترولر يتحكم بالصفحات العامة التي يراها الزائر (الموقع الأساسي)
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AboutUs()
        {
            return View();
        }

        public IActionResult ContactUs()
        {
            return View();
        }

        public IActionResult Services()
        {
            return View();
        }

        public IActionResult Fleet()
        {
            // في الدفعة القادمة، سنقوم بجلب السيارات من قاعدة البيانات وتمريرها هنا
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
    }
}