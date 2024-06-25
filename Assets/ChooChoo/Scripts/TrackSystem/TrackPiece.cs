using System.Collections.Generic;
using System.Linq;
using Bindito.Core;
using ChooChoo.NavigationSystem;
using ChooChoo.WaitingSystem;
using Timberborn.BaseComponentSystem;
using Timberborn.BlockSystem;
using Timberborn.ConstructibleSystem;
using Timberborn.Coordinates;
using Timberborn.SingletonSystem;
using UnityEngine;

namespace ChooChoo.TrackSystem
{
    public class TrackPiece : BaseComponent, IFinishedStateListener, IPostTransformChangeListener
    {
        [SerializeField]
        private int _trackDistance;
        [SerializeField]
        public bool _dividesSection;

        protected EventBus EventBus;

        private BlockService _blockService;
        private TrackArrayProvider _trackArrayProvider;
        private TrackRouteWeightCache _trackRouteWeightCache;
        
        private BlockObject _blockObject;
        private BlockObjectCenter _blockObjectCenter;

        private TrackRoute[] _trackRoutes;
        private PositionedTrackConnection[] _positionedTrackConnections;

        public TrackSection TrackSection;

        public Vector3 CenterCoordinates { get; private set; }
        public bool CanPathFindOverIt { get; private set; }
        public int TrackDistance => _trackDistance;

        [Inject]
        public void InjectDependencies(
            BlockService blockService,
            EventBus eventBus,
            TrackArrayProvider trackArrayProvider,
            TrackRouteWeightCache trackRouteWeightCache)
        {
            _blockService = blockService;
            EventBus = eventBus;
            _trackArrayProvider = trackArrayProvider;
            _trackRouteWeightCache = trackRouteWeightCache;
        }

        public virtual TrackRoute[] TrackRoutes
        {
            get
            {
                if (_trackRoutes != null && Application.isPlaying)
                    return _trackRoutes;
                var trackRoutes = _trackArrayProvider.GetConnections(GameObjectFast.name);
                foreach (var trackRoute in trackRoutes)
                {
                    _trackRouteWeightCache.Add(trackRoute);
                    var position = _blockObjectCenter.WorldCenterGrounded;
                    trackRoute.RouteCorners = trackRoute.RouteCorners.Select(vector3 =>
                    {
                        var orientation = _blockObject.FlipMode.IsFlipped &&
                                          _blockObject.Orientation is Orientation.Cw90 or Orientation.Cw270
                            ? _blockObject.Orientation.Flip()
                            : _blockObject.Orientation;

                        return _blockObject.FlipMode.Transform(orientation.TransformInWorldSpace(vector3), 0) + position;
                    }).ToArray();
                }

                _trackRoutes = trackRoutes;
                return _trackRoutes;
            }
        }

        public PositionedTrackConnection[] PositionedTrackConnections
        {
            get
            {
                if (_positionedTrackConnections != null)
                    return _positionedTrackConnections;
                var entrances = TrackRoutes.Select(route => route.Entrance);
                var exits = TrackRoutes.Select(route => route.Exit);
                var trackConnections = entrances.Concat(exits);
                var uniqueTrackConnections = trackConnections.GroupBy(connection => connection.Direction).SelectMany(directionGroup => directionGroup.GroupBy(connection => connection.Coordinates).Select(group => group.First())).ToArray();
                // foreach (var trackConnection in uniqueTrackConnections)
                //     Plugin.Log.LogInfo(trackConnection.Coordinates + "   " + trackConnection.Direction);
                _positionedTrackConnections = uniqueTrackConnections.Select((trackConnection) =>
                {
                    trackConnection.Coordinates = _blockObject.FlipMode.Transform(trackConnection.Coordinates, _blockObject.Blocks.Size.x);
                    return PositionedTrackConnection.From(trackConnection, _blockObject.Coordinates, _blockObject.Orientation);
                }).ToArray();

                return _positionedTrackConnections;
            }
        }

        public void Awake()
        {
            TrackSection = new TrackSection(this);
            _blockObject = GetComponentFast<BlockObject>();
            _blockObjectCenter = GetComponentFast<BlockObjectCenter>();
            CanPathFindOverIt = !TryGetComponentFast(out TrainWaitingLocation _);
        }

        public void OnPostTransformChanged()
        {
            ResetTrackPiece();
        }

