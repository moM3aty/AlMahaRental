using AlMahaRental.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace AlMahaRental.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class JobApplicationsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public JobApplicationsController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // عرض جميع طلبات التوظيف
        public async Task<IActionResult> Index()
        {
            var applications = await _context.JobApplications
                                             .OrderByDescending(a => a.CreatedAt)
                                             .ToListAsync();
            return View(applications);
        }

        // حذف طلب توظيف
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var application = await _context.JobApplications.FindAsync(id);
            if (application != null)
            {
                // حذف ملف الـ PDF من السيرفر إذا كان موجوداً
                if (!string.IsNullOrEmpty(application.ResumeUrl))
                {
                    string filePath = Path.Combine(_webHostEnvironment.WebRootPath, application.ResumeUrl.TrimStart('/'));
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }

                _context.JobApplications.Remove(application);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}