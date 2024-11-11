using ADITUS.CodeChallenge.API.Domain;

namespace ADITUS.CodeChallenge.API.Services
{
  public class EventsForReservationService : IEventsForReservationService
  {
    private readonly IList<EventForReservation> _eventsReservation;

    public EventsForReservationService()
    {
      _eventsReservation = new List<EventForReservation>
      {
        new EventForReservation
        {
          EventId = Guid.Parse("a8f52fb1-6e3c-486a-bb63-a74c71607f7d"),
          EventName = "ADITUS Event 1",
          EventStartDate = new DateTime(2024, 12, 26),
          EventEndDate = new DateTime(2024, 12, 26),
        },

        new EventForReservation
        {
          EventId = Guid.Parse("982a5d6d-40ee-41cd-ba42-78789836e6ac"),
          EventName = "ADITUS Event 2",
          EventStartDate = new DateTime(2024, 11, 17),
          EventEndDate = new DateTime(2024, 11, 18),
        },

        new EventForReservation
        {
          EventId = Guid.Parse("31432551-8568-4502-9b9a-4112194a97ed"),
          EventName = "ADITUS Event 3",
          EventStartDate = new DateTime(2025, 1, 8),
          EventEndDate = new DateTime(2025, 1, 10),
        },
        
        new EventForReservation
        {
          EventId = Guid.Parse("866a6e3f-8a7e-442c-b633-11e5d9d8d1da"),
          EventName = "ADITUS Event 4",
          EventStartDate = new DateTime(2025, 1, 8),
          EventEndDate = new DateTime(2025, 1, 10),
        }
      };
    }

    /* Funktion zum Liefern aller fiktive Events */
    public Task<IList<EventForReservation>> GetAllEventsForReservation()
    {
      return Task.FromResult(_eventsReservation);
    }

    /* Funktion zum Liefern die Informationen über einen Event */
    public Task<EventForReservation> GetEventById(Guid Id)
    {
      var infoEvent = _eventsReservation.FirstOrDefault(e => e.EventId == Id);
      return Task.FromResult(infoEvent);
    }
  }
}
