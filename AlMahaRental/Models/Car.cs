using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlMahaRental.Models
{
    // نموذج السيارة بناءً على ملف الـ CSV وتصميم الـ HTML
    public class Car
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty; // نوع السيارة (مثل: سوناتا, يوكون)

        [Required]
        [MaxLength(50)]
        public string Category { get; set; } = string.Empty; // فئة السيارة (اقتصادية، فخمة، عائلية)

        // الأسعار مأخوذة من ملف الـ CSV المرفق
        [Column(TypeName = "decimal(18,2)")]
        public decimal DailyPrice { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal MonthlyPrice { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal OpenMileagePrice { get; set; } // عداد مفتوح

        // المواصفات (من تصميم الـ HTML)
        public int Passengers { get; set; } = 4;
        public int Doors { get; set; } = 4;
        public int Bags { get; set; } = 2;
        public string Transmission { get; set; } = "اتوماتيك"; // اتوماتيك او عادي
        public int Year { get; set; }

        public string ImageUrl { get; set; } = string.Empty; // مسار صورة السيارة

        public bool IsAvailable { get; set; } = true; // هل السيارة متاحة للحجز؟
    }
}