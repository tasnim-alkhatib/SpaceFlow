using Microsoft.AspNetCore.Mvc;
using SpaceFlow.Core.Interfaces;
using SpaceFlow.Models;
using SpaceFlow.Web.ViewModels;
using System.Diagnostics;

namespace SpaceFlow.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public IActionResult Index() => View();

        public async Task<IActionResult> BrowseSpaces()
        {
            var rooms = await _unitOfWork.Rooms.GetAllAsync();
            
            var viewModel = rooms.Select(room => new RoomViewModel
            {
                id = room.Id,
                Name = room.Name,
                Description = room.Description,
                ImageUrl = room.ImageUrl,
                PricePerHour = room.PricePerHour
            }).ToList();

            return View(viewModel);
        }
    }
}
