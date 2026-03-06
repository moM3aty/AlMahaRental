using AlMahaRental.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AlMahaRental.Data
{
    public static class DbSeeder
    {
        public static async Task SeedRolesAndAdminAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var dbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();

            // 1. إنشاء الأدوار
            string[] roleNames = { "Admin", "User" };
            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // 2. إنشاء حساب المدير
            var adminEmail = "admin@almaha.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = "1000000000",
                    Email = adminEmail,
                    FirstName = "مدير",
                    LastName = "النظام",
                    IdNumber = "1000000000",
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(adminUser, "Admin@123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }

            // 3. زراعة الفروع لتظهر في الفورمات تلقائياً (التعديل الجديد)
            if (!await dbContext.BranchLocations.AnyAsync())
            {
                var branchesToSeed = new List<BranchLocation>
                {
                    new BranchLocation { Name = "الرياض - الفرع الرئيسي" },
                    new BranchLocation { Name = "مطار الملك خالد - الرياض" },
                    new BranchLocation { Name = "جدة - حي التحلية" },
                    new BranchLocation { Name = "مطار الملك عبدالعزيز - جدة" },
                    new BranchLocation { Name = "الدمام - الفرع الرئيسي" },
                    new BranchLocation { Name = "مطار أبها" },
                    new BranchLocation { Name = "مطار الأحساء" }
                };

                await dbContext.BranchLocations.AddRangeAsync(branchesToSeed);
                await dbContext.SaveChangesAsync();
            }

            // 4. زراعة السيارات
            if (!await dbContext.Cars.AnyAsync())
            {
                var currentYear = DateTime.Now.Year;

                var carsToSeed = new List<Car>
                {
                    new Car { Name = "سوزوكي ديز اير", DailyPrice = 100, MonthlyPrice = 2400, OpenMileagePrice = 195, Category = "اقتصادية", Passengers = 4, Doors = 4, Bags = 2, Transmission = "اتوماتيك", Year = currentYear, ImageUrl = "/images/car-1.jpg", IsAvailable = true, IsSpecialOffer = false },
                    new Car { Name = "ال سيفن", DailyPrice = 100, MonthlyPrice = 2400, OpenMileagePrice = 195, Category = "اقتصادية", Passengers = 4, Doors = 4, Bags = 2, Transmission = "اتوماتيك", Year = currentYear, ImageUrl = "/images/car-2.jpg", IsAvailable = true, IsSpecialOffer = false },
                    new Car { Name = "اكسنت", DailyPrice = 140, MonthlyPrice = 2800, OpenMileagePrice = 280, Category = "اقتصادية", Passengers = 4, Doors = 4, Bags = 2, Transmission = "اتوماتيك", Year = currentYear, ImageUrl = "/images/car-1.jpg", IsAvailable = true, IsSpecialOffer = true },
                    new Car { Name = "بيجاس", DailyPrice = 120, MonthlyPrice = 2600, OpenMileagePrice = 260, Category = "اقتصادية", Passengers = 4, Doors = 4, Bags = 2, Transmission = "اتوماتيك", Year = currentYear, ImageUrl = "/images/car-2.jpg", IsAvailable = true, IsSpecialOffer = false },
                    new Car { Name = "كيا k3", DailyPrice = 150, MonthlyPrice = 2900, OpenMileagePrice = 300, Category = "صغيرة", Passengers = 4, Doors = 4, Bags = 2, Transmission = "اتوماتيك", Year = currentYear, ImageUrl = "/images/car-1.jpg", IsAvailable = true, IsSpecialOffer = false },
                    new Car { Name = "سوناتا", DailyPrice = 300, MonthlyPrice = 4600, OpenMileagePrice = 500, Category = "عائلية", Passengers = 5, Doors = 4, Bags = 3, Transmission = "اتوماتيك", Year = currentYear, ImageUrl = "/images/car-2.jpg", IsAvailable = true, IsSpecialOffer = true },
                    new Car { Name = "كونا واجن", DailyPrice = 180, MonthlyPrice = 3200, OpenMileagePrice = 350, Category = "عائلية", Passengers = 5, Doors = 4, Bags = 3, Transmission = "اتوماتيك", Year = currentYear, ImageUrl = "/images/car-1.jpg", IsAvailable = true, IsSpecialOffer = false },
                    new Car { Name = "كيا سيلتوس", DailyPrice = 280, MonthlyPrice = 4200, OpenMileagePrice = 500, Category = "عائلية", Passengers = 5, Doors = 4, Bags = 3, Transmission = "اتوماتيك", Year = currentYear, ImageUrl = "/images/car-2.jpg", IsAvailable = true, IsSpecialOffer = false },
                    new Car { Name = "يوكون", DailyPrice = 900, MonthlyPrice = 16000, OpenMileagePrice = 1200, Category = "دفع رباعي", Passengers = 7, Doors = 4, Bags = 5, Transmission = "اتوماتيك", Year = currentYear, ImageUrl = "/images/car-1.jpg", IsAvailable = true, IsSpecialOffer = false },
                    new Car { Name = "يوني تي", DailyPrice = 500, MonthlyPrice = 6000, OpenMileagePrice = 750, Category = "دفع رباعي", Passengers = 5, Doors = 4, Bags = 3, Transmission = "اتوماتيك", Year = currentYear, ImageUrl = "/images/car-2.jpg", IsAvailable = true, IsSpecialOffer = false },
                    new Car { Name = "75 بلس", DailyPrice = 450, MonthlyPrice = 5500, OpenMileagePrice = 730, Category = "دفع رباعي", Passengers = 5, Doors = 4, Bags = 3, Transmission = "اتوماتيك", Year = currentYear, ImageUrl = "/images/car-1.jpg", IsAvailable = true, IsSpecialOffer = false },
                    new Car { Name = "تيرتوري", DailyPrice = 450, MonthlyPrice = 5500, OpenMileagePrice = 730, Category = "دفع رباعي", Passengers = 5, Doors = 4, Bags = 3, Transmission = "اتوماتيك", Year = currentYear, ImageUrl = "/images/car-2.jpg", IsAvailable = true, IsSpecialOffer = false },
                    new Car { Name = "تورس", DailyPrice = 350, MonthlyPrice = 4500, OpenMileagePrice = 690, Category = "فخمة", Passengers = 5, Doors = 4, Bags = 3, Transmission = "اتوماتيك", Year = currentYear, ImageUrl = "/images/car-1.jpg", IsAvailable = true, IsSpecialOffer = true },
                    new Car { Name = "يوني في", DailyPrice = 300, MonthlyPrice = 4300, OpenMileagePrice = 600, Category = "فخمة", Passengers = 5, Doors = 4, Bags = 3, Transmission = "اتوماتيك", Year = currentYear, ImageUrl = "/images/car-2.jpg", IsAvailable = true, IsSpecialOffer = false }
                };

                await dbContext.Cars.AddRangeAsync(carsToSeed);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}