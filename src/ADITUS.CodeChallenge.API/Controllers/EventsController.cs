using System.Net.Http;
using ADITUS.CodeChallenge.API.Domain;
using ADITUS.CodeChallenge.API.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ADITUS.CodeChallenge.API
{
  [Route("events")]
  public class EventsController : ControllerBase
  {
    private readonly IEventService _eventService;
    private readonly HttpClient _httpClient;

    // Konstruktor der Klasse EventsController mit dem Interface IEventService und HttpClient als Parameter
    public EventsController(IEventService eventService, HttpClient httpClient)
    {
      _httpClient = httpClient;
      _eventService = eventService;
    }

    [HttpGet]
    [Route("")]
    public async Task<IActionResult> GetEvents()
    {
      var events = await _eventService.GetEvents();
      var StatisticsEvents = await _eventService.GetEventsWithStatistics();

      return Ok(new { @events, StatisticsEvents });
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> GetEvent(Guid id)
    {
      var @event = await _eventService.GetEvent(id);
      if (@event == null)
      {
        return NotFound();
      }

      return Ok(@event);
    }
  }
}