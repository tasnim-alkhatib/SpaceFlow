using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using SpaceFlow.Core.IRepository;
using SpaceFlow.Core.IServices;
using SpaceFlow.Web.ViewModels;
using System.Net.WebSockets;

namespace SpaceFlow.Web.Controllers
{
    public class RoomsController : Controller
    {
        private readonly IRoomService _roomService;

        public RoomsController(IRoomService roomService) => _roomService = roomService;

        public async Task<IActionResult> Details(int id)
        {
            var viewModel = await _roomService.GetRoomDetailsAsync(id);

            if (viewModel == null) return NotFound();

            return View(viewModel);
        }
    }
}
