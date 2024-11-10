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
    //Liefert die Informationen über eine Reservierung
    public Task<Reservation> GetReservation(Guid id)
    {
      var Reservation = _reservation.FirstOrDefault(reservation => reservation.Id == id);
      return Task.FromResult(Reservation);
    }
    //Liefert alle Reservierungen
    public Task<List<Reservation>> GetAllReservations()
    {
      return Task.FromResult(_reservation);
    }

    public Task<bool> GetReservationbyEvent(Guid eventId)
    {
      var eventWithReservation = _reservation.Any(reservation => reservation.EventId == eventId);
      return Task.FromResult(eventWithReservation);
    } // quizaus puedo borrar esto

    public async Task<List<Reservation>> AddReservation(Reservation Currentreservation)
    {
      // Lista de reservas para un evento
      var eventReservations = new List<Reservation>();
      // Alle Informationen über alle Hardware
      var allAvailableHardware = await _hardwareService.GetAllHardware();
      
      //var currentAvailableHardware = new List<Hardware>();
      // Prüfen ob es schon eine Reservierung für das event existiert 
      var existingReservationForEvent = _reservation.FirstOrDefault(r => r.EventId == Currentreservation.EventId);

      // Wenn schon eine Reservierung für das Event existiert, dann ist die Reservierung nicht möglich
      if (existingReservationForEvent != null)
      {
        return new List<Reservation>();
      }
      var myEventsInfo = await _eventsForReservationService.GetEventById(Currentreservation.EventId);
      var reserve = CanReserve(myEventsInfo.EventStartDate);

      // Wenn Reservierung weniger als 4 Wochen vor Event ist reservierung nicht möglich
      if (!reserve)
      {
        return new List<Reservation>();
      }

      var eventReservationId = Guid.NewGuid();
      var newReservation = new Reservation
      {
        Id = eventReservationId,
        EventId = Currentreservation.EventId,
        EventName = myEventsInfo.EventName,
        EventStartDate = myEventsInfo.EventStartDate,
        EventEndDate = myEventsInfo.EventEndDate,
        HardwareList = new List<HardwareForReservation>(),
      };

      foreach (var res in Currentreservation.HardwareList)
      {
          // Holen der verfügbare Hardware, je nach Hardware Id
          var availableHardware = await _hardwareService.GetHardware(res.HardwareId);
          // Verificar si el hardware no existe o si la cantidad disponible es menor que la cantidad solicitada
          if (availableHardware == null || availableHardware.TotalAmount < res.Quantity)
          {
            return new List<Reservation>();
          }

        // Calcular la cantidad total reservada de ese hardware para todos los eventos en la misma fecha
        //var reservedQuantityForSameDayEvents = _reservation
        //    .Where(r => r.EventStartDate.Date == reservation.EventStartDate.Date &&
        //                r.EventEndDate.Date == reservation.EventEndDate.Date)
        //    .Sum(r => r.HardwareList.Where(h => h.HardwareId == res.HardwareId).Sum(h => h.Quantity));

        //var reservedQuantityForSameDayEvents = 0;
        var hasSameDayReservations = _reservation.Any(r => r.EventStartDate.Date == newReservation.EventStartDate.Date &&
                                                            r.EventEndDate.Date == newReservation.EventEndDate.Date);
        var currentAvailableHardware = new List<Hardware>();
        if (hasSameDayReservations)
        {
          // Calcular la cantidad total reservada para el hardware en el mismo día
          foreach (var existingReservation in _reservation)
          {
            foreach (var hardware in existingReservation.HardwareList)
            {
              if (hardware.HardwareId == res.HardwareId)
              {
                foreach (var AvHardware in allAvailableHardware)
                {
                  currentAvailableHardware.Add(
                    new Hardware
                    {
                      Id = AvHardware.Id,
                      Name = AvHardware.Name,
                      TotalAmount = AvHardware.TotalAmount - hardware.Quantity,
                    }
                  );
                }
                //reservedQuantityForSameDayEvents += hardware.Quantity;
              }
            }
          }

          foreach (var currHardwareInfos in currentAvailableHardware)
          {
            if (res.Quantity > currHardwareInfos.TotalAmount)
            {
              continue; //Keine Reservierung ist möglich da nicht genug Menge an Hardware gibt
            }

            newReservation.HardwareList.Add(new HardwareForReservation
            {
              HardwareId = res.HardwareId,
              HardwareName = availableHardware.Name,
              Quantity = res.Quantity
            });
          }
        } else
        {
          newReservation.HardwareList.Add(new HardwareForReservation
          {
            HardwareId = res.HardwareId,
            HardwareName = availableHardware.Name,
            Quantity = res.Quantity
          });
        }


        //var availableQuantityForEvent = availableHardware.TotalAmount - reservedQuantityForSameDayEvents;

        // Si la cantidad solicitada excede la cantidad disponible para el evento
        
        



        // Prüfen, dass es genug Hardware gibt
        //var hardware = await _hardwareService.GetHardware(res.Id);

        //var reservedQuantityForSameDayEvents = _reservation
        //        .Where(r => r.EventStartDate.Date == myEventsInfo.EventStartDate.Date &&
        //                   r.EventEndDate.Date == myEventsInfo.EventEndDate.Date)
        //        .Sum(r => r.HardwareList.Where(h => h.Id == res.Id).Sum(h => h.Quantity));
        //  var availableQuantity = hardware.TotalAmount - reservedQuantityForSameDayEvents;

        //  if (availableQuantity >= hardwareRequest.Quantity)
        //  {
        //    var newReservation = new Reservation
        //    {
        //      Id = eventReservationId,
        //      EventId = reservationRequest.EventId,
        //      HardwareId = hardwareRequest.HardwareId,
        //      Quantity = hardwareRequest.Quantity,
        //      EventName = myEventsInfo.EventName,
        //      HardwareName = hardware.Name,
        //      EventStartDate = myEventsInfo.EventStartDate,
        //      EventEndDate = myEventsInfo.EventEndDate,
        //    };

        //    eventReservations.Add(newReservation);
        //  }
        //}

        //if (eventReservations.Count > 0)
        //{
        //  _reservation.AddRange(eventReservations);
        //}

        //return eventReservations;



      }

      eventReservations.Add(newReservation);

      //  {
      //else
      //{
      //  var myEventsInfo = await _eventsForReservationService.GetEventById(reservation.EventId);
      //  var reserve = CanReserve(myEventsInfo.EventStartDate);

      //  if (reserve)
      //  {
      //    // Prüfen, dass es genug Hardware gibt
      //    var hardware = await _hardwareService.GetHardware(res.HardwareId);

      //    var reservedQuantityForSameDayEvents = _reservation
      //            .Where(r => r.HardwareId == res.HardwareId && r.EventStartDate.Date == myEventsInfo.EventStartDate.Date && r.EventEndDate.Date == myEventsInfo.EventEndDate.Date)
      //            .Sum(r => r.Quantity);

      //    var availableQuantity = hardware.TotalAmount - reservedQuantityForSameDayEvents;
      //    if (availableQuantity >= res.Quantity)
      //    {
      //      // Si aún no hemos creado una reserva para este evento, generamos un ID para la reserva
      //      if (eventReservationId == null)
      //      {
      //        eventReservationId = Guid.NewGuid(); // Generamos un único Guid para todo el evento
      //      }
      //      // Crear una nueva reserva para el evento (esto se hace solo una vez)
      //      var newReservation = new Reservation
      //      {
      //        Id = eventReservationId.Value, // Usamos el mismo ID generado para todas las reservas
      //        EventId = res.EventId,
      //        HardwareId = res.HardwareId,
      //        Quantity = res.Quantity,
      //        EventName = myEventsInfo.EventName,
      //        HardwareName = hardware.Name,
      //        EventStartDate = myEventsInfo.EventStartDate,
      //        EventEndDate = myEventsInfo.EventEndDate,
      //      };

      //      // Añadimos esta reserva a la lista de reservas del evento
      //      eventReservations.Add(newReservation);

      //    }
      //  }
      //}



      // Si no había reservas previas para el evento, guardamos todas las reservas de hardware para ese evento
      if (eventReservations.Count > 0)
      {
        _reservation.AddRange(eventReservations); // Se guardan todas las reservas de hardware para el evento
      }
      return _reservation;
    }
    public bool CanReserve(DateTime eventDate )
    {
      return (eventDate - DateTime.Now).TotalDays >= 29;
    }

  }
}



