using SpaceFlow.Core.Dtos;
using SpaceFlow.Core.Entities;
using SpaceFlow.Core.IRepository;
using SpaceFlow.Core.IServices;

namespace SpaceFlow.Core.Services
{
    public class RoomService : IRoomService
    {
        private readonly IUnitOfWork _unitOfWork;

        public RoomService(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        private RoomDto MapToDto(Room room)
        {
            return new RoomDto
            {
                Id = room.Id,
                Name = room.Name,
                Description = room.Description,
                ImageUrl = room.ImageUrl,
                PricePerHour = room.PricePerHour,
                Capacity = room.Capacity
            };
        }
        public async Task<RoomDto> GetRoomDetailsAsync(int id)
        {
            var room = await _unitOfWork.Rooms.GetByIdAsync(id);
            if (room == null) return null;
            return MapToDto(room);
        }

        public async Task<(IEnumerable<RoomDto> Rooms, int TotalPages)> GetPagedRoomsAsync(int page, int pageSize)
        {
            var rooms = await _unitOfWork.Rooms.GetAllAsync();
            int totalRooms = rooms.Count();

            var pagedData = rooms
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(MapToDto)
                .ToList();

            int totalPages = (int)Math.Ceiling((double)totalRooms / pageSize);
            return (pagedData, totalPages);
        }

        public async Task<IEnumerable<RoomDto>> GetAllRoomsAsync()
        {
            var rooms = await _unitOfWork.Rooms.GetAllAsync();
            return rooms.Select(MapToDto).ToList(); ;
        }

        public async Task AddRoomAsync(Room room)
        {
            await _unitOfWork.Rooms.AddAsync(room);
            await _unitOfWork.CompleteAsync();
        }

        public async Task UpdateRoomAsync(Room room)
        {
            _unitOfWork.Rooms.Update(room);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteRoomAsync(int id)
        {
            var room = await _unitOfWork.Rooms.GetByIdAsync(id);
            if (room != null)
            {
                _unitOfWork.Rooms.Delete(room);
                await _unitOfWork.CompleteAsync();
            }
        }
    }
}
