using System.Collections.Generic;
using System.Linq;
using ChooChoo.NavigationSystem;
using ChooChoo.TrackSystem;
using Timberborn.SingletonSystem;
using Timberborn.TickSystem;

namespace ChooChoo.PassengerSystem
{
    public class PassengerStationLinkRepository : IPostLoadableSingleton, ITickableSingleton
    {
        private readonly TrainDestinationConnectedRepository _trainDestinationConnectedRepository;
        private readonly TrainDestinationService _trainDestinationService;
        private readonly EventBus _eventBus;

        private readonly HashSet<PassengerStationLink> _pathLinks = new();
        private bool _tracksUpdated;

        private PassengerStationLinkRepository(TrainDestinationConnectedRepository trainDestinationConnectedRepository,
            TrainDestinationService trainDestinationService, EventBus eventBus)
        {
            _trainDestinationConnectedRepository = trainDestinationConnectedRepository;
            _trainDestinationService = trainDestinationService;
            _eventBus = eventBus;
        }

        public void PostLoad()
        {
            _eventBus.Register(this);
            _tracksUpdated = true;
        }

        public void Tick()
        {
            if (!_tracksUpdated)
                return;
            UpdateLinks();
            _tracksUpdated = false;
        }

        [OnEvent]
        public void OnTracksRecalculated(TracksRecalculatedEvent tracksRecalculatedEvent)
        {
            _tracksUpdated = true;
        }

        private void UpdateLinks()
        {
            // Plugin.Log.LogWarning("Updating PassengerLinks");
            _pathLinks.Clear();
            var trainDestinations = _trainDestinationConnectedRepository.TrainDestinations;
            foreach (var trainDestination in trainDestinations.Keys)
            {
                // Plugin.Log.LogInfo(trainDestination.GameObjectFast.name);
                if (!trainDestination.TryGetComponentFast(out PassengerStation passengerStation))
                    continue;
                var connectedTrainDestinations = trainDestinations[trainDestination];
                foreach (var connectedTrainDestination in connectedTrainDestinations)
                {
                    // Plugin.Log.LogError(connectedTrainDestination.GameObjectFast.name);
                    if (!connectedTrainDestination.TryGetComponentFast(out PassengerStation connectedPassengerStation))
                        continue;
                    if (passengerStation == connectedPassengerStation)
                        continue;
                    if (_trainDestinationService.TrainDestinationsConnectedBothWays(trainDestination, connectedTrainDestination))
                    {
                        // Plugin.Log.LogWarning("connecting");
                        passengerStation.Connect(connectedPassengerStation);
                    }
                }
            }

            _eventBus.Post(new OnConnectedPassengerStationsUpdated());
        }

        public void AddNew(PassengerStationLink passengerStationLink) => _pathLinks.Add(passengerStationLink);

        public PassengerStationLink GetPathLink(PassengerStation startPassengerStation, PassengerStation endPassengerStation)
        {
            return _pathLinks.FirstOrDefault(pathLink =>
                startPassengerStation == pathLink.StartLinkPoint && endPassengerStation == pathLink.EndLinkPoint);
        }

        public void RemoveInvalidLinks() => _pathLinks.RemoveWhere(link => !link.ValidLink());

        public IEnumerable<PassengerStationLink> PathLinks(PassengerStation a) => _pathLinks.Where(link => link.StartLinkPoint == a);

        public void RemoveLinks(PassengerStation a)
        {
            _pathLinks.RemoveWhere(link => link.StartLinkPoint == a || link.EndLinkPoint == a);
            _eventBus.Post(new OnConnectedPassengerStationsUpdated());
        }

        public bool AlreadyConnected(PassengerStation a, PassengerStation b)
        {
            if (!a.ConnectsTwoWay)
                return GetPathLink(a, b) != null;
            return GetPathLink(a, b) != null || GetPathLink(b, a) != null;
        }
    }
}