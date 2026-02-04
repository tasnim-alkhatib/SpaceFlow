using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SpaceFlow.Core.Entities;
using SpaceFlow.Core.Interfaces;
using SpaceFlow.Web.ViewModels;
using System.Security.Claims;

namespace SpaceFlow.Web.Controllers
{
    [Authorize] 
    public class BookingsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;

        public BookingsController(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Create(int spaceId)
        {
            var room = await _unitOfWork.Rooms.GetByIdAsync(spaceId);
            if(room == null) return NotFound();

            var viewModel = new BookingViewModel
            {
                RoomId = room.Id,
                RoomName = room.Name,
                RoomImageUrl = room.ImageUrl,
                PricePerHour = room.PricePerHour,
                StartTime = DateTime.Now,
                EndTime = DateTime.Now,
            };

            return View(viewModel); 
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BookingViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var overLappingBookings = await _unitOfWork.Bookings.GetOverlappingBookingsAsync(
                        model.RoomId, model.StartTime, model.EndTime);

                    if (overLappingBookings != null)
                    {
                        TempData["ErrorMessage"] = $"This room is already booked from {overLappingBookings.StartTime:HH:mm} to {overLappingBookings.EndTime:HH:mm}.\nPlease choose another date.";
                        return View(model);
                    }

                    var duration = (model.EndTime - model.StartTime).TotalHours;

                    var booking = new Booking
                    {
                        RoomId = model.RoomId,
                        UserId = _userManager.GetUserId(User) ?? "",
                        StartTime = model.StartTime,
                        EndTime = model.EndTime,
                        TotalPrice = Math.Round(model.PricePerHour * (decimal)duration, 2), // Default 1 hour
                        CreatedAt = DateTime.Now
                    };

                    await _unitOfWork.Bookings.AddAsync(booking);
                    await _unitOfWork.CompleteAsync();
                    TempData["SuccessMessage"] = "Booking created successfully!";
                    return RedirectToAction("MyBookings"); 
                }
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = $"Error while booking: {e.Message}";
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> MyBookings()
        {
            var userId = _userManager.GetUserId(User) ?? "";
            if (userId == null) return Challenge();

            var userBookings = await _unitOfWork.Bookings.GetUserBookingsAsync(userId);

            return View(userBookings);
        }

        [HttpPost]
        public IActionResult GetPrice(BookingViewModel model)
        {
            var duration = (model.EndTime - model.StartTime).TotalHours;
            if (duration <= 0) return Content("0.00 LE");

            var total = (decimal)duration * model.PricePerHour;
            return Content($"{total:N2} LE"); 
        }

    }
}