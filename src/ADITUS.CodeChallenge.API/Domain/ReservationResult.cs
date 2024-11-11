namespace ADITUS.CodeChallenge.API.Domain
{
  public record ReservationResult
  {
    public bool Success { get; set; }
    public string Message { get; set; }
    public List<Reservation> Reservations { get; set; }
  }
}
