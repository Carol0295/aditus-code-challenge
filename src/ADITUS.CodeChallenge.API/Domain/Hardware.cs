namespace ADITUS.CodeChallenge.API.Domain
{
  public record Hardware
  {
    public Guid Id { get; init; }
    public string Name { get; init; }
    public int TotalAmount { get; set; }
    
  }
}
