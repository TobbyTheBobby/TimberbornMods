using System.Linq;
using Bindito.Core;
using Timberborn.ConstructibleSystem;
using Timberborn.Coordinates;
using Timberborn.EntityPanelSystem;
using Timberborn.EntitySystem;
using Timberborn.Persistence;
using UnityEngine;

namespace ChooChoo
{
    public class OneWayTrack : TrackPiece, IPersistentEntity, IFinishedStateListener
    {
        private static readonly ComponentKey OneWayTrackKey = new(nameof(OneWayTrack));
        private static readonly PropertyKey<bool> DividesSectionKey = new("DividesSection");
        private static readonly PropertyKey<Direction2D> CurrentOneWayDirectionKey = new("CurrentOneWayDirection");

        [SerializeField] private GameObject Building;
        [SerializeField] private GameObject Sign;

        private EnumObjectSerializer<Direction2D> _direction2dObjectSerializer;
        private IEntityPanel _entityPanel;

        private Direction2D _currentOneWayDirection = Direction2D.Down;
        private LabeledPrefabSwitcher _labeledPrefabSwitcher;
        
        public bool IsOriginalDirection => _currentOneWayDirection == Direction2D.Down;
        
        public override TrackRoute[] TrackRoutes
        {
            get
            {
                if (!DividesSection)
                    return base.TrackRoutes;
                return IsOriginalDirection ? new[] { base.TrackRoutes[0] } : new[] { base.TrackRoutes[1] };
            }
        }

        new void Awake()
        {
            base.Awake();
            _labeledPrefabSwitcher = GetComponentFast<LabeledPrefabSwitcher>();
            Sign.SetActive(false);
            enabled = false; 
        }
        
        [Inject]
        public void InjectDependencies(EnumObjectSerializer<Direction2D> direction2dObjectSerializer, IEntityPanel entityPanel)
        {
            _direction2dObjectSerializer = direction2dObjectSerializer;
            _entityPanel = entityPanel;
        }

        public new void OnEnterFinishedState()
        {
            base.OnEnterFinishedState();
            Sign.SetActive(DividesSection);
            if (!IsOriginalDirection)
                RotateBuilding();
            enabled = true; 
        }

        public new void OnExitFinishedState()
        {
            base.OnExitFinishedState();
            enabled = false; 
        }

        public void ToggleDividesSection(bool newValue)
        {
            DividesSection = newValue;
            Sign.SetActive(DividesSection);
            UpdateEntityPanel();
            UpdateTracksAndSectionColors();
        }

        public void ChangeDirection()
        {
            RotateBuilding();
            _currentOneWayDirection = base.TrackRoutes.First(route => route.Exit.Direction != _currentOneWayDirection).Exit.Direction;
            UpdateTracksAndSectionColors();
        }

        public bool WouldCollideWithAnotherSectionDivider()
        {
            if (DividesSection)
                return false;
            return (TrackRoutes[0].Entrance.ConnectedTrackPiece != null && TrackRoutes[0].Entrance.ConnectedTrackPiece.DividesSection) || (TrackRoutes[0].Exit.ConnectedTrackPiece != null && TrackRoutes[0].Exit.ConnectedTrackPiece.DividesSection);
        }

        public void Save(IEntitySaver entitySaver)
        {
            if (DividesSection)
                entitySaver.GetComponent(OneWayTrackKey).Set(DividesSectionKey, DividesSection);
            entitySaver.GetComponent(OneWayTrackKey).Set(CurrentOneWayDirectionKey, _currentOneWayDirection, _direction2dObjectSerializer);
        }

        public void Load(IEntityLoader entityLoader)
        {
            if (!entityLoader.HasComponent(OneWayTrackKey))
                return;
            if (entityLoader.GetComponent(OneWayTrackKey).Has(DividesSectionKey))
                DividesSection = entityLoader.GetComponent(OneWayTrackKey).Get(DividesSectionKey);
            if (entityLoader.GetComponent(OneWayTrackKey).Has(CurrentOneWayDirectionKey))
                _currentOneWayDirection = entityLoader.GetComponent(OneWayTrackKey).Get(CurrentOneWayDirectionKey, _direction2dObjectSerializer);
            UpdateEntityPanel();
        }

        private void RotateBuilding() => Building.transform.RotateAround(CenterCoordinates, new Vector3(0, 1, 0), 180f);

        private void UpdateEntityPanel()
        {
            if (DividesSection)
                _labeledPrefabSwitcher.SetAlternative();
            else
                _labeledPrefabSwitcher.SetOriginal();
            _entityPanel.ReloadDescription(GetComponentFast<EntityComponent>());
        }
        
        private void UpdateTracksAndSectionColors()
        {
            TrackSection.Refresh();
            EventBus.Post(new OnTracksUpdatedEvent());
        }
    }
}