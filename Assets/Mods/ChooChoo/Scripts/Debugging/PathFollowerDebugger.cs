using System.Collections.Generic;
using System.Linq;
using Bindito.Core;
using ChooChoo.MovementSystem;
using Timberborn.CharacterModelSystem;
using Timberborn.Common;
using Timberborn.Debugging;
using Timberborn.SelectionSystem;
using Timberborn.SingletonSystem;
using UnityEngine;

namespace ChooChoo.Debugging
{
    public class PathFollowerDebugger : ILoadableSingleton, ILateUpdatableSingleton
    {
        private EventBus _eventBus;
        private DebugModeManager _debugModeManager;
        private bool _walkerSelected;
        private Machinist _machinist;
        private CharacterModel _characterModel;
        private readonly List<GameObject> _cornerMarkers = new();
        private Vector3? _destination;

        [Inject]
        public void InjectDependencies(EventBus eventBus, DebugModeManager debugModeManager)
        {
            _eventBus = eventBus;
            _debugModeManager = debugModeManager;
        }

        public void Load()
        {
            _eventBus.Register(this);
        }

        public void LateUpdateSingleton()
        {
            if (_debugModeManager.Enabled && _walkerSelected)
                UpdateMarkers();
            else
                HideMarkers();
        }

        [OnEvent]
        public void OnSelectableObjectSelected(SelectableObjectSelectedEvent selectableObjectSelectedEvent)
        {
            var componentFast = selectableObjectSelectedEvent.SelectableObject.GetComponentFast<Machinist>();
            if (!componentFast)
                return;
            UpdateSelectedWalker(componentFast);
        }

        [OnEvent]
        public void OnSelectableObjectUnselected(SelectableObjectUnselectedEvent selectableObjectUnselectedEvent)
        {
            HideMarkers();
            _walkerSelected = false;
        }

        private Vector3 Destination => _machinist.PathCorners.IsEmpty()
            ? _machinist.TransformFast.position
            : _machinist.PathCorners.Last().RouteCorners.Last();

        private void UpdateSelectedWalker(Machinist machinist)
        {
            _machinist = machinist;
            _characterModel = _machinist.GetComponentFast<CharacterModel>();
            _walkerSelected = true;
        }

        private void UpdateMarkers()
        {
            UpdateMarker();
            if (!PathMarkersStale())
                return;
            ResetPathMarkers();
        }

        private void UpdateMarker()
        {
            var transformFast = _machinist.TransformFast;
            DebuggingMarkers.WalkerGameObjectMarker.transform.SetPositionAndRotation(transformFast.position, transformFast.rotation);
            DebuggingMarkers.WalkerGameObjectMarker.SetActive(true);
            DebuggingMarkers.WalkerModelMarker.transform.SetPositionAndRotation(_characterModel.Position, _characterModel.Rotation);
            DebuggingMarkers.WalkerModelMarker.SetActive(true);
        }

        private bool PathMarkersStale()
        {
            var destination1 = _destination;
            var destination2 = Destination;
            return (destination1.HasValue ? destination1.GetValueOrDefault() != destination2 ? 1 : 0 : 1) != 0 ||
                   _machinist.PathCorners.Count != _cornerMarkers.Count;
        }

        private void HideMarkers()
        {
            DebuggingMarkers.WalkerGameObjectMarker.SetActive(false);
            DebuggingMarkers.WalkerModelMarker.SetActive(false);
            DebuggingMarkers.DestinationMarker.SetActive(false);
            foreach (var cornerMarker in _cornerMarkers)
                Object.Destroy(cornerMarker);
            _cornerMarkers.Clear();
            _destination = new Vector3?();
        }

        private void ResetPathMarkers()
        {
            HideMarkers();
            DebuggingMarkers.DestinationMarker.transform.position = Destination;
            DebuggingMarkers.DestinationMarker.SetActive(true);
            foreach (var trackRoute in _machinist.PathCorners)
            foreach (var pathCorner in trackRoute.RouteCorners)
                _cornerMarkers.Add(Object.Instantiate(DebuggingMarkers.CornerMarkerPrefab, pathCorner, Quaternion.identity,
                    _machinist.TransformFast));
            _destination = Destination;
        }
    }
}