
namespace RentM.Infrastructure.Interfaces
{
    public interface IMotorcycleEventRepository
    {
        Task SaveEventAsync(MotorcycleEvent motorcycleEvent);
        Task<IEnumerable<MotorcycleEvent>> GetAllEventsAsync();
    }
}
