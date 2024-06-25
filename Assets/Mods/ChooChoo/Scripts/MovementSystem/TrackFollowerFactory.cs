using ChooChoo.TrackSystem;
using Timberborn.CharacterMovementSystem;
using Timberborn.Navigation;
using UnityEngine;

namespace ChooChoo.MovementSystem
{
    public class TrackFollowerFactory
    {
        private readonly INavigationService _navigationService;

        public TrackFollowerFactory(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public TrackFollower Create(GameObject owner)
        {
            return new TrackFollower(_navigationService, owner.GetComponent<MovementAnimator>(),
                owner.transform, owner.GetComponent<TrackSectionOccupier>());
        }
    }
}