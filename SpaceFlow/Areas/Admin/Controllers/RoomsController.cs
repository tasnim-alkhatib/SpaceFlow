using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpaceFlow.Core.Entities;
using SpaceFlow.Core.Interfaces;
using SpaceFlow.Web.Areas.Admin.Models;

namespace SpaceFlow.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]    
    public class RoomsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public RoomsController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var rooms = await _unitOfWork.Rooms.GetAllAsync();

            var viewModel = rooms.Select(r => new RoomIndexViewModel
            {
                Id = r.Id,
                Name = r.Name,
                PricePerHour = r.PricePerHour,
                Capacity = r.Capacity,
                ImageUrl = r.ImageUrl
            }).ToList();


            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RoomFormViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string fileName = "default-room.jpg";
                    if (model.Image != null)
                    {
                        // Save the image to wwwroot/images/rooms
                        string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "images", "rooms");

                        // Ensure the directory exists
                        if (!Directory.Exists(uploadDir)) Directory.CreateDirectory(uploadDir);

                        // Create a unique file name
                        fileName = Guid.NewGuid().ToString() + "_" + model.Image.FileName;

                        // Combine the directory and file name to get the full path
                        string filePath = Path.Combine(uploadDir, fileName);

                        // Save the file
                        using var fileStream = new FileStream(filePath, FileMode.Create);
                        await model.Image.CopyToAsync(fileStream);
                    }

                    var room = new Room
                    {
                        Name = model.Name,
                        Description = model.Description,
                        PricePerHour = model.PricePerHour,
                        ImageUrl = fileName,
                        Capacity = model.Capacity
                    };

                    await _unitOfWork.Rooms.AddAsync(room); 
                    await _unitOfWork.CompleteAsync();
                    TempData["SuccessMessage"] = "Room created successfully!";

                    return RedirectToAction(nameof(Index));
                    
                }
            }catch(Exception e)
            {
                ModelState.AddModelError("", "An error occurred: " + e.Message);
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int roomId)
        {
            var room = await _unitOfWork.Rooms.GetByIdAsync(roomId);
            if(room == null ) return NotFound();

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
        public async Task<IActionResult> Edit(int roomId, RoomFormViewModel model)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    var room = await _unitOfWork.Rooms.GetByIdAsync(roomId);
                    if(room == null) return NotFound();

                    room.Name = model.Name;
                    room.Description = model.Description;
                    room.PricePerHour = model.PricePerHour;
                    room.Capacity = model.Capacity;

                    if (model.Image != null)
                    {
                        string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "images", "rooms");

                        if (!string.IsNullOrEmpty(room.ImageUrl) && room.ImageUrl != "default-room.jpg")
                        {
                            string oldPath = Path.Combine(uploadDir, room.ImageUrl);
                            if (System.IO.File.Exists(oldPath)) System.IO.File.Delete(oldPath);
                        }

                        string fileName = Guid.NewGuid().ToString() + "_" + model.Image.FileName;
                        string filePath = Path.Combine(uploadDir, fileName);

                        using var fileStream = new FileStream(filePath, FileMode.Create);
                        await model.Image.CopyToAsync(fileStream);

                        // Optionally delete the old image file here if needed
                        room.ImageUrl = fileName;
                    }
                    _unitOfWork.Rooms.Update(room);
                    await _unitOfWork.CompleteAsync();
                    TempData["SuccessMessage"] = "Updating done successfully!";

                    return RedirectToAction(nameof(Index));
                }
            }
            catch(Exception e)
            {
                ModelState.AddModelError("", "An error occurred: " + e.Message);
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int roomId)
        {
            var room = await _unitOfWork.Rooms.GetByIdAsync(roomId);
            if (room == null) return NotFound();

            if (!string.IsNullOrEmpty(room.ImageUrl) && room.ImageUrl != "default-room.jpg")
            {
                string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", "rooms", room.ImageUrl);
                if (System.IO.File.Exists(filePath)) System.IO.File.Delete(filePath);
            }

            _unitOfWork.Rooms.Delete(room);
            await _unitOfWork.CompleteAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
