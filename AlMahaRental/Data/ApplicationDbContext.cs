using AlMahaRental.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AlMahaRental.Data
{
    // تحديث ملف الـ DbContext ليحتوي على جدول رسائل التواصل
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Car> Cars { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<ContactMessage> ContactMessages { get; set; }
        public DbSet<JobApplication> JobApplications { get; set; }
        public DbSet<BranchLocation> BranchLocations { get; set; }
        public DbSet<SystemNotification> SystemNotifications { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}