﻿using Bindito.Core;
using ChooChoo.NavigationSystem;
using ChooChoo.TrackSystem;
using Timberborn.BlockSystem;
using Timberborn.Localization;
using Timberborn.SingletonSystem;
using Timberborn.StatusSystem;
using Timberborn.TickSystem;

namespace ChooChoo.NavigationSystemUI
{
    public class UnconnectedTrainDestinationStatus : TickableComponent, IFinishedStateListener
    {
        private static readonly string UnconnectedLocKey = "Tobbert.TrainDestination.UnconnectedWarning";
        private static readonly string UnconnectedShortLocKey = "Tobbert.TrainDestination.UnconnectedWarningShort";
        private TrainDestinationService _trainDestinationService;
        private EventBus _eventBus;
        private ILoc _loc;
        private TrainDestination _trainDestination;
        private StatusToggle _unconnectedTrainDestinationStatusToggle;
        private bool _isUnconnectedToAnyTrainDestination;
        private bool _checkForChanges;

        [Inject]
        public void InjectDependencies(TrainDestinationService trainDestinationService, EventBus eventBus, ILoc loc)
        {
            _trainDestinationService = trainDestinationService;
            _eventBus = eventBus;
            _loc = loc;
        }

        public void Awake()
        {
            _trainDestination = GetComponentFast<TrainDestination>();
            _unconnectedTrainDestinationStatusToggle = StatusToggle.CreateNormalStatusWithAlertAndFloatingIcon(
                "UnconnectedTrainDestinationStatus", _loc.T(UnconnectedLocKey), _loc.T(UnconnectedShortLocKey));
            enabled = false;
        }

        public override void StartTickable()
        {
            GetComponentFast<StatusSubject>().RegisterStatus(_unconnectedTrainDestinationStatusToggle);
            CheckIfConnectedToTrainDestination();
            UpdateStatusToggle();
        }

        public override void Tick()
        {
            if (!_checkForChanges)
                return;
            CheckIfConnectedToTrainDestination();
            _checkForChanges = false;
            UpdateStatusToggle();
        }

        public void OnEnterFinishedState()
        {
            enabled = true;
            _eventBus.Register(this);
        }

        public void OnExitFinishedState()
        {
            _eventBus.Unregister(this);
            enabled = false;
        }

        [OnEvent]
        public void OnTracksRecalculated(TracksRecalculatedEvent tracksRecalculatedEvent)
        {
            _checkForChanges = true;
        }

        private void UpdateStatusToggle()
        {
            if (_isUnconnectedToAnyTrainDestination)
                _unconnectedTrainDestinationStatusToggle.Activate();
            else
                _unconnectedTrainDestinationStatusToggle.Deactivate();
        }

        private void CheckIfConnectedToTrainDestination()
        {
            _isUnconnectedToAnyTrainDestination = _trainDestinationService.CanBeReachedTwoWay(_trainDestination);
        }
    }
}