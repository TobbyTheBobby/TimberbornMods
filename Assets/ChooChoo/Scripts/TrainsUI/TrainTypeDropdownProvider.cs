using System.Collections.Generic;
using System.Linq;
using Timberborn.BaseComponentSystem;
using Timberborn.Common;
using Timberborn.DropdownSystem;
using UnityEngine;

namespace ChooChoo
{
    public class TrainTypeDropdownProvider : BaseComponent, IExtendedDropdownProvider
    {
        private TrainModelManager _trainModelManager;

        public IReadOnlyList<string> Items { get; private set; }

        private void Awake()
        {
            _trainModelManager = GetComponentFast<TrainModelManager>();

            Items = _trainModelManager.TrainModels.Select(model => model.TrainModelSpecification.NameLocKey).ToList().AsReadOnlyList();
        }

        public string GetValue()
        {
            return _trainModelManager.ActiveTrainModel.TrainModelSpecification.NameLocKey;
        }

        public void SetValue(string value)
        {
            _trainModelManager.UpdateTrainType(value);
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
