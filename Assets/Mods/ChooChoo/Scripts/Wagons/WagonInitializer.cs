using ChooChoo.TrainYards;
using Timberborn.AssetSystem;
using Timberborn.BaseComponentSystem;
using Timberborn.BlockSystem;
using Timberborn.Characters;
using Timberborn.Coordinates;
using Timberborn.EntitySystem;
using Timberborn.Localization;
using Timberborn.SingletonSystem;
using Timberborn.TimeSystem;
using UnityEngine;

namespace ChooChoo.Wagons
{
    public class WagonInitializer : ILoadableSingleton
    {
        private readonly TrainYardService _trainYardService;
        private readonly IDayNightCycle _dayNightCycle;
        private readonly EntityService _entityService;
        private readonly IAssetLoader _assetLoader;
        private readonly ILoc _loc;

        private BaseComponent _trainWagonPrefab;

        private WagonInitializer(
            TrainYardService trainYardService,
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

        public void Load()
        {
            _trainWagonPrefab = _assetLoader.Load<GameObject>("Tobbert/Prefabs/Wagons/Wagon").GetComponent<BaseComponent>();
        }

        public TrainWagon InitializeWagon(BaseComponent train, int cartNumber)
        {
            var wagon = _entityService.Instantiate(_trainWagonPrefab);
            var trainWagon = wagon.GetComponentFast<TrainWagon>();
            trainWagon.Train = train;

            SetInitialWagonPosition(train, wagon, cartNumber);
            var simpleLabeledPrefab = wagon.GetComponentFast<SimpleLabeledEntitySpec>();
            var character = wagon.GetComponentFast<Character>();
            character.FirstName = _loc.T(simpleLabeledPrefab.EntityNameLocKey);
            character.DayOfBirth = _dayNightCycle.DayNumber;

            return wagon.GetComponentFast<TrainWagon>();
        }

        private void SetInitialWagonPosition(BaseComponent train, BaseComponent wagon, int cartNumber)
        {
            wagon.TransformFast.rotation = _trainYardService.CurrentTrainYard.GetComponentFast<BlockObject>().Orientation.ToWorldSpaceRotation();
            var transform1 = train.TransformFast;
            var offset = transform1.rotation * new Vector3(0, 0f, -0.6f * cartNumber - 1);
            var spawnLocation = transform1.position + offset;
            // Plugin.Log.LogInfo("Spawning wagon " + cartNumber + " at: " + spawnLocation);
            wagon.TransformFast.position = spawnLocation;
        }
    }
}