//using System.Text.Json.Serialization;
//using ADITUS.CodeChallenge.API.Services;

//var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddControllers().AddJsonOptions(x =>
//{
//    x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
//});

//builder.Services.AddSingleton<IEventService, EventService>();

//var app = builder.Build();

//app.UseHttpsRedirection();

//app.UseAuthorization();

//app.MapControllers();

//app.Run();

using System.Text.Json.Serialization;
using ADITUS.CodeChallenge.API;
using ADITUS.CodeChallenge.API.Services;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(x =>
{
  x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddSingleton<IEventService, EventService>();

builder.Services.AddHttpClient<EventsController>();

// Agregar el servicio de CORS
builder.Services.AddCors(options =>
{

  options.AddPolicy("",
      policy =>
      {
        // Aquí puedes restringir los orígenes, métodos, y encabezados permitidos
        policy.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod()
                .WithOrigins("https://codechallenge-statistics.azurewebsites.net/api/")
                .AllowCredentials();

      });
});

var app = builder.Build();

app.UseHttpsRedirection();

app.UseStaticFiles(); // aniadido

app.UseRouting(); // aniadido

app.UseCors(""); // Für die website von code-challenge azure

app.UseAuthorization();

app.MapControllers();
// Redirige cualquier solicitud no manejada por un controlador a Vue.js
app.MapFallbackToFile("index.html");

app.Run();

