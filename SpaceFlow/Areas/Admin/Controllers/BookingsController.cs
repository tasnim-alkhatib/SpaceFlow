using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpaceFlow.Core.Enums;
using SpaceFlow.Core.Interfaces;
using SpaceFlow.Web.Areas.Admin.Models;
using System.Reflection.Metadata.Ecma335;

namespace SpaceFlow.Web.Areas.Admin.Controllers
{ 
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class BookingsController : Controller    
    {
        private readonly IUnitOfWork _unitOfWork;

        public BookingsController(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<IActionResult> Index()
        {
            var bookings = await _unitOfWork.Bookings.GetAllWithDetailsAsync();

            var viewModel = bookings.Select(b => new AdminBookingViewModel
            {
                Id = b.Id,
                UserEmail = b.User?.Email ?? "No Email",
                RoomName = b.Room?.Name ?? "N/A",
                StartTime = b.StartTime,
                EndTime = b.EndTime,
                TotalPrice = b.TotalPrice,
                Status = b.Status
            }).ToList();

            return View(viewModel); 
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int id, BookingStatus status)
        {
            var booking = await _unitOfWork.Bookings.GetByIdAsync(id);
            if (booking == null) return NotFound();

            booking.Status = status; 
            await _unitOfWork.CompleteAsync();

            var badgeClass = status.GetBadgeClass();

            return Content($@"<span class='badge {badgeClass}'>{status}</span>", "text/html");
        }
    }
}
