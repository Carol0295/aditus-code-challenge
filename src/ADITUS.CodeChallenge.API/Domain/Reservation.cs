namespace ADITUS.CodeChallenge.API.Domain
{
  public class Reservation
  {
    public Guid Id { get; init; }
    public List<HardwareForReservation> HardwareList { get; set; }
    public Guid EventId { get; set; }
    public string EventName { get; set; }
    public DateTime EventStartDate { get; set; }
    public DateTime EventEndDate { get; set; }
  }
}




