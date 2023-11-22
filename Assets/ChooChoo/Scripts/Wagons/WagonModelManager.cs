using System.Collections.Generic;
using System.Linq;
using Bindito.Core;
using Timberborn.BaseComponentSystem;
using Timberborn.Persistence;

namespace ChooChoo
{
    public class WagonModelManager : BaseComponent, IPersistentEntity
    {
        private static readonly ComponentKey WagonModelManagerKey = new(nameof(WagonModelManager));
        private static readonly PropertyKey<string> ActiveWagonModelKey = new("ActiveWagonModel");
        
        private ChooChooSettings _chooChooSettings;

        private TrainModelSpecificationRepository _trainModelSpecificationRepository;
        
        private WagonModel[] _trainModels;
        private WagonModel _activeWagonModel;
        
        public WagonModel[] WagonModels
        {
            get
            {
                if (_trainModels == null)
                    InitializeModels();
                return _trainModels;
            }
        }
        
        public WagonModel ActiveWagonModel
        {
            get
            {
                if (_activeWagonModel == null)
                    InitializeModels();
                return _activeWagonModel;
            }
            private set => _activeWagonModel = value;
        }

        [Inject]
        public void InjectDependencies(ChooChooSettings chooChooSettings, TrainModelSpecificationRepository trainModelSpecificationRepository) 
        {
            _chooChooSettings = chooChooSettings;
            _trainModelSpecificationRepository = trainModelSpecificationRepository;
        }

        private void Start()
        {
            RefreshModel();
        }

        private void InitializeModels()
        {
            var list = new List<WagonModel>();
            foreach (var wagonModelSpecification in _trainModelSpecificationRepository.ActiveWagonModels)
            {
                var model = ChooChooCore.FindBodyPart(TransformFast, wagonModelSpecification.ModelLocation.Split("/").Last()).gameObject;
                list.Add(new WagonModel(model, wagonModelSpecification));
            }
            _trainModels = list.ToArray();
            ActiveWagonModel = GetWagonModel(_chooChooSettings.DefaultModelSettings.DefaultWagonModel);
        }

        public void UpdateWagonType(string value)
        {
            ActiveWagonModel = GetWagonModel(value);
            RefreshModel();
            GetComponentFast<TrainWagon>().Train.GetComponentFast<WagonManager>().SetObjectToFollow();
        }

        private void RefreshModel()
        {
            foreach (var wagonModel in WagonModels)
            {
                wagonModel.Model.SetActive(wagonModel == ActiveWagonModel || wagonModel.WagonModelSpecification.NameLocKey == ActiveWagonModel.WagonModelSpecification.DependentModel);
            }
        }
        
        public void Save(IEntitySaver entitySaver)
        {
            entitySaver.GetComponent(WagonModelManagerKey).Set(ActiveWagonModelKey, ActiveWagonModel.WagonModelSpecification.NameLocKey);
        }

        public void Load(IEntityLoader entityLoader)
        {
            if (!entityLoader.HasComponent(WagonModelManagerKey))
                return;
            ActiveWagonModel = GetWagonModel(entityLoader.GetComponent(WagonModelManagerKey).Get(ActiveWagonModelKey));
        }

        private WagonModel GetWagonModel(string nameLocKey)
        {
            var trainModel = WagonModels.FirstOrDefault(model => model.WagonModelSpecification.NameLocKey == nameLocKey);
            return trainModel ?? WagonModels[0];
            
        }
    }
}