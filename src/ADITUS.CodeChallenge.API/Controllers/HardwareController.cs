using Microsoft.AspNetCore.Mvc;
using ADITUS.CodeChallenge.API.Services;

namespace ADITUS.CodeChallenge.API
{
  [Route("hardware")]
  public class HardwareController : ControllerBase
  {
    private readonly IHardwareService _hardwareService;
    private readonly IEventsForReservationService _eventsForReservationService;

    public HardwareController(IHardwareService hardwareService, IEventsForReservationService eventsForReservationService)
    {
      _hardwareService = hardwareService;
      _eventsForReservationService = eventsForReservationService;
    }

    [HttpGet]
    [Route("")]
    public async Task<IActionResult> GetAllHardware()
    {
      var hardware = await _hardwareService.GetAllHardware();
      return Ok(hardware);
    }

    [HttpGet]
    [Route("{name}")]
    public async Task<IActionResult> GetHardware(Guid hardwareId)
    {
      var hardware = await _hardwareService.GetHardware(hardwareId);
      if (hardware == null)
      {
        return NotFound();
      }
      else
      {
        return Ok();
      }
    }

    [HttpGet]
    [Route("eventsForReservation")] 
    public async Task<IActionResult> GetAllEventsForReservation()
    {
      var eventsReservation = await _eventsForReservationService.GetAllEventsForReservation();
      return Ok(eventsReservation);
    }
  }
}
