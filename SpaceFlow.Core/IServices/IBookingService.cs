using SpaceFlow.Core.Dtos;
using SpaceFlow.Core.Entities;

namespace SpaceFlow.Core.IServices
{
    public interface IBookingService
    {
        Task<(bool Success, string Message)> CreateBookingAsync(BookingRequestDto dto, string userId);
        Task<IEnumerable<BookingDto>> GetUserBookingsAsync(string userId);
        decimal CalculateTotalPrice(DateTime start, DateTime end, decimal pricePerHour);
    }
}
