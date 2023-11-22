using System.Collections.Generic;
using System.Linq;
using Timberborn.AssetSystem;
using Timberborn.GameFactionSystem;
using Timberborn.Persistence;
using Timberborn.SingletonSystem;
using UnityEngine;

namespace ChooChoo
{
    public class TrainModelSpecificationRepository : ILoadableSingleton
    {
        private readonly IResourceAssetLoader _resourceAssetLoader;

        private readonly ISpecificationService _specificationService;

        private readonly TrainModelSpecificationDeserializer _trainModelSpecificationDeserializer;
        
        private readonly WagonModelSpecificationDeserializer _wagonModelSpecificationDeserializer;

        private readonly FactionService _factionService;

        private TrainModelSpecification[] _trainModelSpecifications;

        public TrainModelSpecification[] ActiveTrainModels;
        
        private WagonModelSpecification[] _wagonModelSpecifications;

        public WagonModelSpecification[] ActiveWagonModels;

        public List<WagonModelSpecification> SelectableActiveWagonModels => ActiveWagonModels.Where(model => !ActiveWagonModels.Select(specification => specification.DependentModel).Contains(model.NameLocKey)).ToList();


        TrainModelSpecificationRepository(
            IResourceAssetLoader resourceAssetLoader,
            ISpecificationService specificationService,
            TrainModelSpecificationDeserializer trainModelSpecificationDeserializer, 
            WagonModelSpecificationDeserializer wagonModelSpecificationDeserializer, 
            FactionService factionService)
        {
            _resourceAssetLoader = resourceAssetLoader;
            _specificationService = specificationService;
            _trainModelSpecificationDeserializer = trainModelSpecificationDeserializer;
            _wagonModelSpecificationDeserializer = wagonModelSpecificationDeserializer;
            _factionService = factionService;
        }
        
        public void Load()
        {
            InitialzeTrainModels();
            InitialzeWagonModels();
        }

        private void InitialzeTrainModels()
        {
            var train = _resourceAssetLoader.Load<GameObject>("tobbert.choochoo/tobbert_choochoo/Train");

            var modelsContainer = ChooChooCore.FindBodyPart(train.transform, "#HeightOffset");
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
                var model = _resourceAssetLoader.Load<GameObject>(modelSpecification.ModelLocation);
                model.transform.SetParent(modelsContainer);
                model.transform.localPosition = Vector3.zero;
                activeTrainModels.Add(modelSpecification);
            }
            ActiveTrainModels = activeTrainModels.ToArray();
        }
        
        private void InitialzeWagonModels()
        {
            var train = _resourceAssetLoader.Load<GameObject>("tobbert.choochoo/tobbert_choochoo/Wagon");

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
                var model = _resourceAssetLoader.Load<GameObject>(modelSpecification.ModelLocation);
                model.transform.SetParent(modelsContainer);
                model.transform.localPosition = Vector3.zero;
                activeTrainModels.Add(modelSpecification);
            }
            ActiveWagonModels = activeTrainModels.ToArray();
        }
    }
}