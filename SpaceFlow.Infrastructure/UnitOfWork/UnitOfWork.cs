using SpaceFlow.Core.Entities;
using SpaceFlow.Core.Interfaces;
using SpaceFlow.Infrastructure.Data;
using SpaceFlow.Infrastructure.Repositories;

namespace SpaceFlow.Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public IRoomRepository Rooms { get; private set; }
        public IBookingRepository Bookings { get; private set; }

        public UnitOfWork(AppDbContext context)
        {
            _context = context;

            Rooms = new RoomRepository(_context);
            Bookings = new BookingRepository(_context);
        }

        public async Task<int> CompleteAsync() => await _context.SaveChangesAsync();
        public void Dispose() => _context.Dispose();
    }
}
