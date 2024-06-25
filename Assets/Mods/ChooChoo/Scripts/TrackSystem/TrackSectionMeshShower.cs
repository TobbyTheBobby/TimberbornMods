using System;
using Bindito.Core;
using ChooChoo.Trains;
using ChooChoo.Wagons;
using Timberborn.BaseComponentSystem;
using Timberborn.ConstructibleSystem;
using Timberborn.SelectionSystem;
using Timberborn.SingletonSystem;
using Timberborn.ToolSystem;
using UnityEngine;

namespace ChooChoo.TrackSystem
{
    public class TrackSectionMeshShower : BaseComponent, IFinishedStateListener
    {
        // height in object should be 0.06
        [SerializeField]
        private GameObject _trackSectionMesh;

        [SerializeField]
        private GameObject _trackEntranceSectionMesh;

        [SerializeField]
        private GameObject _trackExitSectionMesh;

        [SerializeField]
        private GameObject _trackContainedSectionMesh;

        private ToolGroupManager _toolGroupManager;
        private EventBus _eventBus;

        private TrackPiece _trackPiece;

        private MeshRenderer _trackContainedSectionMeshRenderer;
        private MeshRenderer _trackSectionMeshRenderer;
        private MeshRenderer _trackEntranceSectionMeshRenderer;
        private MeshRenderer _trackExitSectionMeshRenderer;

        private GameObject _selectedGameObject;

        [Inject]
        public void InjectDependencies(ToolGroupManager toolGroupManager, EventBus eventBus)
        {
            _toolGroupManager = toolGroupManager;
            _eventBus = eventBus;
            _trackPiece = GetComponentFast<TrackPiece>();
        }

        private void Start()
        {
            if (_trackContainedSectionMesh != null)
            {
                _trackContainedSectionMeshRenderer = _trackContainedSectionMesh.GetComponentInChildren<MeshRenderer>();
                _trackContainedSectionMeshRenderer.material = BoundsNavRangeServicePatch.Material;
                _trackContainedSectionMeshRenderer.material.renderQueue = BoundsNavRangeServicePatch.Material.renderQueue;
                _trackContainedSectionMesh.SetActive(false);
            }

            if (_trackEntranceSectionMesh != null)
            {
                _trackEntranceSectionMeshRenderer = _trackEntranceSectionMesh.GetComponentInChildren<MeshRenderer>();
                _trackEntranceSectionMeshRenderer.material = BoundsNavRangeServicePatch.Material;
                _trackEntranceSectionMeshRenderer.material.renderQueue = BoundsNavRangeServicePatch.Material.renderQueue;
                _trackEntranceSectionMesh.SetActive(false);
            }

            if (_trackExitSectionMesh != null)
            {
                _trackExitSectionMeshRenderer = _trackExitSectionMesh.GetComponentInChildren<MeshRenderer>();
                _trackExitSectionMeshRenderer.material = BoundsNavRangeServicePatch.Material;
                _trackExitSectionMeshRenderer.material.renderQueue = BoundsNavRangeServicePatch.Material.renderQueue;
                _trackExitSectionMesh.SetActive(false);
            }

            if (_trackSectionMesh != null)
            {
                _trackSectionMeshRenderer = _trackSectionMesh.GetComponentInChildren<MeshRenderer>();
                _trackSectionMeshRenderer.material = BoundsNavRangeServicePatch.Material;
                _trackSectionMeshRenderer.material.renderQueue = BoundsNavRangeServicePatch.Material.renderQueue;
                _trackSectionMesh.SetActive(false);
            }

            SetActive(false);
        }

        public void OnEnterFinishedState()
        {
            _eventBus.Register(this);
            UpdateColor();
            SetActive(!ShouldBeActive());
        }

        public void OnExitFinishedState()
        {
            _eventBus.Unregister(this);
        }

        [OnEvent]
        public void OnToolGroupEntered(ToolGroupEnteredEvent toolGroupEnteredEvent)
        {
            SetActive(ShouldBeActive());
        }

        [OnEvent]
        public void OnToolEntered(ToolEnteredEvent toolEnteredEvent)
        {
            SetActive(ShouldBeActive());
        }

        [OnEvent]
        public void OnTrackUpdate(OnTracksUpdatedEvent onTracksUpdatedEvent)
        {
            UpdateColor();
            SetActive(ShouldBeActive());
        }

        [OnEvent]
        public void OnSelectableObjectSelected(SelectableObjectSelectedEvent selectableObjectSelectedEvent)
        {
            _selectedGameObject = selectableObjectSelectedEvent.SelectableObject.GameObjectFast;
            SetActive(ShouldBeActive());
        }

        [OnEvent]
        public void OnSelectableObjectUnselected(SelectableObjectUnselectedEvent selectableObjectUnselectedEvent)
        {
            _selectedGameObject = null;
            SetActive(ShouldBeActive());
        }

        private bool ShouldBeActive()
        {
            if (_selectedGameObject != null &&
                (_selectedGameObject.GetComponent<TrackPiece>() ||
                 _selectedGameObject.GetComponent<Train>() ||
                 _selectedGameObject.GetComponent<TrainWagon>()))
            {
                return true;
            }

            var activeToolGroup = _toolGroupManager.ActiveToolGroup;

            var flag = false;
            try
            {
                flag = activeToolGroup != null && activeToolGroup.DisplayNameLocKey.ToLower().Contains("train");
            }
            catch (Exception)
            {
                // ignored
            }

            return flag;
        }

        private void UpdateColor()
        {
            if (_trackEntranceSectionMeshRenderer != null)
            {
                if (_trackPiece.TrackRoutes[0].Entrance.ConnectedTrackPiece != null)
                    _trackEntranceSectionMeshRenderer.material.SetColor("_BaseColor_1",
                        _trackPiece.TrackRoutes[0].Entrance.ConnectedTrackPiece.TrackSection.Color);
                else
                    _trackEntranceSectionMeshRenderer.material.SetColor("_BaseColor_1", Color.white);
            }

            if (_trackExitSectionMeshRenderer != null)
            {
                if (_trackPiece.TrackRoutes[0].Exit.ConnectedTrackPiece != null)
                    _trackExitSectionMeshRenderer.material.SetColor("_BaseColor_1",
                        _trackPiece.TrackRoutes[0].Exit.ConnectedTrackPiece.TrackSection.Color);
                else
                    _trackExitSectionMeshRenderer.material.SetColor("_BaseColor_1", Color.white);
            }

            if (_trackContainedSectionMeshRenderer != null)
                _trackContainedSectionMeshRenderer.material.SetColor("_BaseColor_1", _trackPiece.TrackSection.Color);
            if (_trackSectionMeshRenderer != null)
                _trackSectionMeshRenderer.material.SetColor("_BaseColor_1", _trackPiece.TrackSection.Color);
        }

        private void SetActive(bool active)
        {
            var flag1 = active && _trackPiece._dividesSection;
            if (_trackEntranceSectionMesh != null && _trackEntranceSectionMesh.activeSelf != flag1)
                _trackEntranceSectionMesh.SetActive(flag1);
            if (_trackExitSectionMesh != null && _trackExitSectionMesh.activeSelf != flag1)
                _trackExitSectionMesh.SetActive(flag1);
            if (_trackContainedSectionMesh != null && _trackContainedSectionMesh.activeSelf != flag1)
                _trackContainedSectionMesh.SetActive(flag1);
            var flag2 = active && !_trackPiece._dividesSection;
            if (_trackSectionMesh != null && _trackSectionMesh.activeSelf != flag2)
                _trackSectionMesh.SetActive(flag2);
        }
    }
}