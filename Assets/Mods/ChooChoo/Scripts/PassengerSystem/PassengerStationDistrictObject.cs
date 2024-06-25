using Bindito.Core;
using ChooChoo.TrackSystem;
using Timberborn.BaseComponentSystem;
using Timberborn.BlockSystem;
using Timberborn.ConstructibleSystem;
using Timberborn.Navigation;
using Timberborn.Persistence;
using Timberborn.SingletonSystem;
using UnityEngine;

namespace ChooChoo.PassengerSystem
{
    public class PassengerStationDistrictObject : BaseComponent, IFinishedStateListener, IPersistentEntity
    {
        private static readonly ComponentKey PassengerStationDistrictObjectKey = new(nameof(PassengerStationDistrictObject));
        private static readonly PropertyKey<bool> GoesAcrossDistrictKey = new("GoesAcrossDistrict");

        [SerializeField] private Vector3Int _coordinateOffset;
        private IDistrictService _districtService;
        private EventBus _eventBus;
        private BlockObject _blockObject;

        public bool GoesAcrossDistrict { get; private set; }

        [Inject]
        public void InjectDependencies(IDistrictService districtService, EventBus eventBus)
        {
            _districtService = districtService;
            _eventBus = eventBus;
        }

        public void Awake()
        {
            enabled = false;
            _eventBus.Register(this);
            _blockObject = GetComponentFast<BlockObject>();
        }

        public void Save(IEntitySaver entitySaver)
        {
            entitySaver.GetComponent(PassengerStationDistrictObjectKey).Set(GoesAcrossDistrictKey, GoesAcrossDistrict);
        }

        public void Load(IEntityLoader entityLoader)
        {
            if (!entityLoader.HasComponent(PassengerStationDistrictObjectKey))
                return;
            if (!entityLoader.GetComponent(PassengerStationDistrictObjectKey).Has(GoesAcrossDistrictKey))
                return;
            UpdateDistrictObject(entityLoader.GetComponent(PassengerStationDistrictObjectKey).Get(GoesAcrossDistrictKey));
        }

        public void OnEnterFinishedState()
        {
            enabled = true;
        }

        public void OnExitFinishedState()
        {
            enabled = false;
        }

        public void UpdateDistrictObject(bool newValue)
        {
            if (GoesAcrossDistrict == false && newValue == false)
                return;

            GoesAcrossDistrict = newValue;
            if (GoesAcrossDistrict)
            {
                Enable();
            }
            else
            {
                Disable();
            }

            _eventBus.Post(new OnTracksUpdatedEvent());
        }

        private void Enable()
        {
            _districtService.SetObstacle(ObstacleCoordinates);
        }

        private void Disable()
        {
            _districtService.UnsetObstacle(ObstacleCoordinates);
        }

        private Vector3Int ObstacleCoordinates => _blockObject.Transform(_coordinateOffset);
    }
}