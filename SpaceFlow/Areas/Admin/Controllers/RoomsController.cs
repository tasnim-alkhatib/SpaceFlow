using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpaceFlow.Core.Entities;
using SpaceFlow.Core.IRepository;
using SpaceFlow.Core.IServices;
using SpaceFlow.Infrastructure.FileStorage;
using SpaceFlow.Infrastructure.UnitOfWork;
using SpaceFlow.Web.Areas.Admin.Models;

namespace SpaceFlow.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class RoomsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRoomService _roomService;
        private readonly IFileService _fileService;

        public RoomsController(IUnitOfWork unitOfWork, IRoomService roomService, IFileService fileService)
        {
            _unitOfWork = unitOfWork;
            _roomService = roomService;
            _fileService = fileService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var rooms = await _roomService.GetAllRoomsAsync();
            return View(rooms);
        }

        [HttpGet]
        public async Task<IActionResult> Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RoomFormViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            try
            {
                string fileName = "default-room.jpg";

                if (model.Image != null)
                {
                    using var stream = model.Image.OpenReadStream();
                    fileName = await _fileService.UploadFileAsync(stream, model.Image.FileName, "rooms");
                }

                var room = new Room
                {
                    Name = model.Name,
                    Description = model.Description,
                    PricePerHour = model.PricePerHour,
                    Capacity = model.Capacity,
                    ImageUrl = fileName
                };

                await _roomService.AddRoomAsync(room);
                TempData["SuccessMessage"] = "Room created successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", "Something went wrong: " + e.Message);
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int roomId)
        {
            var room = await _roomService.GetRoomDetailsAsync(roomId);
            if (room == null) return NotFound();

            var viewModel = new RoomFormViewModel
            {
                Id = room.Id,
                Name = room.Name,
                Description = room.Description,
                PricePerHour = room.PricePerHour,
                ImageUrl = room.ImageUrl,
                Capacity = room.Capacity
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(RoomFormViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            try
            {
                var roomToUpdate = await _unitOfWork.Rooms.GetByIdAsync(model.Id);

                if (roomToUpdate == null) return NotFound();

                string fileName = roomToUpdate.ImageUrl;
                if (model.Image != null)
                {
                    if (fileName != "default-room.jpg") _fileService.DeleteFile(fileName, "rooms");
                    using var stream = model.Image.OpenReadStream();
                    fileName = await _fileService.UploadFileAsync(stream, model.Image.FileName, "rooms");
                }

                roomToUpdate.Name = model.Name;
                roomToUpdate.Description = model.Description;
                roomToUpdate.PricePerHour = model.PricePerHour;
                roomToUpdate.Capacity = model.Capacity;
                roomToUpdate.ImageUrl = fileName;

                await _roomService.UpdateRoomAsync(roomToUpdate);

                TempData["SuccessMessage"] = "Room updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", "Error: " + e.Message);
                return View(model);
            }
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var room = await _roomService.GetRoomDetailsAsync(id);
            if (room != null)
            {
                // delete the image file if it's not the default one
                if (room.ImageUrl != "default-room.jpg") _fileService.DeleteFile(room.ImageUrl, "rooms");

                // delete the room from db
                await _roomService.DeleteRoomAsync(id);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}