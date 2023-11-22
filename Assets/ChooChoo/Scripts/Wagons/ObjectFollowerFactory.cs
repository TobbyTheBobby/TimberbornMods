using Timberborn.CharacterMovementSystem;
using Timberborn.Navigation;
using UnityEngine;

namespace  ChooChoo
{
  public class ObjectFollowerFactory
  {
    private readonly INavigationService _navigationService;

    public ObjectFollowerFactory(INavigationService navigationService) 
    {
      _navigationService = navigationService;
    }

    public ObjectFollower Create(GameObject owner) => new(_navigationService, owner.GetComponent<MovementAnimator>(), owner.transform, owner);
  }
}
