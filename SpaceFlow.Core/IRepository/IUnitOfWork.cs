using SpaceFlow.Core.Entities;

namespace SpaceFlow.Core.IRepository
{
    public interface IUnitOfWork : IDisposable
    {
        IRoomRepository Rooms { get; }
        IBookingRepository Bookings { get; }
        Task<int> CompleteAsync();
    }
}
