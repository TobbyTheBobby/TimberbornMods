using Bindito.Core;
using System.Collections.Generic;
using System.Linq;
using Timberborn.CharacterModelSystem;
using Timberborn.Common;
using Timberborn.Debugging;
using Timberborn.SelectionSystem;
using Timberborn.SingletonSystem;
using Timberborn.WalkingSystemUI;
using UnityEngine;

namespace ChooChoo
{
  public class PathFollowerDebugger : ILoadableSingleton, ILateUpdatableSingleton
  {
    private GameObject _walkerGameObjectMarker;
    private GameObject _walkerModelMarker;
    private GameObject _destinationMarker;
    private GameObject _cornerMarkerPrefab;
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
      var walkerDebugger = Object.FindObjectsOfType<WalkerDebugger>(true).First();

      _walkerGameObjectMarker = (GameObject)ChooChooCore.GetInaccessibleField(walkerDebugger, "_walkerGameObjectMarker");
      _walkerModelMarker = (GameObject)ChooChooCore.GetInaccessibleField(walkerDebugger, "_walkerModelMarker");
      _destinationMarker = (GameObject)ChooChooCore.GetInaccessibleField(walkerDebugger, "_destinationMarker");
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
      Machinist componentFast = selectableObjectSelectedEvent.SelectableObject.GetComponentFast<Machinist>();
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

    private Vector3 Destination => _machinist.PathCorners.IsEmpty() ? _machinist.TransformFast.position : _machinist.PathCorners.Last().RouteCorners.Last();

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
      Transform transformFast = _machinist.TransformFast;
      _walkerGameObjectMarker.transform.SetPositionAndRotation(transformFast.position, transformFast.rotation);
      _walkerGameObjectMarker.SetActive(true);
      _walkerModelMarker.transform.SetPositionAndRotation(_characterModel.Position, _characterModel.Rotation);
      _walkerModelMarker.SetActive(true);
    }

    private bool PathMarkersStale()
    {
      Vector3? destination1 = _destination;
      Vector3 destination2 = Destination;
      return (destination1.HasValue ? destination1.GetValueOrDefault() != destination2 ? 1 : 0 : 1) != 0 || _machinist.PathCorners.Count != _cornerMarkers.Count;
    }

    private void HideMarkers()
    {
      _walkerGameObjectMarker.SetActive(false);
      _walkerModelMarker.SetActive(false);
      _destinationMarker.SetActive(false);
      foreach (var cornerMarker in _cornerMarkers)
        Object.Destroy(cornerMarker);
      _cornerMarkers.Clear();
      _destination = new Vector3?();
    }

    private void ResetPathMarkers()
    {
      HideMarkers();
      _destinationMarker.transform.position = Destination;
      _destinationMarker.SetActive(true);
      foreach (var trackRoute in _machinist.PathCorners)
          foreach (var pathCorner in trackRoute.RouteCorners)
            _cornerMarkers.Add(Object.Instantiate(_cornerMarkerPrefab, pathCorner, Quaternion.identity, _machinist.TransformFast));
      _destination = Destination;
    }
  }
}
