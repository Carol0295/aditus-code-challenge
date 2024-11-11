using Microsoft.AspNetCore.Mvc;
using ADITUS.CodeChallenge.API.Services;
using ADITUS.CodeChallenge.API.Domain;
using Newtonsoft.Json;
using System.Globalization;

namespace ADITUS.CodeChallenge.API
{
  [Route("reservation")]
  public class ReservationController : ControllerBase
  {
    private readonly IReservationService _reservationService;
    private readonly IEventsForReservationService _eventsForReservationService;

    public ReservationController(IReservationService reservationService, IEventsForReservationService eventsForReservationService)
    {
      _reservationService = reservationService;
      _eventsForReservationService = eventsForReservationService;
    }
    /* Funkntion zum Abholen aller Reservierungen in Memory*/
    [HttpGet]
    [Route("allReservations")]
    public async Task<IActionResult> GetAllReservations()
    {
      var reservations = await _reservationService.GetAllReservations();
      return Ok(reservations);
    }

    /* Funktion zum Senden zum Bearbeitung nach der Service von der zu bearabeitenden Reservierungsinformationen*/
    [HttpPost]
    [Route("")]
    public async Task<IActionResult> CreateReservation([FromBody] Reservation hardwareReservation)
    {
      if (hardwareReservation == null || hardwareReservation.HardwareList == null || !hardwareReservation.HardwareList.Any() ) 
      {
        return StatusCode(500, new { Success = false, message = "Bitte Daten prüfen." });
      }

      try
      {
        var successReservations = await _reservationService.AddReservation(hardwareReservation);

        if (successReservations.Success == false )
        {
          return StatusCode(500, new { Success = false, message = successReservations.Message });
        }

        return Ok(new { Success = true, Message = "Die Reservierung wurde gemacht, eine Freigabe steht aus. Bitte aktuelle Reservierungen Seite ansehen.", reservations = successReservations });

      } catch (Exception ex) {
        
        return BadRequest(new { Success = false, message = ex.Message });
      }
      
    }


    /* Funktion zum zurückgeben alle Fiktive Events für das Select in dem HTML */
    [HttpGet]
    [Route("eventsForReservation")]
    public async Task<IActionResult> GetAllEventsForReservation()
    {
      var eventsReservation = await _eventsForReservationService.GetAllEventsForReservation();
      return Ok(eventsReservation);
    }
  }
}
