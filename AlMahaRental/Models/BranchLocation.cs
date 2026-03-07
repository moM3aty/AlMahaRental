using System.ComponentModel.DataAnnotations;

namespace AlMahaRental.Models
{
    public class BranchLocation
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "اسم الفرع مطلوب")]
        [MaxLength(150)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(100)]
        public string WorkingHours { get; set; } = "من الأحد إلى الخميس 8:00 ص - 4:30 م";

        [MaxLength(20)]
        public string PhoneNumber { get; set; } = string.Empty;

        [MaxLength(500)]
        public string MapUrl { get; set; } = string.Empty;
    }
}