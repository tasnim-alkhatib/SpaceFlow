namespace SpaceFlow.Web.ViewModels
{
    public class RoomViewModel
    {
        public int id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public decimal PricePerHour { get; set; }
        public int Capacity { get; set; }
        public string ShortDescription => 
            Description.Length > 50 ? Description.Substring(0, 50) + "..." : Description;
    }
}
