using SpaceFlow.Core.Dtos;
using SpaceFlow.Core.IRepository;
using SpaceFlow.Core.IServices;
using SpaceFlow.Core.Enums;

namespace SpaceFlow.Core.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IUnitOfWork _unitOfWork;
        public DashboardService(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<AdminDashboardDto> GetStatsAsync()
        {
            var allRooms = await _unitOfWork.Rooms.GetAllAsync();
            var totalRooms = allRooms.Count();

            var allBookings = await _unitOfWork.Bookings.GetAllAsync();
            var totalBookings = allBookings.Count();

            var pendingBookings = allBookings.Count(b => b.Status == BookingStatus.Pending);

            var totalRevenue = allBookings.Sum(b => b.TotalPrice);

            return new AdminDashboardDto
            {
                TotalRooms = totalRooms,
                TotalBookings = totalBookings,
                PendingBookings = pendingBookings,
                TotalRevenue = totalRevenue
            };
        }
    }
}
