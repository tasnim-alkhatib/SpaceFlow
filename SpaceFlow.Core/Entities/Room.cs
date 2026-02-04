using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceFlow.Core.Entities
{
    public class Room
    {
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        [Range(1, 1000)]
        public decimal PricePerHour { get; set; }

        public int Capacity { get; set; }

        public string? ImageUrl { get; set; } 

        public bool IsAvailable { get; set; } = true;

        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();

        public ICollection<Amenity> Amenities { get; set; } = new List<Amenity>();
    }
}
