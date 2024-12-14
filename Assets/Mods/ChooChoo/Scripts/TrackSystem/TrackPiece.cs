using System.Collections.Generic;
using System.Linq;
using Bindito.Core;
using ChooChoo.WaitingSystem;
using Timberborn.BaseComponentSystem;
using Timberborn.BlockSystem;
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
            TrackArrayProvider trackArrayProvider)
        {
            _blockService = blockService;
            EventBus = eventBus;
            _trackArrayProvider = trackArrayProvider;
        }

        public TrackRoute[] SpecificationTrackRoutes => _trackArrayProvider.GetConnections(GameObjectFast.name);
        
        public virtual TrackRoute[] TrackRoutes
        {
            get
            {
                if (_trackRoutes != null)
                    return _trackRoutes;
                var trackRoutes = SpecificationTrackRoutes;
                foreach (var trackRoute in trackRoutes)
                {
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
                //     Plugin.Log.LogInfo(trackConnection.Coordinates + "   " + trackConnection.Direction);
                _positionedTrackConnections = UniqueTrackConnections.Select((trackConnection) =>
                {
                    trackConnection.Coordinates = _blockObject.FlipMode.Transform(trackConnection.Coordinates, _blockObject.Blocks.Size.x);
                    return PositionedTrackConnection.From(trackConnection, _blockObject.Coordinates, _blockObject.Orientation);
                }).ToArray();

                return _positionedTrackConnections;
            }
        }

        public TrackConnection[] UniqueTrackConnections
        {
            get
            {
                var entrances = TrackRoutes.Select(route => route.Entrance);
                var exits = TrackRoutes.Select(route => route.Exit);
                var trackConnections = entrances.Concat(exits);
                return trackConnections.GroupBy(connection => connection.Direction).SelectMany(directionGroup => directionGroup.GroupBy(connection => connection.Coordinates).Select(group => group.First())).ToArray();
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
            // var stopwatch = Stopwatch.StartNew();
            
            enabled = true;
            CenterCoordinates = GetComponentFast<BlockObjectCenter>().WorldCenterGrounded;
            EventBus.Post(new TracksUpdatedEvent());
            
            // stopwatch.Stop();
            // Debug.LogWarning("ElapsedTicks: " + stopwatch.ElapsedTicks + " (10.000 Ticks = 1ms)");
        }

        public void OnExitFinishedState()
        {
            enabled = false;
            EventBus.Post(new TracksUpdatedEvent());
        }

        public void ResetTrackPiece()
        {
            TrackSection = new TrackSection(this);
            _trackRoutes = null;
            _positionedTrackConnections = null;
        }

        public void LookForTrackSection()
        {
            if (!this) return;

            foreach (var directionalTrackRoute in TrackRoutes)
            {
                if (!TrackConnectionCanConnect(directionalTrackRoute.Exit, out var trackPiece))
                    continue;
                var myTrackRouteEntrances = CheckAndGetEntrances(trackPiece).ToArray();
                var myTrackRouteExits = CheckAndGetExits(trackPiece).ToArray();
                var otherTrackRoutesEntrances = trackPiece.CheckAndGetEntrances(this).ToArray();
                MakeConnection(trackPiece, myTrackRouteEntrances, myTrackRouteExits, otherTrackRoutesEntrances);
            }

            foreach (var directionalTrackRoute in TrackRoutes)
            {
                if (!TrackConnectionCanConnect(directionalTrackRoute.Entrance, out var trackPiece))
                    continue;
                var myTrackRouteEntrances = CheckAndGetEntrances(trackPiece).ToArray();
                var myTrackRouteExits = CheckAndGetExits(trackPiece).ToArray();
                var otherTrackRoutesEntrances = trackPiece.CheckAndGetEntrances(this).ToArray();
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

        private bool TrackConnectionCanConnect(TrackConnection trackConnection, out TrackPiece trackPiece)
        {
            trackPiece = null;
            // Plugin.Log.LogError(_blockObject.Transform(trackConnection.Coordinates - trackConnection.Direction.ToOffset()) + " Entrance");
            trackPiece = _blockService.GetObjectsWithComponentAt<TrackPiece>(_blockObject.Transform(trackConnection.Coordinates - trackConnection.Direction.ToOffset())).FirstOrDefault();
            return trackPiece;
        }
    }
}