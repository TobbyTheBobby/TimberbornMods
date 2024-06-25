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
    public class WagonTypeSettingDropdownProvider : IExtendedDropdownProvider, ILoadableSingleton
    {
        private readonly WagonModelSpecificationDeserializer _wagonModelSpecificationDeserializer;
        private readonly IApiSpecificationService _apiSpecificationService;
        private readonly ChooChooSettings _chooChooSettings;
        private readonly ILoc _loc;

        public IReadOnlyList<string> Items { get; private set; }

        public WagonTypeSettingDropdownProvider(
            WagonModelSpecificationDeserializer wagonModelSpecificationDeserializer,
            IApiSpecificationService apiSpecificationService,
            ChooChooSettings chooSettings,
            ILoc loc)
        {
            _wagonModelSpecificationDeserializer = wagonModelSpecificationDeserializer;
            _apiSpecificationService = apiSpecificationService;
            _chooChooSettings = chooSettings;
            _loc = loc;
        }

        public void Load()
        {
            Items = _apiSpecificationService.GetSpecifications(_wagonModelSpecificationDeserializer)
                .GroupBy(specification => specification.Faction)
                .First()
                .Where(specification => specification.DependentModel != null)
                .Select(specification => specification.NameLocKey)
                .ToList();
        }

        public string GetValue()
        {
            return _chooChooSettings.DefaultModelSettings.DefaultWagonModel;
        }

        public void SetValue(string value)
        {
            _chooChooSettings.ChangeWagonModelSetting(value);
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