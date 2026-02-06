using Microsoft.EntityFrameworkCore;
using SpaceFlow.Core.Entities;
using SpaceFlow.Core.IRepository;
using SpaceFlow.Infrastructure.Data;
using System;
using System.Collections.Generic;
using SpaceFlow.Core.Enums;

namespace SpaceFlow.Infrastructure.Repositories
{
    public class BookingRepository : GenericRepository<Booking>, IBookingRepository
    {
        public BookingRepository(AppDbContext context) : base(context){}

        // for admin
        public async Task<IEnumerable<Booking>> GetAllWithDetailsAsync()
        {
            return await _context.Bookings
                .Include(b => b.User) 
                .Include(b => b.Room)
                .ToListAsync();
        }

        public async Task<Booking?> GetOverlappingBookingsAsync(int roomId, DateTime start, DateTime end)
        {
            return await _context.Bookings
                .Where(b => b.RoomId == roomId && b.Status != BookingStatus.Cancelled)
                .FirstOrDefaultAsync(b => start < b.EndTime && end > b.StartTime);
        }

        public async Task<IEnumerable<Booking>> GetUserBookingsAsync(string userId)
        {
            return await _context.Bookings
                .Include(b => b.Room) 
                .Where(b => b.UserId == userId)
                .ToListAsync();
        }
    }
}
