using SpaceFlow.Core.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using SpaceFlow.Core.CustomValidation;

namespace SpaceFlow.Core.Entities
{
    public class Booking
    {
        public int Id { get; set; }

        [Required]
        public int RoomId { get; set; }
        public Room Room { get; set; } = null!; // Navigation Property

        [Required]
        public string UserId { get; set; } = string.Empty;
        public IdentityUser User { get; set; } = null!; 

        [Required]
        [FutureDate(ErrorMessage = "Start Date Cannot be in the past")]
        public DateTime StartTime { get; set; }

        [Required]
        [DateGreater("StartTime")]
        public DateTime EndTime { get; set; }

        public decimal TotalPrice { get; set; }

        public BookingStatus Status { get; set; } = BookingStatus.Pending;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
