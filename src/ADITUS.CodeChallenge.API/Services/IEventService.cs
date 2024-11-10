using ADITUS.CodeChallenge.API.Domain;

namespace ADITUS.CodeChallenge.API.Services
{
  public interface IEventService
  {
    Task<Event> GetEvent(Guid id);
    Task<HybridEvent> GetEventStatistics(Guid id, EventType type);
    Task<IList<Event>> GetEvents();
    Task<IList<HybridEvent>> GetEventsWithStatistics();
  }
}