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

        public DateTime? DateOfBirth { get; set; } // تاريخ الميلاد

        [MaxLength(50)]
        public string Nationality { get; set; } = string.Empty; // الجنسية

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}