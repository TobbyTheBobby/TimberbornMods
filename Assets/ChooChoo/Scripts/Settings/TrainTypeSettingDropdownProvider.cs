using System.Collections.Generic;
using System.Linq;
using ChooChoo.ModelSystem;
using TimberApi.SpecificationSystem;
using Timberborn.DropdownSystem;
using Timberborn.Localization;
using Timberborn.SingletonSystem;
using UnityEngine;

namespace ChooChoo.Settings
{
    public class TrainTypeSettingDropdownProvider : IExtendedDropdownProvider, ILoadableSingleton
    {
        private readonly TrainModelSpecificationDeserializer _trainModelSpecificationDeserializer;
        private readonly IApiSpecificationService _apiSpecificationService;
        private readonly ChooChooSettings _chooChooSettings;
        private readonly ILoc _loc;

        public IReadOnlyList<string> Items { get; private set; }

        public TrainTypeSettingDropdownProvider(
            TrainModelSpecificationDeserializer trainModelSpecificationDeserializer,
            IApiSpecificationService apiSpecificationService,
            ChooChooSettings chooSettings,
            ILoc loc)
        {
            _trainModelSpecificationDeserializer = trainModelSpecificationDeserializer;
            _apiSpecificationService = apiSpecificationService;
            _chooChooSettings = chooSettings;
            _loc = loc;
        }

        public void Load()
        {
            Items = _apiSpecificationService
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