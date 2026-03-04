using AlMahaRental.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace AlMahaRental.Areas.Admin.Controllers
{
    // كنترولر مخصص للإدارة لمتابعة المستخدمين المسجلين في النظام
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UsersController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        // عرض جميع المستخدمين
        public async Task<IActionResult> Index()
        {
            // جلب المستخدمين، نستثني الحساب الذي نقوم بتسجيل الدخول به حالياً لكي لا يحذف المدير نفسه بالخطأ
            var currentUserId = _userManager.GetUserId(User);
            var users = await _userManager.Users
                                          .Where(u => u.Id != currentUserId)
                                          .OrderByDescending(u => u.CreatedAt)
                                          .ToListAsync();
            return View(users);
        }

        // حذف مستخدم
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                // يمكننا إضافة منطق للتحقق إذا كان لدى المستخدم حجوزات قبل حذفه
                var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            return RedirectToAction(nameof(Index));
        }
    }
}