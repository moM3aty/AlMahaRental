using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace AlMahaRental.Models
{
    // توسيع الـ IdentityUser ليحتوي على البيانات الخاصة بالعميل من فورم التسجيل
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string IdNumber { get; set; } = string.Empty; // رقم الهوية

        // يمكننا إضافة حقول أخرى مثل تاريخ الميلاد أو الجنسية لاحقاً إذا لزم الأمر
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}