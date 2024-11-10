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

    [HttpGet]
    [Route("allReservations")]
    public async Task<IActionResult> GetAllReservations()
    {
      var reservations = await _reservationService.GetAllReservations();
      return Ok(reservations);
    }

    [HttpGet]
    [Route("{id}")]
    //Liefert die Informationen über eine Reservierung
    public async Task<IActionResult> GetReservation(Guid id)
    {
      var reservation = await _reservationService.GetReservation(id);
      if (reservation == null)
      {
        return NotFound();
      }

      return Ok(reservation);
    }

    [HttpPost]
    [Route("")]
    public async Task<IActionResult> CreateReservation([FromBody] Reservation hardwareReservation)
    {
      if (hardwareReservation == null || hardwareReservation.HardwareList == null || !hardwareReservation.HardwareList.Any() ) 
      {
        return BadRequest("Invalid reservation data.");
      }
      
      var successReservations = await _reservationService.AddReservation(hardwareReservation);

      if (successReservations != null)
      {
        return Ok(successReservations);
      }
      else
      {
        return StatusCode(500, "Error beim Bearbeitung der Reservierung");
      }
    }

    [HttpGet]
    [Route("")] // mirar si cambio la ruta
    public async Task<IActionResult> GetAllEventsForReservation()
    {
      var eventsReservation = await _eventsForReservationService.GetAllEventsForReservation();
      return Ok(eventsReservation);
    }
  }
}
