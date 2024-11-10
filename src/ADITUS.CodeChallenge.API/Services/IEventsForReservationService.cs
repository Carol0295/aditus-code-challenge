using ADITUS.CodeChallenge.API.Domain;

namespace ADITUS.CodeChallenge.API.Services
{
  public interface IEventsForReservationService
  {
    Task<IList<EventForReservation>> GetAllEventsForReservation();

    Task<EventForReservation> GetEventById(Guid id);
  }
}
