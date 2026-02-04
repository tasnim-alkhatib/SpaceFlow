using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceFlow.Core.Entities
{
    public class Amenity
    {
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        public string? IconClass { get; set; } 

        public ICollection<Room> Rooms { get; set; } = new List<Room>();
    }
}
