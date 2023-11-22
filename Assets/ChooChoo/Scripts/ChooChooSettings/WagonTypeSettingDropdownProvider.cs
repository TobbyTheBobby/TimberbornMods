using System.Collections.Generic;
using Timberborn.DropdownSystem;
using Timberborn.Localization;
using Timberborn.SingletonSystem;
using UnityEngine;

namespace ChooChoo
{
    public class WagonTypeSettingDropdownProvider : IExtendedDropdownProvider, ILoadableSingleton
    {
        private readonly ChooChooSettings _chooChooSettings;
        private readonly ILoc _loc;

        public IReadOnlyList<string> Items { get; private set; }
        
        public WagonTypeSettingDropdownProvider(ChooChooSettings chooSettings, ILoc loc)
        {
            _chooChooSettings = chooSettings;
            _loc = loc;
        }

        public void Load()
        {
            Items = new[] { "Tobbert.WagonModel.BoxWagon", "Tobbert.WagonModel.TankWagon", "Tobbert.WagonModel.FlatWagon", "Tobbert.WagonModel.FlipperWagon", "Tobbert.WagonModel.PassengerWagon", "Tobbert.WagonModel.MetalCart" };
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
