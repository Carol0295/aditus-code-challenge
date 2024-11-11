using Microsoft.AspNetCore.Mvc;
using ADITUS.CodeChallenge.API.Services;

namespace ADITUS.CodeChallenge.API
{
  [Route("hardware")]
  public class HardwareController : ControllerBase
  {
    private readonly IHardwareService _hardwareService;
    public HardwareController(IHardwareService hardwareService)
    {
      _hardwareService = hardwareService;
    }

    /* Funktion zum Abholen aller Informationen über die Hardware */
    [HttpGet]
    [Route("")]
    public async Task<IActionResult> GetAllHardware()
    {
      var hardware = await _hardwareService.GetAllHardware();
      return Ok(hardware);
    }
  }
}
