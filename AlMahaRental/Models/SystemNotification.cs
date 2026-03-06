using System;
using System.ComponentModel.DataAnnotations;

namespace AlMahaRental.Models
{
    public class SystemNotification
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; } = string.Empty; // عنوان الإشعار (مثال: حجز جديد)

        [Required]
        public string Message { get; set; } = string.Empty; // تفاصيل الإشعار

        [MaxLength(50)]
        public string Type { get; set; } = "General"; // نوع الإشعار (Booking, Message, User)

        [MaxLength(255)]
        public string LinkUrl { get; set; } = string.Empty; // الرابط الذي سيذهب إليه المدير عند النقر

        public bool IsRead { get; set; } = false; // حالة القراءة

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}