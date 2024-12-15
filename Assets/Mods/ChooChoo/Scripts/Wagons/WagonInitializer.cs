using Timberborn.AssetSystem;
using Timberborn.BaseComponentSystem;
using Timberborn.Characters;
using Timberborn.EntitySystem;
using Timberborn.Localization;
using Timberborn.SingletonSystem;
using Timberborn.TimeSystem;
using UnityEngine;

namespace ChooChoo.Wagons
{
    public class WagonInitializer : ILoadableSingleton
    {
        private readonly IDayNightCycle _dayNightCycle;
        private readonly EntityService _entityService;
        private readonly IAssetLoader _assetLoader;
        private readonly ILoc _loc;

        private BaseComponent _trainWagonPrefab;

        private WagonInitializer(
            IDayNightCycle dayNightCycle,
            EntityService entityService,
            IAssetLoader assetLoader,
            ILoc loc)
        {
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
            var trainTransform = train.TransformFast;
            wagon.TransformFast.rotation = trainTransform.rotation;
            var offset = trainTransform.rotation * new Vector3(0, 0f, -0.6f * cartNumber - 1);
            var spawnLocation = trainTransform.position + offset;
            // Plugin.Log.LogInfo("Spawning wagon " + cartNumber + " at: " + spawnLocation);
            wagon.TransformFast.position = spawnLocation;
        }
    }
}