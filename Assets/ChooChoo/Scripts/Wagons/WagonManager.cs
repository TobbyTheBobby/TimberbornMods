using System.Collections.Generic;
using Bindito.Core;
using Timberborn.BaseComponentSystem;
using Timberborn.EntitySystem;
using Timberborn.Persistence;

namespace ChooChoo
{
    public class WagonManager : BaseComponent, IDeletableEntity, IPersistentEntity
    {
        private static readonly ComponentKey TrainWagonManagerKey = new(nameof(TrainYard));

        private static readonly ListKey<TrainWagon> WagonsKey = new("TrainWagons");

        private EntityService _entityService;

        private WagonsObjectSerializer _wagonsObjectSerializer;

        private WagonInitializer _wagonInitializer;

        private WagonModelManager _wagonModelManager;

        public List<TrainWagon> Wagons { get; private set; }

        public int MinimumNumberOfWagons = 2;
        
        public int MaximumNumberOfWagons = 4;

        [Inject]
        public void InjectDependencies(
            EntityService entityService, 
            WagonsObjectSerializer wagonsObjectSerializer,
            WagonInitializer wagonInitializer)
        {
            _entityService = entityService;
            _wagonsObjectSerializer = wagonsObjectSerializer;
            _wagonInitializer = wagonInitializer;
        }

        public void Start()
        {
            if (Wagons == null)
                InitializeWagons();
            SetObjectToFollow();
            foreach (var trainWagon in Wagons) 
                trainWagon.Train = this;
        }

        public void DeleteEntity()
        {
            foreach (var wagon in Wagons)
                _entityService.Delete(wagon);
        }

        public void Save(IEntitySaver entitySaver)
        {
            if (Wagons != null)
                entitySaver.GetComponent(TrainWagonManagerKey).Set(WagonsKey, Wagons, _wagonsObjectSerializer);
        }

        public void Load(IEntityLoader entityLoader)
        {
            if (!entityLoader.HasComponent(TrainWagonManagerKey))
                return;
            var component = entityLoader.GetComponent(TrainWagonManagerKey);
            if (component.Has(WagonsKey))
                Wagons = component.Get(WagonsKey, _wagonsObjectSerializer);
        }

        public void SetObjectToFollow()
        {
            for (int i = Wagons.Count - 1; i > 0; i--)
                Wagons[i].InitializeObjectFollower(Wagons[i - 1].TransformFast, Wagons[i - 1].GetComponentFast<WagonModelManager>().ActiveWagonModel.WagonModelSpecification.Length);
            Wagons[0].InitializeObjectFollower(TransformFast, GetComponentFast<TrainModelManager>().ActiveTrainModel.TrainModelSpecification.Length);
        }

        private void InitializeWagons()
        {
            var trainWagons = new List<TrainWagon>();
            for (int i = 0; i < MaximumNumberOfWagons; i++)
                trainWagons.Add(_wagonInitializer.InitializeWagon(this, i));
            Wagons = trainWagons;
        }
    }
}