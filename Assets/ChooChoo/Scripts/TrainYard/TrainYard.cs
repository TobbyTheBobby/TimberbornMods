using Bindito.Core;
using Timberborn.AssetSystem;
using Timberborn.BaseComponentSystem;
using Timberborn.BlockSystem;
using Timberborn.Characters;
using Timberborn.Common;
using Timberborn.ConstructibleSystem;
using Timberborn.Coordinates;
using Timberborn.EntitySystem;
using Timberborn.GameFactionSystem;
using Timberborn.InventorySystem;
using Timberborn.Localization;
using Timberborn.TimeSystem;
using UnityEngine;

namespace ChooChoo
{
    public class TrainYard : BaseComponent, IRegisteredComponent, IFinishedStateListener
    {
        private const string TrainNameLocKey = "Tobbert.Train.PrefabName";

        [SerializeField]
        private int _maxCapacity;
        
        private ILoc _loc;

        private EntityService _entityService;

        private IResourceAssetLoader _resourceAssetLoader;

        private TrainYardService _trainYardService;

        private IDayNightCycle _dayNightCycle;
        public Inventory Inventory { get; private set; }
        public int MaxCapacity => _maxCapacity;

        [Inject]
        public void InjectDependencies(ILoc loc, EntityService entityService, IResourceAssetLoader resourceAssetLoader, FactionService factionService, TrainYardService trainYardService, IDayNightCycle dayNightCycle)
        {
            _loc = loc;
            _entityService = entityService;
            _resourceAssetLoader = resourceAssetLoader;
            _trainYardService = trainYardService;
            _dayNightCycle = dayNightCycle;
        }

        public void Awake()
        {
            enabled = false;
            if (!name.ToLower().Contains("preview"))
                _trainYardService.CurrentTrainYard = GetComponentFast<TrainDestination>();
        }

        public void OnEnterFinishedState()
        {
            enabled = true;
            Inventory.Enable();
        }

        public void OnExitFinishedState()
        {
            Inventory.Disable();
            enabled = false;
        }
        
        public void InitializeInventory(Inventory inventory)
        {
            Asserts.FieldIsNull(this, Inventory, "Inventory");
            Inventory = inventory;
        }

        public void InitializeTrain()
        {
            var trainPrefab = _resourceAssetLoader.Load<GameObject>("tobbert.choochoo/tobbert_choochoo/Train").GetComponent<BaseComponent>();

            var train = _entityService.Instantiate(trainPrefab);
            
            foreach (var goodAmountSpecification in train.GetComponentFast<Train>().TrainCost)
                Inventory.Take(goodAmountSpecification.ToGoodAmount());

            train.GetComponentFast<TrainYardSubject>().HomeTrainYard = GetComponentFast<TrainDestination>();

            SetInitialTrainLocation(train);

            SetTrainName(train);
        }

        private void SetInitialTrainLocation(BaseComponent train)
        {
            train.TransformFast.rotation = GetComponentFast<BlockObject>().Orientation.ToWorldSpaceRotation();
            var position = train.TransformFast.position;
            position += GetSpawnOffset();
            position += TransformFast.position;
            train.TransformFast.position = position;
        }

        private Vector3 GetSpawnOffset() => GetComponentFast<BlockObject>().Orientation.TransformInWorldSpace(new Vector3(0.5f, 0f, 2.8f));

        private void SetTrainName(BaseComponent train)
        {
            Character component = train.GetComponentFast<Character>();
            component.FirstName = _loc.T(TrainNameLocKey);
            component.DayOfBirth = _dayNightCycle.DayNumber;
        }
    }
}