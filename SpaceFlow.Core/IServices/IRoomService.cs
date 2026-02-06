using SpaceFlow.Core.Dtos;
using SpaceFlow.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceFlow.Core.IServices
{
    public interface IRoomService
    {
        Task<RoomDto> GetRoomDetailsAsync(int id);
        Task<(IEnumerable<RoomDto> Rooms, int TotalPages)> GetPagedRoomsAsync(int page, int pageSize);
        Task<IEnumerable<RoomDto>> GetAllRoomsAsync();
        Task AddRoomAsync(Room room);
        Task UpdateRoomAsync(Room room);
        Task DeleteRoomAsync(int id);
    }
}
