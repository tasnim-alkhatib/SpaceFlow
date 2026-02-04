using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using SpaceFlow.Core.Interfaces;
using SpaceFlow.Web.ViewModels;
using System.Net.WebSockets;

namespace SpaceFlow.Web.Controllers
{
    public class RoomsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public RoomsController(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<IActionResult> Details(int id)
        {
            var room = await _unitOfWork.Rooms.GetByIdAsync(id);
            if (room == null) return NotFound();

            var viewModel = new RoomViewModel
            {
                id = room.Id,
                Name = room.Name,
                Description = room.Description,
                ImageUrl = room.ImageUrl,
                PricePerHour = room.PricePerHour,
                Capacity = room.Capacity
            };

            return View(viewModel);
        }
    }
}
