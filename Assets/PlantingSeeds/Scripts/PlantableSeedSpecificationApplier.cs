using System.Linq;
using Bindito.Unity;
using Timberborn.Persistence;
using Timberborn.Planting;
using Timberborn.PrefabSystem;
using Timberborn.SingletonSystem;

namespace PlantingSeeds
{
    public class PlantableSeedSpecificationApplier : ILoadableSingleton
    {
        private readonly PlantableSeedSpecificationDeserializer _plantableSeedSpecificationDeserializer;
        private readonly ISpecificationService _specificationService;
        private readonly ObjectCollectionService _objectCollectionService;
        private readonly IInstantiator _instantiator;

        private PlantableSeedSpecification[] _plantableSeedSpecifications;

        PlantableSeedSpecificationApplier(
            PlantableSeedSpecificationDeserializer plantableSeedSpecificationDeserializer, 
            ISpecificationService specificationService, 
            ObjectCollectionService objectCollectionService,
            IInstantiator instantiator)
        {
            _plantableSeedSpecificationDeserializer = plantableSeedSpecificationDeserializer;
            _specificationService = specificationService;
            _objectCollectionService = objectCollectionService;
            _instantiator = instantiator;
        }
        
        public void Load()
        {
            _plantableSeedSpecifications = _specificationService.GetSpecifications(_plantableSeedSpecificationDeserializer).ToArray();
            
            foreach (var plantable in _objectCollectionService.GetAllMonoBehaviours<Plantable>())
            {
                var plantableSeedSpecification = _plantableSeedSpecifications.FirstOrDefault(specification => specification.PlantablePrefabName == plantable.PrefabName);
                if (plantableSeedSpecification == null)
                    continue;
            
                var plantingSeedComponent = plantable.GameObjectFast.GetComponent<PlantingSeedComponent>();
                if (plantingSeedComponent != null)
                    continue;
                
                plantingSeedComponent = _instantiator.AddComponent<PlantingSeedComponent>(plantable.GameObjectFast);
                plantingSeedComponent.GoodId = plantableSeedSpecification.GoodId;
                plantingSeedComponent.GoodAmount = plantableSeedSpecification.GoodAmount;
            }
        }
    }
}