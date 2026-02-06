using Microsoft.AspNetCore.Mvc;
using SpaceFlow.Core.IRepository;
using SpaceFlow.Core.IServices;
using SpaceFlow.Models;
using SpaceFlow.Web.ViewModels;
using System.Diagnostics;

namespace SpaceFlow.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRoomService _roomService; 

        public HomeController(IRoomService roomService) => _roomService = roomService;

        public IActionResult Index() => View();

        public async Task<IActionResult> BrowseSpaces(int page = 1)
        {
            int pageSize = 6;
            var (rooms, totalPages) = await _roomService.GetPagedRoomsAsync(page, pageSize);

            var viewModels = rooms.Select(r => new RoomViewModel
            {
                id = r.Id,
                Name = r.Name,
                Description = r.Description,
                ImageUrl = r.ImageUrl,
                PricePerHour = r.PricePerHour
            }).ToList();

            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = page;

            return View(viewModels);
        }
    }
}
