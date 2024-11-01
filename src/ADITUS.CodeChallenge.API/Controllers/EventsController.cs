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

    // Konstruktor der Klasse EventsController mit dem Interface IEventService als Parameter
    public EventsController(IEventService eventService, HttpClient httpClient)
    {
      _httpClient = httpClient;
      _eventService = eventService;
    }


    // Api-Endpunkt für alle Events - Codigo dado por ellos.
    /*[HttpGet]
    [Route("")]
    public async Task<IActionResult> GetEvents()
    {
      var events = await _eventService.GetEvents();
      return Ok(events);
    }
    */
    [HttpGet]
    [Route("")]
    public async Task<IActionResult> GetEvents()
    {
      var events = await _eventService.GetEvents();
      List<HybridEvent> StatisticsEvents = new List<HybridEvent>();
      object list = null;

      //var MyUrl = "https://codechallenge-statistics.azurewebsites.net/api/onsite-statistics/";
      foreach (var InfoEvent in events) // recorro mis eventos
      {

        var Resp = await GetEvent(InfoEvent.Id, InfoEvent.Type);
        // el Json que me devuelve GetEvent(id, type) lo meto junto a otra lista para juntarlo con los eventos.
        // Y sino directamente con vue prüfen que el evento sea de tipo XY y luego mostrar la estadistica.
        if (Resp is OkObjectResult okResult && okResult.Value is HybridEvent EventsData)
        {
          //list = new { Event = InfoEvent, DataEvents = EventsData };
          // Añadimos el objeto StatisticsEvents a la lista
          var statisticsEvent = new HybridEvent
          {
            Id = InfoEvent.Id, // Asigna un nuevo GUID
            OnSite = EventsData.OnSite, // Asigna la información del evento
            Online = EventsData.Online // Si es aplicable
          };
          StatisticsEvents.Add(statisticsEvent); // -> ESTO FUNCIONA
        }
      }
      return Ok(new { @events, StatisticsEvents });
    }

    // Api-Endpunkt für die Information eines Events
    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> GetEvent(Guid id, EventType type)
    {
      var @event = await _eventService.GetEvent(id, type);
      var HybridList = new HybridEvent();
      //Id = id,
      //EventName = @event.Name;
      //HttpResponseMessage Resp;
      HttpResponseMessage RespOnSite;
      HttpResponseMessage RespOnline;
      var MyUrl = "https://codechallenge-statistics.azurewebsites.net/api/";
      //var MyOnSiteUrl = "";
      //var MyOnlineUrl = "";
      if (@event == null)
      {
        return NotFound();
      }
      else
      {
        //if (type == EventType.OnSite) // Onsite
        //{
        //  MyUrl += "onsite-statistics/";
        //}

        //if(type == EventType.Online) // Online
        //{
        //  MyUrl += "online-statistics/";
        //}

        //Resp = await _httpClient.GetAsync(MyUrl + id);

        if (type == EventType.OnSite || type == EventType.Hybrid)  // Si es OnSite o Hybrid, obtenemos estadísticas OnSite
        {

          RespOnSite = await _httpClient.GetAsync(MyUrl + "onsite-statistics/" + id);
          if (RespOnSite.IsSuccessStatusCode)
          {
            var OnSiteJson = await RespOnSite.Content.ReadAsStringAsync();
            HybridList.OnSite = JsonConvert.DeserializeObject<OnSiteEvent>(OnSiteJson);
          }
        }

        if (type == EventType.Online || type == EventType.Hybrid)  // Si es OnSite o Hybrid, obtenemos estadísticas OnSite
        {
          RespOnline = await _httpClient.GetAsync(MyUrl + "online-statistics/" + id);
          if (RespOnline.IsSuccessStatusCode)
          {
            var OnSiteJson = await RespOnline.Content.ReadAsStringAsync();
            HybridList.Online = JsonConvert.DeserializeObject<OnlineEvent>(OnSiteJson);
          }
        }

        /*
        if ( type == EventType.Hybrid)  // Wenn Veranstaltung Hybrid dann beides abrufen und in Liste speichern
        {
          var HybridList = new HybridEvent();
          // Für die OnSite Veranstaltungen
          RespOnSite = await _httpClient.GetAsync(MyUrl + "onsite-statistics/" + id);
          if (RespOnSite.IsSuccessStatusCode)
          {
            //var OnSite = new OnSite();
            var OnSiteJson = await RespOnSite.Content.ReadAsStringAsync();
            HybridList.OnSite = JsonConvert.DeserializeObject<OnSite>(OnSiteJson);
            //OnSite = OnSiteData; // Asignamos las estadísticas OnSite
            //OnSite.Add(HybridList);
          }

          // Für die Online Veranstaltungen
          RespOnline = await _httpClient.GetAsync(MyUrl + "online-statistics/" + id);
          if (RespOnline.IsSuccessStatusCode)
          {
            //var Online = new StatisticsEvents();
            var OnlineJson = await RespOnline.Content.ReadAsStringAsync();
            HybridList.StatisticFromEvent = JsonConvert.DeserializeObject<StatisticsEvents>(OnlineJson);
            //Online = OnlineData; // Asignamos las estadísticas OnSite
            //Online.Add(HybridList);
          }
          return Ok(HybridList); // Paso la lista Online
        }
        
        if (Resp.IsSuccessStatusCode) // 
        {
          
          var jsonData = await Resp.Content.ReadAsStringAsync();
            if(type == EventType.OnSite)
            {
                var OnSite = new OnSite();
                OnSite = JsonConvert.DeserializeObject<OnSite>(jsonData);
                return Ok(OnSite); // Paso la lista Onsite
            }
            else if(type == EventType.Online){
                var Online = new StatisticsEvents();
                Online = JsonConvert.DeserializeObject<StatisticsEvents>(jsonData);
                return Ok(Online); // Paso la lista Online
            }*/

        //creo que esta parte ya no me hace falta ya que hybrid se realiza arriba
        //if ( type == EventType.Hybrid)
        //{
        //    HybridEvent Hybrid = new HybridEvent();
        //    Hybrid = JsonConvert.DeserializeObject<HybridEvent>(jsonData);
        //    return Ok(Hybrid); // Pensar bien como devuelvo las dos listas juntas o quizas hacer aca mismo json serialize
        //}

        //}


      }
      return Ok(HybridList);
      //return Ok(StatisticsEvents); // Mirar como paso el objeto final por el Ok
    }
  }
}