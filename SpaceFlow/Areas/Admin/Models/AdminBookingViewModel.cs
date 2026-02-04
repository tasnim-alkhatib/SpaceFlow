using SpaceFlow.Core.Enums;

namespace SpaceFlow.Web.Areas.Admin.Models
{
    public class AdminBookingViewModel
    {
        public int Id { get; set; }
        public string UserEmail { get; set; }
        public string RoomName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public decimal TotalPrice { get; set; }
        public BookingStatus Status { get; set; }
    }
}
