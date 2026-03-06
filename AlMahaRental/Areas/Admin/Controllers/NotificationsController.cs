using AlMahaRental.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace AlMahaRental.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class NotificationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public NotificationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var notifications = await _context.SystemNotifications
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();

            return View(notifications);
        }

        // --- الميزة المطلوبة: التحديد كمقروء عند النقر والتحويل للرابط المطلوب ---
        public async Task<IActionResult> MarkAsReadAndRedirect(int id)
        {
            var notification = await _context.SystemNotifications.FindAsync(id);

            if (notification != null)
            {
                // تحويل الحالة إلى مقروء لكي ينقص الرقم من الجرس
                notification.IsRead = true;
                await _context.SaveChangesAsync();

                // التوجه إلى الصفحة المقصودة (مثلاً صفحة الحجوزات أو الرسائل)
                if (!string.IsNullOrEmpty(notification.LinkUrl))
                {
                    return Redirect(notification.LinkUrl);
                }
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> MarkAllAsRead()
        {
            var unread = await _context.SystemNotifications.Where(n => !n.IsRead).ToListAsync();
            unread.ForEach(n => n.IsRead = true);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}