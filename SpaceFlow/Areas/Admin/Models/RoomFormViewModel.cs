using System.ComponentModel.DataAnnotations;

namespace SpaceFlow.Web.Areas.Admin.Models
{
    public class RoomFormViewModel
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }

        [Range(1, 10000)]
        public decimal PricePerHour { get; set; }
        public int Capacity { get; set; }
        public IFormFile? Image { get; set; } 
        public string? ImageUrl { get; set; }
    }
}
