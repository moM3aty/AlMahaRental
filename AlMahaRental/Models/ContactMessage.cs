using System;
using System.ComponentModel.DataAnnotations;

namespace AlMahaRental.Models
{
    // نموذج لحفظ رسائل الزوار من صفحة "اتصل بنا"
    public class ContactMessage
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "الاسم مطلوب")]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "البريد الإلكتروني مطلوب")]
        [EmailAddress]
        [MaxLength(150)]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "رقم الجوال مطلوب")]
        [MaxLength(20)]
        public string PhoneNumber { get; set; } = string.Empty;

        [MaxLength(100)]
        public string Topic { get; set; } = string.Empty; // الاستفسار / الموضوع

        [MaxLength(100)]
        public string City { get; set; } = string.Empty;

        [Required(ErrorMessage = "محتوى الرسالة مطلوب")]
        public string MessageBody { get; set; } = string.Empty;

        public bool IsRead { get; set; } = false; // لمعرفة هل قام المدير بقراءة الرسالة أم لا

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}