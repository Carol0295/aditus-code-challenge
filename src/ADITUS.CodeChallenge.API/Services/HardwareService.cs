using ADITUS.CodeChallenge.API.Domain;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ADITUS.CodeChallenge.API.Services
{
  public class HardwareService : IHardwareService
  {
    private readonly IList<Hardware> _hardware;

    public HardwareService()
    {
      _hardware = new List<Hardware>
      {
        new Hardware
        {
          Id = Guid.Parse("74e2ff76-6f6e-48b4-aab6-b1529f866f62"),
          Name = "Drehsperre",
          TotalAmount = 20,
        },

        new Hardware{
          Id = Guid.Parse("06df0fb5-7d2b-4fac-b169-64db7a4fad8c"),
          Name = "Funkhandscanner",
          TotalAmount = 30,
        },

        new Hardware
        {
          Id = Guid.Parse("ff2810ae-c21c-4a5f-8b59-699afc9e8347"),
          Name = "Mobiles Scan-Terminal",
          TotalAmount = 20,
        }
      };
    }

    public Task<Hardware> GetHardware(Guid hardwareId)
    {
       var Hardware = _hardware.FirstOrDefault(h => h.Id == hardwareId);
       return Task.FromResult(Hardware);
    }

    public Task<IList<Hardware>> GetAllHardware()
    {
      return Task.FromResult(_hardware);
    }

  }
}
