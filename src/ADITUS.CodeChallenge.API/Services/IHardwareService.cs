using ADITUS.CodeChallenge.API.Domain;

namespace ADITUS.CodeChallenge.API.Services
{
  public interface IHardwareService
  {
    Task<Hardware> GetHardware(Guid HardwareId);

    Task<IList<Hardware>> GetAllHardware();

  }
}
