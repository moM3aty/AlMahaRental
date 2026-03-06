using AlMahaRental.Data;
using AlMahaRental.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace AlMahaRental.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class BranchLocationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BranchLocationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // عرض قائمة الفروع
        public async Task<IActionResult> Index()
        {
            var locations = await _context.BranchLocations.ToListAsync();
            return View(locations);
        }

        // صفحة إضافة فرع جديد
        public IActionResult Create()
        {
            return View();
        }

        // حفظ الفرع الجديد
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BranchLocation location)
        {
            if (ModelState.IsValid)
            {
                _context.Add(location);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(location);
        }

        // حذف الفرع
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var location = await _context.BranchLocations.FindAsync(id);
            if (location != null)
            {
                _context.BranchLocations.Remove(location);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}