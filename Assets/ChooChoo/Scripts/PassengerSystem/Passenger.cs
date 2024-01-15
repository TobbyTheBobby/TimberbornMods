using System.Collections.Generic;
using Bindito.Core;
using ChooChoo.BeaverNavigationUtilities;
using Timberborn.CharacterModelSystem;
using Timberborn.CharacterMovementSystem;
using Timberborn.TickSystem;
using Timberborn.TimeSystem;
using UnityEngine;

namespace ChooChoo.PassengerSystem
{
    public class Passenger : TickableComponent
    {
        private PassengerStationLinkRepository _passengerStationLinkRepository;
        private IDayNightCycle _dayNightCycle;

        private PathFollowerUtilities _pathFollowerUtilities;
        private CharacterModel _characterModel;

        private float _waitUntilTimestamp;
        public PassengerStationLink PassengerStationLink { get; private set; }

        // private bool IsWaiting => (double) _waitUntilTimestamp > (double) _dayNightCycle.PartialDayNumber;
        public bool IsWaiting => PassengerStationLink != null;

        [Inject]
        public void InjectDependencies(PassengerStationLinkRepository passengerStationLinkPointService, IDayNightCycle dayNightCycle)
        {
            _passengerStationLinkRepository = passengerStationLinkPointService;
            _dayNightCycle = dayNightCycle;
        }

        private void Awake()
        {
            _pathFollowerUtilities = GetComponentFast<PathFollowerUtilities>();
            _characterModel = GetComponentFast<CharacterModel>();
        }

        public override void Tick()
        {
            _characterModel.Model.gameObject.SetActive(!IsWaiting);
            if (IsWaiting && !((double)_waitUntilTimestamp > (double)_dayNightCycle.PartialDayNumber))
            {
                ArrivedAtDestination();
            }
        }

        public void ArrivedAtDestination()
        {
            LeaveStation();
        }

        public bool ShouldWait(IReadOnlyList<Vector3> pathCorners, ref int nextCornerIndex, MovementAnimator movementAnimator)
        {
            return IsWaiting || CheckForPathLink(ref pathCorners, ref nextCornerIndex, movementAnimator);
        }

        private bool CheckForPathLink(ref IReadOnlyList<Vector3> pathCorners, ref int nextCornerIndex, MovementAnimator movementAnimator)
        {
            // Plugin.Log.LogInfo("index: " + nextCornerIndex + "  Count: " + pathCorners.Count);
            var startBlockObject = _pathFollowerUtilities.GetBlockObjectAtIndex(nextCornerIndex - 1);
            var endBlockObject = _pathFollowerUtilities.GetBlockObjectAtIndex(nextCornerIndex);

            // Plugin.Log.LogInfo(GameObjectFast.name);
            // Plugin.Log.LogInfo(TransformFast.position + "");
            // Plugin.Log.LogInfo(startBlockObject + " index: " + (nextCornerIndex - 1));
            // Plugin.Log.LogInfo(endBlockObject + " index: " + nextCornerIndex);

            if (
                startBlockObject == null ||
                endBlockObject == null ||
                !startBlockObject.TryGetComponentFast(out PassengerStation startPassengerStation) ||
                !endBlockObject.TryGetComponentFast(out PassengerStation endPassengerStation))
            {
                return false;
            }

            var passengerStationLink = _passengerStationLinkRepository.GetPathLink(startPassengerStation, endPassengerStation);
            if (passengerStationLink == null)
                return false;
            EnterStation(passengerStationLink);
            movementAnimator.StopAnimatingMovement();
            if (nextCornerIndex + 1 < pathCorners.Count)
            {
                TransformFast.position = passengerStationLink.EndLinkPoint.Location;
                ++nextCornerIndex;
            }
            else
            {
                TransformFast.position = pathCorners[nextCornerIndex];
                return false;
            }

            _waitUntilTimestamp = _dayNightCycle.DayNumberHoursFromNow(passengerStationLink.WaitingTimeInHours);
            return true;
        }

        private void EnterStation(PassengerStationLink passengerStationLink)
        {
            PassengerStationLink = passengerStationLink;
            passengerStationLink.StartLinkPoint.PassengerQueue.Add(this);
        }

        private void LeaveStation()
        {
            if (!IsWaiting)
                return;
            PassengerStationLink.StartLinkPoint.PassengerQueue.Remove(this);
            PassengerStationLink = null;
        }
    }
}