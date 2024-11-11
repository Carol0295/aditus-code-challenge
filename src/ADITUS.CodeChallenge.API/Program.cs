using System.Text.Json.Serialization;
using ADITUS.CodeChallenge.API;
using ADITUS.CodeChallenge.API.Services;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(x =>
{
  x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddSingleton<IEventService, EventService>();
builder.Services.AddSingleton<IHardwareService, HardwareService>();
builder.Services.AddSingleton<IReservationService, ReservationService>();
builder.Services.AddSingleton<IEventsForReservationService, EventsForReservationService>();

builder.Services.AddHttpClient<EventsController>();

// Hinzufügen des CORS Service
builder.Services.AddCors(options =>
{
  options.AddPolicy("AllowLocalhost",
      policy =>
      {
        policy.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod()
                .WithOrigins("https://codechallenge-statistics.azurewebsites.net/api/", "http://localhost:63179", "http://localhost:7087")
                .AllowCredentials();

      });
});

var app = builder.Build();

app.UseHttpsRedirection();

app.UseStaticFiles(); 

app.UseRouting(); 

app.UseCors("AllowLocalhost"); 

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("index.html");

app.Run();

