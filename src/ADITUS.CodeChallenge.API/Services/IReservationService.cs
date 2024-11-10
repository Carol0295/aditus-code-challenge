using ADITUS.CodeChallenge.API.Domain;

namespace ADITUS.CodeChallenge.API.Services
{
  public interface IReservationService
  {
    Task<Reservation> GetReservation(Guid id);
    Task<List<Reservation>> GetAllReservations();
    Task<bool> GetReservationbyEvent(Guid eventId);
    Task<List<Reservation>> AddReservation(Reservation reservation);
  }
}
