using System.Collections.Generic;
using System.Linq;
using Timberborn.CharacterMovementSystem;
using Timberborn.Navigation;
using UnityEngine;

namespace ChooChoo
{
  public class ObjectFollower : ITrackFollower
  {
    private readonly INavigationService _navigationService;
    private readonly MovementAnimator _movementAnimator;
    private readonly Transform _transform;
    private readonly GameObject _train;
    private readonly List<PathCorner> _animatedPathCorners = new(100);
    private ITrackFollower _objectToFollowTrackFollower;
    private List<TrackRoute> _pathCorners = new();
    private int _currentCornerIndex;
    private int _nextSubCornerIndex;
    private Transform _objectToFollow;
    private float _minDistanceFromObject;
    private TrackSection _currentTrackSection;

    public int CurrentCornerIndex => _currentCornerIndex;
    
    

    // public List<TrackSection> OccupiedTrackSections
    // {
    //   get => _objectToFollowTrackFollower.OccupiedTrackSections;
    //   set { }
    // }

    public ObjectFollower(
      INavigationService navigationService,
      MovementAnimator movementAnimator,
      Transform transform,
      GameObject train)
    {
      _navigationService = navigationService;
      _movementAnimator = movementAnimator;
      _transform = transform;
      _train = train;
    }

    public void SetObjectToFollow(Transform objectToFollow, float minDistanceFromObject)
    {
      _objectToFollow = objectToFollow;
      _minDistanceFromObject = minDistanceFromObject;
    }
    public void SetNewPathRoutes(ITrackFollower trackFollower, List<TrackRoute> pathCorners)
    {
      _objectToFollowTrackFollower = trackFollower;
      _pathCorners = pathCorners;
      _currentCornerIndex = 1;
      _nextSubCornerIndex = 0;
    }

    public void MoveTowardsObject(float deltaTime, string animationName, float movementSpeed)
    {
      if (_objectToFollowTrackFollower == null || _pathCorners.Count == 0)
        return;
      
      _animatedPathCorners.Clear();
      float time = Time.time;
      _animatedPathCorners.Add(new PathCorner(_transform.position, time));
      float num = deltaTime;
      while (CanMoveCloserToObject()
             && num > 0.0 
             && !ReachedLastPathCorner()
             && _objectToFollowTrackFollower.CurrentCornerIndex >= _currentCornerIndex)
      {
        _nextSubCornerIndex = PeekNextSubCornerIndex();
        Vector3 position;
        (position, num) = MoveInDirection(_transform.position, _pathCorners[_currentCornerIndex].RouteCorners[_nextSubCornerIndex], movementSpeed, num);
        _transform.position = position;
        float timeInSeconds = time + deltaTime - num;
        _animatedPathCorners.Add(new PathCorner(position, timeInSeconds));
      }
      if (!ReachedLastPathCorner() && CanMoveCloserToObject())
        _animatedPathCorners.Add(new PathCorner(_pathCorners[_currentCornerIndex].RouteCorners[_nextSubCornerIndex], (float) ((double) time + deltaTime + 1.0)));
      _movementAnimator.AnimateMovementAlongPath(_animatedPathCorners, animationName, movementSpeed);
    }

    public void StopMoving()
    {
      _movementAnimator.StopAnimatingMovement();
    }

    private bool ReachedLastPathCorner() => _pathCorners.Count > 0 && _navigationService.InStoppingProximity(_pathCorners.Last().RouteCorners.Last(), _transform.position);

    private bool CanMoveCloserToObject() => Vector3.Distance(_transform.position, _objectToFollow.position) > _minDistanceFromObject;
    
    private bool LastOfSubCorners() => _nextSubCornerIndex >= _pathCorners[_currentCornerIndex].RouteCorners.Length - 1;

    private int PeekNextSubCornerIndex()
    {
      var flag1 = _currentCornerIndex + 1 >= _pathCorners.Count;
      var flag2 = _nextSubCornerIndex + 1 >= _pathCorners[_currentCornerIndex].RouteCorners.Length;
      var flag3 = !_navigationService.InStoppingProximity(_transform.position, _pathCorners[_currentCornerIndex].RouteCorners[_nextSubCornerIndex]);
      
      // Plugin.Log.LogWarning(flag1 + "     " + flag2 + "     " + flag3);

      if ((flag1 && flag2) || flag3)
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
      Vector3 normalized = (destination - position).normalized;
      if (normalized == Vector3.zero)
        return (destination, deltaTime);
      Vector3 movement = speed * deltaTime * normalized;
      float magnitude1 = movement.magnitude;
      Vector3 vector3 = ClampMovement(movement, magnitude1);
      float actualDistance = Vector3.Distance(position, destination);
      float magnitude2 = vector3.magnitude;
      if ((double) actualDistance >= (double) magnitude2)
      {
        float num = LeftTime(deltaTime, magnitude2, magnitude1);
        return (position + vector3, num);
      }
      float num1 = LeftTime(deltaTime, actualDistance, magnitude2);
      return (destination, num1);
    }

    private static Vector3 ClampMovement(Vector3 movement, float movementMagnitude) => movementMagnitude <= 0.05 ? movement : movement.normalized * 0.05f;

    private static float LeftTime(float deltaTime, float actualDistance, float maxDistance) => deltaTime * (float) (1.0 - (double) actualDistance / (double) maxDistance);
  }
}
