using Bindito.Core;
using ChooChoo.NavigationSystem;
using ChooChoo.Trains;
using Timberborn.AssetSystem;
using Timberborn.BaseComponentSystem;
using Timberborn.BlockSystem;
using Timberborn.Characters;
using Timberborn.Common;
using Timberborn.Coordinates;
using Timberborn.EntitySystem;
using Timberborn.GameFactionSystem;
using Timberborn.InventorySystem;
using Timberborn.Localization;
using Timberborn.TimeSystem;
using UnityEngine;

namespace ChooChoo.TrainYards
{
    public class TrainYard : BaseComponent, IRegisteredComponent, IFinishedStateListener
    {
        private const string TrainNameLocKey = "Tobbert.Train.PrefabName";

        [SerializeField]
        private int _maxCapacity;

        private TrainYardService _trainYardService;
        private IDayNightCycle _dayNightCycle;
        private EntityService _entityService;
        private IAssetLoader _assetLoader;
        private ILoc _loc;

        private BlockObject _blockObject;
        
        public Inventory Inventory { get; private set; }

        public int MaxCapacity => _maxCapacity;

        [Inject]
        public void InjectDependencies(
            TrainYardService trainYardService,
            FactionService factionService,
            IDayNightCycle dayNightCycle,
            EntityService entityService,
            IAssetLoader assetLoader,
            ILoc loc)
        {
            _trainYardService = trainYardService;
            _dayNightCycle = dayNightCycle;
            _entityService = entityService;
            _assetLoader = assetLoader;
            _loc = loc;
        }

        public void Awake()
        {
            _blockObject = GetComponentFast<BlockObject>();
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
            var trainPrefab = _assetLoader.Load<GameObject>("Tobbert/Prefabs/Trains/Train").GetComponent<BaseComponent>();

            var train = _entityService.Instantiate(trainPrefab);

            foreach (var goodAmountSpecification in train.GetComponentFast<Train>().TrainCost)
                Inventory.Take(goodAmountSpecification.ToGoodAmount());

            train.GetComponentFast<TrainYardSubject>().HomeTrainYard = GetComponentFast<TrainDestination>();

            SetInitialTrainLocation(train);

            SetTrainName(train);
        }

        private void SetInitialTrainLocation(BaseComponent train)
        {
            train.TransformFast.rotation = _blockObject.Orientation.ToWorldSpaceRotation();
            var position = train.TransformFast.position;
            position += GetSpawnOffset();
            position += TransformFast.position;
            train.TransformFast.position = position;
        }

        private Vector3 GetSpawnOffset()
        {
            return _blockObject.FlipMode.Transform(_blockObject.Orientation.TransformInWorldSpace(new Vector3(0.5f, 0f, 2.8f)), 2);
        }

        private void SetTrainName(BaseComponent train)
        {
            var component = train.GetComponentFast<Character>();
            component.FirstName = _loc.T(TrainNameLocKey);
            component.DayOfBirth = _dayNightCycle.DayNumber;
        }
    }
}