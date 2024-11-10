namespace ADITUS.CodeChallenge.API.Domain
{
  public record EventForReservation
  {
    public Guid EventId { get; set; }
    public String EventName  { get; set; }
    public DateTime EventStartDate { get; set; }
    public DateTime EventEndDate  { get; set; }
  }
}
