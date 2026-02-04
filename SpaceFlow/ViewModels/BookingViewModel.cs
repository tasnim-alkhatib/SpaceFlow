using SpaceFlow.Core.CustomValidation;
using System.ComponentModel.DataAnnotations;

namespace SpaceFlow.Web.ViewModels
{
    public class BookingViewModel
    {
        public int RoomId { get; set; }
        public string? RoomName { get; set; }
        public string? RoomImageUrl { get; set; }
        public decimal PricePerHour { get; set; }

        [Required(ErrorMessage = "Please Select a Start Time")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-ddTHH:mm}")]
        [FutureDate(ErrorMessage = "Start Date Cannot be in the past")]
        public DateTime StartTime { get; set; } 

        [Required(ErrorMessage = "Please Select an End Time")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-ddTHH:mm}")]
        [DateGreater("StartTime")]
        public DateTime EndTime { get; set; }

        public decimal TotalPrice =>
            Math.Round((decimal)(EndTime - StartTime).TotalHours * PricePerHour, 2);
    }
}
