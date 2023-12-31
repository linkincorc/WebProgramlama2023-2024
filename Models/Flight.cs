using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlaneTicket.Models
{
    public class Flight
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Flight Number")]
        public string FlightNumber { get; set; }

        [Required]
        [Display(Name = "Departure Time")]
        public DateTime DepartureTime { get; set; }

        [Required]
        [Display(Name = "Arrival Time")]
        public DateTime ArrivalTime { get; set; }

        // Stored Flight Duration
        [Display(Name = "Flight Duration")]
        public TimeSpan FlightDuration { get; set; }

        [Required]
        [Display(Name = "Origin")]
        public string Origin { get; set; }

        [Required]
        [Display(Name = "Destination")]
        public string Destination { get; set; }

        [Required]
        [Display(Name = "Price")]
        public decimal Price { get; set; }

        // Flight Status (e.g., On-Time, Delayed, Cancelled)
        [Display(Name = "Flight Status")]
        public string FlightStatus { get; set; }

        // Foreign Key - Airplane
        [Required]
        [Display(Name = "Airplane")]
        public int AirplaneId { get; set; }

        // Navigation property
        [ForeignKey("AirplaneId")]
        public virtual Airplane Airplane { get; set; }
    }
}