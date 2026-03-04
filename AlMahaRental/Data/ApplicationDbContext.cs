using AlMahaRental.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace AlMahaRental.Data
{
    // ربط الـ Models بقاعدة البيانات باستخدام Entity Framework Core
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Car> Cars { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        // سيتم إضافة المزيد من الجداول مثل الفروع (Branches) والتواصل (ContactMessages) لاحقاً

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // يمكنك إضافة أي قيود إضافية على الجداول هنا إذا لزم الأمر
        }
    }
}