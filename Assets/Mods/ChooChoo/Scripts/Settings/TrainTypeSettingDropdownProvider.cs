using System.Collections.Generic;
using System.Linq;
using ChooChoo.ModelSystem;
using Timberborn.DropdownSystem;
using Timberborn.Localization;
using Timberborn.Persistence;
using Timberborn.SingletonSystem;
using UnityEngine;

namespace ChooChoo.Settings
{
    public class TrainTypeSettingDropdownProvider : IExtendedDropdownProvider, ILoadableSingleton
    {
        private readonly TrainModelSpecificationDeserializer _trainModelSpecificationDeserializer;
        private readonly ISpecificationService _specificationService;
        private readonly ChooChooSettings _chooChooSettings;
        private readonly ILoc _loc;

        public IReadOnlyList<string> Items { get; private set; }

        public TrainTypeSettingDropdownProvider(
            TrainModelSpecificationDeserializer trainModelSpecificationDeserializer,
            ISpecificationService specificationService,
            ChooChooSettings chooSettings,
            ILoc loc)
        {
            _trainModelSpecificationDeserializer = trainModelSpecificationDeserializer;
            _specificationService = specificationService;
            _chooChooSettings = chooSettings;
            _loc = loc;
        }

        public void Load()
        {
            Items = _specificationService
                .GetSpecifications(_trainModelSpecificationDeserializer)
                .GroupBy(specification => specification.Faction)
                .First()
                .Select(specification => specification.NameLocKey)
                .ToList();
        }

        public string GetValue()
        {
            return _chooChooSettings.DefaultModelSettings.DefaultTrainModel;
        }

        public void SetValue(string value)
        {
            _chooChooSettings.ChangeTrainModelSetting(value);
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