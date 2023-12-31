using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlaneTicket.Models
{
    public class Reservation
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Reservation Date")]
        public DateTime ReservationDate { get; set; }

        [Display(Name = "Is Purchased")]
        public bool IsPurchased { get; set; } = false;

        // Computed property for Purchase Deadline
        [Display(Name = "Purchase Deadline")]
        public DateTime? PurchaseDeadline 
        {
            get
            {
                // Check if Flight is null before accessing its properties
                return Flight == null ? (DateTime?)null : Flight.DepartureTime.AddHours(-2);
            }
        }

        // Foreign Key - User
        [Required]
        public string UserId { get; set; }

        // Navigation property to User
        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        // Foreign Key - Flight
        [Required]
        public int FlightId { get; set; }

        // Navigation property to Flight
        [ForeignKey("FlightId")]
        public virtual Flight Flight { get; set; }

        // Foreign Key - Seat
        [Required]
        public int SeatId { get; set; }

        // Navigation property to Seat
        [ForeignKey("SeatId")]
        public virtual Seat Seat { get; set; }
    }
}