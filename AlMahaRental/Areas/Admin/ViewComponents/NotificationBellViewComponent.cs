using AlMahaRental.Data;
using AlMahaRental.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace AlMahaRental.Areas.Admin.ViewComponents
{
    // مكون عرض لجلب إحصائيات الإشعارات الديناميكية في لوحة التحكم
    public class NotificationBellViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        // حقن DbContext (للحجوزات والرسائل) و UserManager (للمستخدمين الجدد)
        public NotificationBellViewComponent(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            // 1. حساب عدد الرسائل غير المقروءة
            int unreadMessagesCount = await _context.ContactMessages.CountAsync(m => !m.IsRead);
            
            // 2. حساب عدد الحجوزات الجديدة (قيد المراجعة)
            int pendingBookingsCount = await _context.Bookings.CountAsync(b => b.Status == "Pending");

            // 3. حساب عدد المستخدمين الذين سجلوا خلال آخر 24 ساعة
            var yesterday = DateTime.UtcNow.AddDays(-1);
            int newUsersCount = await _userManager.Users.CountAsync(u => u.CreatedAt >= yesterday);

            // إجمالي الإشعارات ليظهر بالرقم الأحمر
            int totalNotifications = unreadMessagesCount + pendingBookingsCount + newUsersCount;

            // تمرير البيانات لواجهة الإشعارات
            ViewBag.UnreadMessages = unreadMessagesCount;
            ViewBag.PendingBookings = pendingBookingsCount;
            ViewBag.NewUsers = newUsersCount;
            ViewBag.TotalCount = totalNotifications;

      
            // جلب آخر 5 إشعارات (سواء مقروءة أو لا) لعرضها في القائمة المنسدلة
            var latestNotifications = await _context.SystemNotifications
                .OrderByDescending(n => n.CreatedAt)
                .Take(5)
                .ToListAsync();

            // حساب عدد الإشعارات "غير المقروءة" فقط ليظهر الرقم الأحمر فوق الجرس
            ViewBag.UnreadCount = await _context.SystemNotifications.CountAsync(n => !n.IsRead);

            return View(latestNotifications);
        }
    }
}