        public void OnEnterFinishedState()
        {
            enabled = true;
            LookForTrackSection();
            RefreshConnectedTrackPieces();
            CenterCoordinates = GetComponentFast<BlockObjectCenter>().WorldCenterGrounded;
            EventBus.Post(new OnTracksUpdatedEvent());

            // foreach (var track in TrackSection.TrackPieces)
            // {
            //     if (track != null)
            //     {
            //         Plugin.Log.LogInfo(track.transform.position.ToString());
            //     }
            //     else
            //     {
            //         Plugin.Log.LogWarning("");
            //     }
            // }


            // foreach (var trackRoute in TrackRoutes)
            // {
            //     if (trackRoute.Exit.ConnectedTrackRoutes == null)
            //         continue;
            //
            //     Plugin.Log.LogWarning($"{trackRoute.Exit.Coordinates} {trackRoute.Exit.Direction}: {trackRoute.Exit.ConnectedTrackRoutes.Length}");
            //
            //     foreach (var trackRoute1 in trackRoute.Exit.ConnectedTrackRoutes)
            //     {
            //         if (trackRoute1.Exit.ConnectedTrackPiece != null)
            //         {
            //             Plugin.Log.LogInfo(trackRoute1.Exit.ConnectedTrackPiece.GetComponent<BlockObjectCenter>().WorldCenterGrounded + "");
            //         }
            //         else
            //         {
            //             Plugin.Log.LogInfo("");
            //         }
            //     }
            // }
        }

        public void OnExitFinishedState()
        {
            enabled = false;
            TrackSection.Dissolve(this);
            EventBus.Post(new OnTracksUpdatedEvent());
        }

        public void ResetTrackPiece()
        {
            _trackRouteWeightCache.Remove(TrackRoutes);
            TrackSection = new TrackSection(this);
            _trackRoutes = null;
            _positionedTrackConnections = null;
        }

        public void LookForTrackSection()
        {
            if (!this)
                return;

            foreach (var directionalTrackRoute in TrackRoutes)
            {
                // Plugin.Log.LogWarning(directionalTrackRoute.Exit.Direction.ToString());
                // _trackConnectionService.CanConnectInDirection(trackConnection.Coordinates, trackConnection.Direction);
                // Plugin.Log.LogInfo("Offset " + trackConnection.Coordinates);
                // Plugin.Log.LogInfo("Direciton offset " + trackConnection.Direction.ToOffset());
                // Plugin.Log.LogInfo("Together " + (trackConnection.Coordinates - trackConnection.Direction.ToOffset()));
                // Plugin.Log.LogInfo("Final" + _blockObject.Transform(trackConnection.Coordinates - trackConnection.Direction.ToOffset()));

                if (!TrackConnectionCanConnect(directionalTrackRoute.Exit, out var trackPiece)) 
                    continue;
                var myTrackRouteEntrances = CheckAndGetEntrances(trackPiece).ToArray();
                var myTrackRouteExits = CheckAndGetExits(trackPiece).ToArray();

                // var connection = GetConnection(trackPiece, trackConnection.Direction);
                var otherTrackRoutesEntrances = trackPiece.CheckAndGetEntrances(this).ToArray();
                // Plugin.Log.LogInfo(obj.Coordinates.ToString());
                // Plugin.Log.LogError(
                //     "My Entrances: " + myTrackRouteEntrances.Length + 
                //     " My Exits: " + myTrackRouteExits.Length + 
                //     " Other Entrances: " + otherTrackRoutesEntrances.Length);
                // if (myTrackRouteEntrances.Length < 1 || myTrackRouteExits.Length < 1 || otherTrackRoutesEntrances.Length < 1 || otherTrackRoutesExits.Length < 1)
                // if ((myTrackRouteEntrances.Length < 1 && otherTrackRoutesExits.Length) < 1 || (otherTrackRoutesEntrances.Length < 1 && myTrackRouteExits.Length < 1))
                //     continue;
                if (myTrackRouteExits.Length < 1 || otherTrackRoutesEntrances.Length < 1)
                {
                    continue;
                }

                MakeConnection(trackPiece, myTrackRouteEntrances, myTrackRouteExits, otherTrackRoutesEntrances);
            }

            foreach (var directionalTrackRoute in TrackRoutes)
            {
                // Plugin.Log.LogWarning(directionalTrackRoute.Exit.Direction.ToString());
                // _trackConnectionService.CanConnectInDirection(trackConnection.Coordinates, trackConnection.Direction);
                // Plugin.Log.LogInfo("Offset " + trackConnection.Coordinates);
                // Plugin.Log.LogInfo("Direciton offset " + trackConnection.Direction.ToOffset());
                // Plugin.Log.LogInfo("Together " + (trackConnection.Coordinates - trackConnection.Direction.ToOffset()));
                // Plugin.Log.LogInfo("Final" + _blockObject.Transform(trackConnection.Coordinates - trackConnection.Direction.ToOffset()));
            
                if (!TrackConnectionCanConnect(directionalTrackRoute.Entrance, out var trackPiece)) 
                    continue;
                var myTrackRouteEntrances = CheckAndGetEntrances(trackPiece).ToArray();
                var myTrackRouteExits = CheckAndGetExits(trackPiece).ToArray();
            
                // var connection = GetConnection(trackPiece, trackConnection.Direction);
                var otherTrackRoutesEntrances = trackPiece.CheckAndGetEntrances(this).ToArray();
                // Plugin.Log.LogInfo(obj.Coordinates.ToString());
                // Plugin.Log.LogWarning(
                //     "My Entrances: " + myTrackRouteEntrances.Length + 
                //     " My Exits: " + myTrackRouteExits.Length + 
                //     " Other Entrances: " + otherTrackRoutesEntrances.Length);
                // if (myTrackRouteEntrances.Length < 1 || myTrackRouteExits.Length < 1 || otherTrackRoutesEntrances.Length < 1 || otherTrackRoutesExits.Length < 1)
                if (myTrackRouteEntrances.Length < 1 || otherTrackRoutesEntrances.Length < 1)
                {
                    continue;
                }
            
                MakeConnection(trackPiece, myTrackRouteEntrances, myTrackRouteExits, otherTrackRoutesEntrances);
            }
        }

