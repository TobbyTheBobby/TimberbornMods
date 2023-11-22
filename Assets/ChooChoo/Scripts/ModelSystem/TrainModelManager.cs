using System.Collections.Generic;
using System.Linq;
using Bindito.Core;
using Timberborn.Persistence;
using UnityEngine;

namespace ChooChoo
{
    public class TrainModelManager : MonoBehaviour, IPersistentEntity
    {
        private static readonly ComponentKey TrainModelManagerKey = new(nameof(TrainModelManager));
        private static readonly PropertyKey<string> ActiveModelKey = new("ActiveTrainModel");
        
        private ChooChooSettings _chooChooSettings;

        private TrainModelSpecificationRepository _trainModelSpecificationRepository;
        
        private TrainModel[] _trainModels;

        public TrainModel[] TrainModels
        {
            get
            {
                if (_trainModels == null)
                    InitializeModels();
                return _trainModels;
            }
        }

        public TrainModel ActiveTrainModel { get; private set; }

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
            var list = new List<TrainModel>();
            foreach (var trainModelSpecification in _trainModelSpecificationRepository.ActiveTrainModels)
            {
                var model = ChooChooCore.FindBodyPart(transform, trainModelSpecification.ModelLocation.Split("/").Last()).gameObject;
                list.Add(new TrainModel(model, trainModelSpecification));
            }
            _trainModels = list.ToArray();
            ActiveTrainModel = GetTrainModel(_chooChooSettings.DefaultModelSettings.DefaultTrainModel);
        }

        public void UpdateTrainType(string value)
        {
            ActiveTrainModel = GetTrainModel(value);
            RefreshModel();
            GetComponent<WagonManager>().SetObjectToFollow();
        }

        public void RefreshModel()
        {
            foreach (var trainModel in TrainModels)
            {
                trainModel.Model.SetActive(trainModel == ActiveTrainModel);
            }
        }

        public void Save(IEntitySaver entitySaver)
        {
            entitySaver.GetComponent(TrainModelManagerKey).Set(ActiveModelKey, ActiveTrainModel.TrainModelSpecification.NameLocKey);
        }

        public void Load(IEntityLoader entityLoader)
        {
            if (!entityLoader.HasComponent(TrainModelManagerKey))
                return;
            if (!entityLoader.GetComponent(TrainModelManagerKey).Has(ActiveModelKey))
                return;
            ActiveTrainModel = GetTrainModel(entityLoader.GetComponent(TrainModelManagerKey).Get(ActiveModelKey));
        }

        private TrainModel GetTrainModel(string nameLocKey)
        {
            var trainModel = TrainModels.FirstOrDefault(model => model.TrainModelSpecification.NameLocKey == nameLocKey);
            return trainModel ?? TrainModels[0];
        }
    }
}