using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlMahaRental.Models
{
    public class Booking
    {
        [Key]
        public int Id { get; set; }

        // رقم الحجز الذي سيستخدمه العميل لتتبع حجزه
        [Required]
        [MaxLength(50)]
        public string BookingReference { get; set; } = Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper();

        [Required]
        public string UserId { get; set; } = string.Empty;
        [ForeignKey("UserId")]
        public ApplicationUser? User { get; set; }

        [Required]
        public int CarId { get; set; }
        [ForeignKey("CarId")]
        public Car? Car { get; set; }

        [Required]
        public DateTime PickupDate { get; set; }

        [Required]
        public DateTime ReturnDate { get; set; }

        [Required]
        [MaxLength(100)]
        public string PickupLocation { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string ReturnLocation { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalPrice { get; set; }

        [Required]
        public string Status { get; set; } = "Pending"; // Pending, Confirmed, Cancelled, Completed

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}