        private IEnumerable<TrackRoute> CheckAndGetEntrances(TrackPiece previousTrackPiece)
        {
            foreach (var trackRoute in TrackRoutes)
            {
                if (!TrackConnectionCanConnect(trackRoute.Entrance, out var trackPiece)) 
                    continue;
                if (trackPiece == previousTrackPiece)
                    yield return trackRoute;
            }
        }

        private IEnumerable<TrackRoute> CheckAndGetExits(TrackPiece previousTrackPiece)
        {
            foreach (var trackRoute in TrackRoutes)
            {
                if (!TrackConnectionCanConnect(trackRoute.Exit, out var trackPiece)) 
                    continue;
                if (trackPiece == previousTrackPiece)
                    yield return trackRoute;
            }
        }

        private void MakeConnection(TrackPiece trackPiece, TrackRoute[] myTrackRouteEntrances, TrackRoute[] myTrackRouteExits, TrackRoute[] otherTrackRoutesEntrances)
        {
            // Plugin.Log.LogWarning("Connecting");

            foreach (var trackRoute in myTrackRouteExits)
            {
                trackRoute.Exit.ConnectedTrackPiece = trackPiece;
                trackRoute.Exit.ConnectedTrackRoutes = otherTrackRoutesEntrances;
            }

            foreach (var trackRoute in myTrackRouteEntrances)
            {
                trackRoute.Entrance.ConnectedTrackPiece = trackPiece;
                // trackRoute.Entrance.ConnectedTrackRoutes = otherTrackRoutesExits;
            }

            var flag4 = TryGetComponentFast(out TrainWaitingLocation _);
            var flag3 = trackPiece.TryGetComponentFast(out TrainWaitingLocation _);

            if (flag3 || flag4)
            {
                return;
            }

            var flag1 = _dividesSection;
            var flag2 = trackPiece._dividesSection;

            if (flag1 || flag2)
            {
                if (flag1)
                    trackPiece.TrackSection.Add(this);
                if (flag2)
                    TrackSection.Add(trackPiece);
                return;
            }

            if (trackPiece.TrackSection != TrackSection)
                trackPiece.TrackSection.Merge(TrackSection);
        }

        private void RefreshConnectedTrackPieces()
        {
            foreach (var trackConnection in TrackRoutes.SelectMany(route => new List<TrackConnection> { route.Entrance, route.Exit }))
            {
                if (TrackConnectionCanConnect(trackConnection, out var trackPiece))
                {
                    trackPiece.LookForTrackSection();
                }
            }
        }

        private bool TrackConnectionCanConnect(TrackConnection trackConnection, out TrackPiece trackPiece)
        {
            trackPiece = null;
            // Plugin.Log.LogError(_blockObject.Transform(trackConnection.Coordinates - trackConnection.Direction.ToOffset()) + " Entrance");
            var floorObjectAt = _blockService.GetFloorObjectAt(_blockObject.Transform(trackConnection.Coordinates - trackConnection.Direction.ToOffset()));
            if (!floorObjectAt || !floorObjectAt.TryGetComponentFast(out TrackPiece potentialTrackPiece))
                return false;
            trackPiece = potentialTrackPiece;
            return true;
        }
    }
}