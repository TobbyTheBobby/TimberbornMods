using Timberborn.CharacterMovementSystem;
using Timberborn.Navigation;
using UnityEngine;

namespace  ChooChoo
{
  public class TrackFollowerFactory
  {
    private readonly INavigationService _navigationService;
    private readonly TrainNavigationService _trainNavigationService;
    // private readonly TrackSectionService _trackSectionService;

    public TrackFollowerFactory(
      INavigationService navigationService, 
      TrainNavigationService trainNavigationService
      // TrackSectionService trackSectionService
      ) 
    {
      _navigationService = navigationService;
      _trainNavigationService = trainNavigationService;
      // _trackSectionService = trackSectionService;
    }

    public TrackFollower Create(GameObject owner) => new(_navigationService, _trainNavigationService, owner.GetComponent<MovementAnimator>(), owner.transform, owner.GetComponent<Machinist>(), owner.GetComponent<TrackSectionOccupier>());
  }
}
