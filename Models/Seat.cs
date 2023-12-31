using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlaneTicket.Models
{
    public class Seat
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Seat Code")]
        public string SeatCode { get; set; }

        [Required]
        [Display(Name = "Class")]
        public string Class { get; set; } // e.g., Economy, Business

        [Required]
        [Display(Name = "Availability")]
        public bool IsAvailable { get; set; } = true; // By default, a seat is available

        // Foreign Key - Airplane
        [Required]
        [Display(Name = "Airplane")]
        public int AirplaneId { get; set; }

        // Navigation property
        [ForeignKey("AirplaneId")]
        public virtual Airplane Airplane { get; set; }
    }
}