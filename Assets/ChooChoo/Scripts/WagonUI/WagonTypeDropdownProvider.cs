using System.Collections.Generic;
using System.Linq;
using Bindito.Core;
using Timberborn.BaseComponentSystem;
using Timberborn.Common;
using Timberborn.DropdownSystem;
using UnityEngine;

namespace ChooChoo
{
    public class WagonTypeDropdownProvider : BaseComponent, IExtendedDropdownProvider
    {
        private TrainModelSpecificationRepository _trainModelSpecificationRepository;

        private WagonModelManager _wagonModelManager;

        public IReadOnlyList<string> Items { get; private set; }
        
        [Inject]
        public void InjectDependencies(TrainModelSpecificationRepository trainModelSpecificationRepository)
        {
            _trainModelSpecificationRepository = trainModelSpecificationRepository;
        }

        private void Awake()
        {
            _wagonModelManager = GetComponentFast<WagonModelManager>();
            
            Items = _trainModelSpecificationRepository.SelectableActiveWagonModels.Select(model => model.NameLocKey).ToList().AsReadOnlyList();
        }

        public string GetValue()
        {
            return _wagonModelManager.ActiveWagonModel.WagonModelSpecification.NameLocKey;
        }

        public void SetValue(string value)
        {
            _wagonModelManager.UpdateWagonType(value);
        }

        public string FormatDisplayText(string value)
        {
            return value;
        }

        public Sprite GetIcon(string value)
        {
            return null;
        }
    }
}
