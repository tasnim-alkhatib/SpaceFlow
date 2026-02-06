using Microsoft.AspNetCore.Identity;
using SpaceFlow.Core.Dtos;
using SpaceFlow.Core.Entities;
using SpaceFlow.Core.Enums;
using SpaceFlow.Core.IRepository;
using SpaceFlow.Core.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceFlow.Core.Services
{
    public class BookingService : IBookingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;

        public BookingService(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task<(bool Success, string Message)> CreateBookingAsync(BookingRequestDto dto, string userId)
        {
            var overlapping = await _unitOfWork.Bookings.GetOverlappingBookingsAsync(dto.RoomId, dto.StartTime, dto.EndTime);
            
            if (overlapping != null) 
                return (false, $"The room is already booked from {overlapping.StartTime:HH:mm} to {overlapping.EndTime:HH:mm}. \nPlease choose another date");

            var duration = (dto.EndTime - dto.StartTime).TotalHours;

            var booking = new Booking
            {
                RoomId = dto.RoomId,
                UserId = userId,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime,
                TotalPrice = Math.Round(dto.PricePerHour * (decimal)duration, 2), // Default 1 hour
                Status = BookingStatus.Pending,
                CreatedAt = DateTime.Now
            };

            await _unitOfWork.Bookings.AddAsync(booking);
            await _unitOfWork.CompleteAsync();

            return(true, "Booking created successfully!");
        }

        public async Task<IEnumerable<BookingDto>> GetUserBookingsAsync(string userId)
        {
            var bookings = await _unitOfWork.Bookings.GetUserBookingsAsync(userId);

            return bookings.Select(b => new BookingDto
            {
                //Id = b.Id,
                RoomId = b.RoomId,
                RoomName = b.Room?.Name,
                StartTime = b.StartTime,
                EndTime = b.EndTime,
                TotalPrice = b.TotalPrice,
                Status = b.Status
            }).ToList();
        }

        public decimal CalculateTotalPrice(DateTime start, DateTime end, decimal pricePerHour)
        {
            var duration = (end - start).TotalHours;
            if (duration <= 0) return 0;

            return Math.Round(pricePerHour * (decimal)duration, 2);
        }
    }
}
