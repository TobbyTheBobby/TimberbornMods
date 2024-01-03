using Bindito.Core;
using System.Collections.Generic;
using System.Linq;
using Timberborn.BlockSystem;
using Timberborn.CharacterModelSystem;
using Timberborn.Common;
using Timberborn.Debugging;
using Timberborn.SelectionSystem;
using Timberborn.SingletonSystem;
using Timberborn.WalkingSystemUI;
using UnityEngine;

namespace ChooChoo
{
  public class TrackPieceDebugger : ILoadableSingleton, ILateUpdatableSingleton
  {
    private GameObject _walkerGameObjectMarker;
    private GameObject _walkerModelMarker;
    private GameObject _cornerMarkerPrefab;
    private EventBus _eventBus;
    private DebugModeManager _debugModeManager;
    private bool _walkerSelected;
    private TrackPiece _trackPiece;
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
      var walkerDebugger = Object.FindObjectsOfType<WalkerDebugger>(true).First();

      _walkerGameObjectMarker = (GameObject)ChooChooCore.GetInaccessibleField(walkerDebugger, "_walkerGameObjectMarker");
      _walkerModelMarker = (GameObject)ChooChooCore.GetInaccessibleField(walkerDebugger, "_walkerModelMarker");
      _cornerMarkerPrefab = (GameObject)ChooChooCore.GetInaccessibleField(walkerDebugger, "_cornerMarkerPrefab");

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
      if (!(bool) (Object) componentFast)
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
      Transform transformFast = _trackPiece.TransformFast;
      _walkerGameObjectMarker.transform.SetPositionAndRotation(transformFast.position, transformFast.rotation);
      _walkerGameObjectMarker.SetActive(true);
    }

    private void HideMarkers()
    {
      _walkerGameObjectMarker.SetActive(false);
      _walkerModelMarker.SetActive(false);
      foreach (var cornerMarker in _cornerMarkers)
        Object.Destroy(cornerMarker);
      _cornerMarkers.Clear();
      _destination = new Vector3?();
    }

    private void ResetPathMarkers()
    {
      HideMarkers();
      foreach (var trackRoute in _trackPiece.TrackRoutes)
          foreach (var pathCorner in trackRoute.RouteCorners)
            _cornerMarkers.Add(Object.Instantiate(_cornerMarkerPrefab, pathCorner, Quaternion.identity, _trackPiece.TransformFast));
    }
  }
}
