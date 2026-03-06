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
    }
}