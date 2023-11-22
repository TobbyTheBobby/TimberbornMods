namespace ChooChoo
{
  public class PassengerStationLink
  {
    public readonly PassengerStation StartLinkPoint;
    public readonly PassengerStation EndLinkPoint;
    public readonly float WaitingTimeInHours;

    public PassengerStationLink(
      PassengerStation startLinkPoint,
      PassengerStation endLinkPoint,
      float waitingTimeInHours)
    {
      StartLinkPoint = startLinkPoint;
      EndLinkPoint = endLinkPoint;
      WaitingTimeInHours = waitingTimeInHours;
    }

    public bool ValidLink() => StartLinkPoint != null && StartLinkPoint.enabled && EndLinkPoint != null && EndLinkPoint.enabled;
  }
}
