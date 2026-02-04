namespace SpaceFlow.Web.Areas.Admin.Models
{
    public class RoomIndexViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal PricePerHour { get; set; }
        public int Capacity { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
    }
}
