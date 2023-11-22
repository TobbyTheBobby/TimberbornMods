using System.Collections.Generic;
using System.Linq;
using Timberborn.Common;
using Timberborn.DropdownSystem;
using Timberborn.Localization;
using Timberborn.SingletonSystem;
using UnityEngine;

namespace ChooChoo
{
    public class TrainTypeSettingDropdownProvider : IExtendedDropdownProvider, ILoadableSingleton
    {
        private readonly ChooChooSettings _chooChooSettings;
        private readonly ILoc _loc;

        public IReadOnlyList<string> Items { get; private set; }
        
        public TrainTypeSettingDropdownProvider(ChooChooSettings chooSettings, ILoc loc)
        {
            _chooChooSettings = chooSettings;
            _loc = loc;
        }

        public void Load()
        {
            Items = new[] { "Tobbert.TrainModel.BigWooden", "Tobbert.TrainModel.SmallLog" }.ToList().AsReadOnlyList();
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
