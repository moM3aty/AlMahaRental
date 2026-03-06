using System;
using System.ComponentModel.DataAnnotations;

namespace AlMahaRental.Models
{
    public class JobApplication
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "الاسم الكامل مطلوب")]
        [MaxLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "تاريخ الميلاد مطلوب")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "الجنسية مطلوبة")]
        [MaxLength(50)]
        public string Nationality { get; set; } = string.Empty;

        [Required(ErrorMessage = "البريد الإلكتروني مطلوب")]
        [EmailAddress]
        [MaxLength(150)]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "رقم الهوية مطلوب")]
        [MaxLength(20)]
        public string IdNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "رقم الجوال مطلوب")]
        [MaxLength(20)]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "القسم المفضل مطلوب")]
        [MaxLength(100)]
        public string Department { get; set; } = string.Empty;

        [Required(ErrorMessage = "المدينة مطلوبة")]
        [MaxLength(100)]
        public string City { get; set; } = string.Empty;

        [Required(ErrorMessage = "اللغات مطلوبة")]
        [MaxLength(200)]
        public string Languages { get; set; } = string.Empty;

        // هل يمتلك خبرة؟
        public bool HasExperience { get; set; }

        [Required(ErrorMessage = "تفاصيل الخبرة مطلوبة")]
        public string ExperienceDetails { get; set; } = string.Empty;

        // مسار ملف السيرة الذاتية على السيرفر
        public string ResumeUrl { get; set; } = string.Empty;

        [MaxLength(255)]
        public string? LinkedInUrl { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}