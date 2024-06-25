using System.Collections.Generic;
using System.Linq;
using ChooChoo.TrackSystem;
using Timberborn.CharacterMovementSystem;
using Timberborn.Navigation;
using UnityEngine;

namespace ChooChoo.MovementSystem
{
    public class TrackFollower : ITrackFollower
    {
        private readonly INavigationService _navigationService;
        private readonly MovementAnimator _movementAnimator;
        private readonly Transform _transform;
        private readonly TrackSectionOccupier _trackSectionOccupier;
        private readonly List<PathCorner> _animatedPathCorners = new(100);
        private List<TrackRoute> _pathCorners;
        private int _pathCornersCount;
        private int _currentCornerIndex;
        private int _nextSubCornerIndex;
        private static readonly float ExtraPathCornersMultiplier = 2.5f;

        public int CurrentCornerIndex => _currentCornerIndex;

        public TrackFollower(
            INavigationService navigationService,
            MovementAnimator movementAnimator,
            Transform transform,
            TrackSectionOccupier trackSectionOccupier)
        {
            _navigationService = navigationService;
            _movementAnimator = movementAnimator;
            _transform = transform;
            _trackSectionOccupier = trackSectionOccupier;
        }

        public void StartMovingAlongPath(List<TrackRoute> pathCorners)
        {
            _pathCorners = pathCorners;
            _currentCornerIndex = 1;
            _nextSubCornerIndex = 0;
        }

        public void MoveAlongPath(float deltaTime, string animationName, float movementSpeed)
        {
            _animatedPathCorners.Clear();
            var time = Time.time;
            _animatedPathCorners.Add(new PathCorner(_transform.position, time));
            var num = deltaTime;
            while (num > 0.0 && !ReachedLastPathCorner())
            {
                if (!CanEnterNextSection())
                {
                    _animatedPathCorners.Clear();
                    return;
                }

                _nextSubCornerIndex = PeekNextSubCornerIndex();
                Vector3 position;
                (position, num) = MoveInDirection(_transform.position, _pathCorners[_currentCornerIndex].RouteCorners[_nextSubCornerIndex],
                    movementSpeed, num);
                _transform.position = position;
                var timeInSeconds = time + deltaTime - num;
                _animatedPathCorners.Add(new PathCorner(position, timeInSeconds));
            }

            if (!ReachedLastPathCorner())
                _animatedPathCorners.Add(new PathCorner(_pathCorners[_currentCornerIndex].RouteCorners[_nextSubCornerIndex],
                    (float)((double)time + deltaTime + 1.0)));
            _movementAnimator.AnimateMovementAlongPath(_animatedPathCorners, animationName, movementSpeed);
        }

        public void StopMoving()
        {
            _movementAnimator.StopAnimatingMovement();
        }

        public bool ReachedLastPathCorner()
        {
            return _navigationService.InStoppingProximity(_pathCorners.Last().RouteCorners.Last(), _transform.position);
        }

        private bool CanEnterNextSection()
        {
            var nextCornerToCheckIndex = -1;

            while (_currentCornerIndex + nextCornerToCheckIndex - 1 < _pathCorners.Count &&
                   _pathCorners.Count > _currentCornerIndex + nextCornerToCheckIndex)
            {
                // Plugin.Log.LogInfo("Count: " + _pathCorners.Count + "   " + _currentCornerIndex + "   "+ nextCornerToCheckIndex);
                var trackPiece = _pathCorners[_currentCornerIndex + nextCornerToCheckIndex].Exit.ConnectedTrackPiece;
                nextCornerToCheckIndex += 1;

                if (trackPiece == null)
                    return false;

                if (trackPiece._dividesSection && trackPiece != _pathCorners[^2].Exit.ConnectedTrackPiece)
                    continue;

                var trackSection = trackPiece.TrackSection;

                var flag = trackSection.Equals(_trackSectionOccupier.TrackSection);
                if (flag)
                    return true;

                if (trackSection.Occupied)
                {
                    if (trackSection.HasWaitingTrain)
                    {
                        // _machinist.RefreshPath();
                        return false;
                    }

                    trackSection.HasWaitingTrain = true;
                    return false;
                }

                _trackSectionOccupier.OccupyNextTrackSection(trackSection);
                return true;
            }

            return true;
        }

        private bool LastOfSubCorners()
        {
            return _nextSubCornerIndex >= _pathCorners[_currentCornerIndex].RouteCorners.Length - 1;
        }

        private int PeekNextSubCornerIndex()
        {
            if ((_currentCornerIndex + 1 >= _pathCorners.Count && _nextSubCornerIndex + 1 >= _pathCorners[_currentCornerIndex].RouteCorners.Length) ||
                !_navigationService.InStoppingProximity(_transform.position, _pathCorners[_currentCornerIndex].RouteCorners[_nextSubCornerIndex]))
            {
                return _nextSubCornerIndex;
            }
            else
            {
                if (LastOfSubCorners())
                {
                    _currentCornerIndex += 1;
                    _nextSubCornerIndex = -1;
                }

                return _nextSubCornerIndex + 1;
            }
        }

        private static (Vector3 position, float leftTime) MoveInDirection(
            Vector3 position,
            Vector3 destination,
            float speed,
            float deltaTime)
        {
            var normalized = (destination - position).normalized;
            if (normalized == Vector3.zero)
                return (destination, deltaTime);
            var movement = speed * deltaTime * normalized;
            var magnitude1 = movement.magnitude;
            var vector3 = ClampMovement(movement, magnitude1);
            var actualDistance = Vector3.Distance(position, destination);
            var magnitude2 = vector3.magnitude;
            if (actualDistance >= magnitude2)
            {
                var num = LeftTime(deltaTime, magnitude2, magnitude1);
                return (position + vector3, num);
            }

            var num1 = LeftTime(deltaTime, actualDistance, magnitude2);
            return (destination, num1);
        }

        private static Vector3 ClampMovement(Vector3 movement, float movementMagnitude)
        {
            return movementMagnitude <= 0.1 / ExtraPathCornersMultiplier ? movement : movement.normalized * 0.1f / ExtraPathCornersMultiplier;
        }

        private static float LeftTime(float deltaTime, float actualDistance, float maxDistance)
        {
            return deltaTime * (float)(1.0 - actualDistance / maxDistance);
        }
    }
}