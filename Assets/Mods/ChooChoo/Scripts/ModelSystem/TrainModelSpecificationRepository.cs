using System.Collections.Generic;
using System.Linq;
using ChooChoo.Core;
using Timberborn.AssetSystem;
using Timberborn.Common;
using Timberborn.GameFactionSystem;
using Timberborn.Persistence;
using Timberborn.SingletonSystem;
using UnityEngine;

namespace ChooChoo.ModelSystem
{
    public class TrainModelSpecificationRepository : ILoadableSingleton
    {
        private readonly TrainModelSpecificationDeserializer _trainModelSpecificationDeserializer;
        private readonly WagonModelSpecificationDeserializer _wagonModelSpecificationDeserializer;
        private readonly ISpecificationService _specificationService;
        private readonly IAssetLoader _assetLoader;
        private readonly FactionService _factionService;

        private TrainModelSpecification[] _trainModelSpecifications;
        public TrainModelSpecification[] ActiveTrainModels;

        private WagonModelSpecification[] _wagonModelSpecifications;
        public WagonModelSpecification[] ActiveWagonModels;

        public List<WagonModelSpecification> SelectableActiveWagonModels => ActiveWagonModels
            .Where(model => !ActiveWagonModels.Select(specification => specification.DependentModel).Contains(model.NameLocKey)).ToList();

        private TrainModelSpecificationRepository(
            TrainModelSpecificationDeserializer trainModelSpecificationDeserializer,
            WagonModelSpecificationDeserializer wagonModelSpecificationDeserializer,
            ISpecificationService specificationService,
            IAssetLoader assetLoader,
            FactionService factionService)
        {
            _trainModelSpecificationDeserializer = trainModelSpecificationDeserializer;
            _wagonModelSpecificationDeserializer = wagonModelSpecificationDeserializer;
            _specificationService = specificationService;
            _assetLoader = assetLoader;
            _factionService = factionService;
        }

        public void Load()
        {
            InitializeTrainModels();
            InitializeWagonModels();
        }

        private void InitializeTrainModels()
        {
            var train = _assetLoader.Load<GameObject>("Tobbert/Prefabs/Trains/Train");

            var modelsContainer = train.gameObject.FindChildTransform("#HeightOffset");
            foreach (Transform child in modelsContainer)
                Object.Destroy(child.gameObject);

            _trainModelSpecifications = _specificationService.GetSpecifications(_trainModelSpecificationDeserializer).ToArray();
            // TODO Move initializing to separate class Initializer
            var activeTrainModels = new List<TrainModelSpecification>();
            foreach (var modelSpecification in _trainModelSpecifications)
            {
                if (_factionService.Current.Id != modelSpecification.Faction)
                    continue;
                // Plugin.Log.LogInfo(modelSpecification.NameLocKey);
                var model = _assetLoader.Load<GameObject>(modelSpecification.ModelLocation);
                model.transform.SetParent(modelsContainer);
                model.transform.localPosition = Vector3.zero;
                activeTrainModels.Add(modelSpecification);
            }

            ActiveTrainModels = activeTrainModels.ToArray();
        }

        private void InitializeWagonModels()
        {
            var train = _assetLoader.Load<GameObject>("Tobbert/Prefabs/Wagons/Wagon");

            var modelsContainer = ChooChooCore.FindBodyPart(train.transform, "#HeightOffset");
            foreach (Transform child in modelsContainer)
                Object.Destroy(child.gameObject);

            _wagonModelSpecifications = _specificationService.GetSpecifications(_wagonModelSpecificationDeserializer).ToArray();
            // TODO Move initializing to separate class Initializer
            var activeTrainModels = new List<WagonModelSpecification>();
            foreach (var modelSpecification in _wagonModelSpecifications)
            {
                if (_factionService.Current.Id != modelSpecification.Faction)
                    continue;
                // Plugin.Log.LogInfo(modelSpecification.NameLocKey);
                var model = _assetLoader.Load<GameObject>(modelSpecification.ModelLocation);
                model.transform.SetParent(modelsContainer);
                model.transform.localPosition = Vector3.zero;
                activeTrainModels.Add(modelSpecification);
            }

            ActiveWagonModels = activeTrainModels.ToArray();
        }
    }
}