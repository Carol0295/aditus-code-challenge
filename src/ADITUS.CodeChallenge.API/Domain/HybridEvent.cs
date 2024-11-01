namespace ADITUS.CodeChallenge.API.Domain
{
  public class HybridEvent
  {
    public Guid Id { get; set; }
    public OnSiteEvent OnSite { get; set; }
    public OnlineEvent Online { get; set; }
  }
}
