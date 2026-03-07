using AlMahaRental.Data;
using AlMahaRental.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System;
using System.IO;
using System.Linq;

namespace AlMahaRental.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public HomeController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // تم التعديل لإرسال الفروع
        public async Task<IActionResult> Index()
        {
            ViewBag.Locations = await _context.BranchLocations.ToListAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ContactUs(ContactMessage message)
        {
            ModelState.Remove("CreatedAt");
            ModelState.Remove("IsRead");

            if (ModelState.IsValid)
            {
                message.CreatedAt = DateTime.UtcNow;
                _context.ContactMessages.Add(message);

                // === إضافة إشعار: رسالة تواصل جديدة ===
                _context.SystemNotifications.Add(new SystemNotification
                {
                    Title = "رسالة تواصل جديدة",
                    Message = $"أرسل الزائر {message.Name} رسالة بخصوص: {message.Topic}",
                    Type = "Message",
                    LinkUrl = "/Admin/ContactMessages",
                    IsRead = false,
                    CreatedAt = DateTime.UtcNow
                });

                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "تم إرسال رسالتك بنجاح. سنتواصل معك قريباً!";
                return RedirectToAction(nameof(ContactUs));
            }
            return View(message);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Jobs(JobApplication application, IFormFile resumeFile)
        {
            ModelState.Remove("ResumeUrl");
            ModelState.Remove("CreatedAt");

            if (!application.HasExperience)
            {
                ModelState.Remove("ExperienceDetails");
                application.ExperienceDetails = "لا توجد خبرة سابقة";
            }

            if (ModelState.IsValid)
            {
                if (resumeFile != null && resumeFile.Length > 0)
                {
                    string uploadsFolder = Path.Combine(_env.WebRootPath, "uploads", "resumes");
                    if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(resumeFile.FileName);
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await resumeFile.CopyToAsync(fileStream);
                    }
                    application.ResumeUrl = "/uploads/resumes/" + uniqueFileName;
                }

                application.CreatedAt = DateTime.UtcNow;
                _context.JobApplications.Add(application);

                // === إضافة إشعار: طلب توظيف جديد ===
                _context.SystemNotifications.Add(new SystemNotification
                {
                    Title = "طلب توظيف جديد",
                    Message = $"قدم {application.FullName} طلباً للعمل في قسم {application.Department}",
                    Type = "Job",
                    LinkUrl = "/Admin/JobApplications",
                    IsRead = false,
                    CreatedAt = DateTime.UtcNow
                });

                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "تم إرسال طلب التوظيف بنجاح!";
                return RedirectToAction(nameof(Jobs));
            }
            return View(application);
        }

        public async Task<IActionResult> Fleet()
        {
            var cars = await _context.Cars.ToListAsync();
            return View(cars);
        }

        public async Task<IActionResult> Offers()
        {
            var offers = await _context.Cars.Where(c => c.IsSpecialOffer && c.IsAvailable).ToListAsync();
            return View(offers);
        }

        public IActionResult AboutUs() => View();
        public IActionResult ContactUs() => View();
        public IActionResult Jobs() => View();
        public IActionResult Services() => View();
        public IActionResult FAQ() => View();
        public async Task<IActionResult> OurLocations()
        {
            var locations = await _context.BranchLocations.ToListAsync();
            return View(locations);
        }
    }
}