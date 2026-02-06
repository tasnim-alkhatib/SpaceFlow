using Microsoft.EntityFrameworkCore;
using SpaceFlow.Core.Entities;
using SpaceFlow.Core.IRepository;
using SpaceFlow.Infrastructure.Data;

namespace SpaceFlow.Infrastructure.Repositories
{
    public class RoomRepository : GenericRepository<Room>, IRoomRepository
    {
        public RoomRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<Room>> GetAvailableRoomsAsync()
            => await _context.Rooms.Where(r => r.IsAvailable).OrderBy(r => r.PricePerHour).ToListAsync();
    }
}
