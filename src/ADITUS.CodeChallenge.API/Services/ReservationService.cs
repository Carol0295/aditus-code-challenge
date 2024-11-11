using System.Collections.Generic;
using System.Globalization;
using ADITUS.CodeChallenge.API.Domain;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Logging;

namespace ADITUS.CodeChallenge.API.Services
{
  public class ReservationService : IReservationService
  {
    private readonly IHardwareService _hardwareService;
    private readonly List<Reservation> _reservation;
    private readonly IEventsForReservationService _eventsForReservationService;

    public ReservationService(IHardwareService hardwareService)
    {
      _hardwareService = hardwareService;
      _reservation = new List<Reservation>();
      _eventsForReservationService = new EventsForReservationService();
    }
    /* Liefert die Informationen über eine Reservierung */
    public Task<Reservation> GetReservation(Guid id)
    {
      var Reservation = _reservation.FirstOrDefault(reservation => reservation.Id == id);
      return Task.FromResult(Reservation);
    }
    /* Funktion liefert alle Reservierungen */
    public Task<List<Reservation>> GetAllReservations()
    {
      return Task.FromResult(_reservation);
    }

    /* Funktion zum Erstellen von Reservierungen */
    public async Task<ReservationResult> AddReservation(Reservation Currentreservation)
    {
      var allAvailableHardware = await _hardwareService.GetAllHardware();

      // Prüfen ob es schon eine Reservierung für das event existiert 
      var existingReservationForEvent = _reservation.FirstOrDefault(reservation => reservation.EventId == Currentreservation.EventId);
      if (existingReservationForEvent != null)
      {
        return new ReservationResult
        {
          Success = false,
          Message = "Für diese Veranltung existiert schon eine Reservierung im System."
        };
      }

      //Alle Infos über der Veranstaltung holen und prüfen nach Datum. Wenn Reservierung weniger als 4 Wochen vor Event ist reservierung nicht möglich
      var myEventsInfo = await _eventsForReservationService.GetEventById(Currentreservation.EventId);
      var reserve = CanReserve(myEventsInfo.EventStartDate);
      if (!reserve)
      {
        return new ReservationResult
        {
          Success = false,
          Message = "Die Reservierung darf nicht ab 4 Wochen vor Veranstaltung gemacht werden."
        };
      }

      // Reservierung mit Daten befüllen, die schon da sind.
      var newReservation = new Reservation
      {
        Id = Guid.NewGuid(),
        EventId = Currentreservation.EventId,
        EventName = myEventsInfo.EventName,
        EventStartDate = myEventsInfo.EventStartDate,
        EventEndDate = myEventsInfo.EventEndDate,
        HardwareList = new List<HardwareForReservation>(),
      };

      // Die Liste von ausgewählte Hardwares und Mengen durchlaufen und Reservierung befüllen mit Daten
      foreach (var res in Currentreservation.HardwareList)
      {
        var availableHardware = await _hardwareService.GetHardware(res.HardwareId);
        
        // HardwareIds mit Menge in die Liste speichern
        newReservation.HardwareList.Add(new HardwareForReservation
        {
          HardwareId = res.HardwareId,
          HardwareName = availableHardware.Name,
          Quantity = res.Quantity
        });
      }

      // Reservierung in die Liste _reservation hinzufügen
      if (newReservation != null)
      {
        _reservation.Add(newReservation);
      }

      // Liste geht zurück zum Controller ReservationController, falls alles klappt
      return new ReservationResult
      {
        Success = true,
        Reservations = _reservation
      };
    }

    /* Prüfung nach der 4 Wochen vor der Veranstaltung */
    public bool CanReserve(DateTime eventDate )
    {
      return (eventDate - DateTime.Now).TotalDays >= 28;
    }

  }
}



