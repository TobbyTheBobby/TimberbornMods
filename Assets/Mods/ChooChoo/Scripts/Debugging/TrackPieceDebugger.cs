using System.Collections.Generic;
using Bindito.Core;
using ChooChoo.TrackSystem;
using Timberborn.Debugging;
using Timberborn.SelectionSystem;
using Timberborn.SingletonSystem;
using UnityEngine;

namespace ChooChoo.Debugging
{
    public class TrackPieceDebugger : ILoadableSingleton, ILateUpdatableSingleton
    {
        private EventBus _eventBus;
        private DebugModeManager _debugModeManager;
        private bool _walkerSelected;
        private TrackPiece _trackPiece;
        private readonly List<GameObject> _cornerMarkers = new();

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
        public void OnSelectableObjectSelected(
            SelectableObjectSelectedEvent selectableObjectSelectedEvent)
        {
            var componentFast = selectableObjectSelectedEvent.SelectableObject.GetComponentFast<TrackPiece>();
            if (!componentFast)
                return;
            UpdateSelectedWalker(componentFast);
        }

        [OnEvent]
        public void OnSelectableObjectUnselected(
            SelectableObjectUnselectedEvent selectableObjectUnselectedEvent)
        {
            HideMarkers();
            _walkerSelected = false;
        }

        private void UpdateSelectedWalker(TrackPiece trackPiece)
        {
            _trackPiece = trackPiece;
            _walkerSelected = true;
        }

        private void UpdateMarkers()
        {
            UpdateMarker();
            ResetPathMarkers();
        }

        private void UpdateMarker()
        {
            var transformFast = _trackPiece.TransformFast;
            DebuggingMarkers.WalkerGameObjectMarker.transform.SetPositionAndRotation(transformFast.position, transformFast.rotation);
            DebuggingMarkers.WalkerGameObjectMarker.SetActive(true);
        }

        private void HideMarkers()
        {
            DebuggingMarkers.WalkerGameObjectMarker.SetActive(false);
            DebuggingMarkers.WalkerModelMarker.SetActive(false);
            foreach (var cornerMarker in _cornerMarkers)
                Object.Destroy(cornerMarker);
            _cornerMarkers.Clear();
        }

        private void ResetPathMarkers()
        {
            HideMarkers();
            foreach (var trackRoute in _trackPiece.TrackRoutes)
            foreach (var pathCorner in trackRoute.RouteCorners)
                _cornerMarkers.Add(
                    Object.Instantiate(DebuggingMarkers.CornerMarkerPrefab, pathCorner, Quaternion.identity, _trackPiece.TransformFast));
        }
    }
}