using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SpaceFlow.Core.Dtos;
using SpaceFlow.Core.Entities;
using SpaceFlow.Core.IRepository;
using SpaceFlow.Core.IServices;
using SpaceFlow.Infrastructure.UnitOfWork;
using SpaceFlow.Web.ViewModels;
using System.Security.Claims;

namespace SpaceFlow.Web.Controllers
{
    [Authorize] 
    public class BookingsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;   
        private readonly IBookingService _bookingService;
        private readonly UserManager<IdentityUser> _userManager;

        public BookingsController(IUnitOfWork unitOfWork, IBookingService bookingService, UserManager<IdentityUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _bookingService = bookingService;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Create(int spaceId)
        {
            var room = await _unitOfWork.Rooms.GetByIdAsync(spaceId);

            if (room == null) return NotFound();

            var viewModel = new BookingViewModel
            {
                RoomId = room.Id,
                RoomName = room.Name,
                RoomImageUrl = room.ImageUrl,
                PricePerHour = room.PricePerHour,
                StartTime = DateTime.Now,
                EndTime = DateTime.Now
            };

            return View(viewModel); 
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BookingViewModel model)
        {
            async Task RePopulateRoomData()
            {
                var room = await _unitOfWork.Rooms.GetByIdAsync(model.RoomId);
                if (room != null)
                {
                    model.RoomImageUrl = room.ImageUrl;
                    model.RoomName = room.Name;
                    model.PricePerHour = room.PricePerHour;
                }
            }

            if (!ModelState.IsValid)
            {
                await RePopulateRoomData();
                return View(model);
            }

            var bookingDto = new BookingRequestDto
            {
                RoomId = model.RoomId,
                StartTime = model.StartTime,
                EndTime = model.EndTime,
                PricePerHour = model.PricePerHour
            };

            var userId = _userManager.GetUserId(User) ?? "";
            var result = await _bookingService.CreateBookingAsync(bookingDto, userId);

            if (result.Success)
            {
                TempData["SuccessMessage"] = result.Message;
                return RedirectToAction(nameof(MyBookings));
            }

            TempData["ErrorMessage"] = result.Message;
            await RePopulateRoomData();
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> MyBookings()
        {
            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(userId)) return Challenge();

            var userBookings = await _bookingService.GetUserBookingsAsync(userId);

            return View(userBookings);
        }


        [HttpPost]
        public IActionResult GetPrice(BookingViewModel model)
        {
            var duration = _bookingService.CalculateTotalPrice(model.StartTime, model.EndTime, model.PricePerHour);
            return Content($"{duration:N2} LE"); 
        }

    }
}