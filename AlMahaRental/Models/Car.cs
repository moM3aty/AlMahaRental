using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlMahaRental.Models
{
    public class Car
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string Category { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)")]
        public decimal DailyPrice { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal MonthlyPrice { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal OpenMileagePrice { get; set; }

        public int Passengers { get; set; } = 4;
        public int Doors { get; set; } = 4;
        public int Bags { get; set; } = 2;
        public string Transmission { get; set; } = "اتوماتيك";
        public int Year { get; set; }

        public string ImageUrl { get; set; } = string.Empty;

        public bool IsAvailable { get; set; } = true;

        // الحقل الجديد الخاص بالعروض الخاصة
        public bool IsSpecialOffer { get; set; } = false;
    }
}