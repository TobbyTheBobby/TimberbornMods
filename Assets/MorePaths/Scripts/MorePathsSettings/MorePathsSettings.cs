using System.Collections.Generic;
using System.Linq;
using Timberborn.SettingsSystem;
using Timberborn.SingletonSystem;

namespace MorePaths
{
    public class MorePathsSettings : ILoadableSingleton
    {
        private readonly ISettings _settings;
        
        private readonly MorePathsCore _morePathsCore;

        private readonly EventBus _eventBus;

        public readonly List<MorePathsSetting> Settings = new();
        
        MorePathsSettings(ISettings settings, MorePathsCore morePathsCore, EventBus eventBus)
        {
            _settings = settings;
            _morePathsCore = morePathsCore;
            _eventBus = eventBus;
        }
        
        public void Load()
        {
            foreach (var pathSpecification in _morePathsCore.PathsSpecifications)
            {
                // if (pathSpecification.Name == "DefaultPath")
                //     continue;
                Settings.Add(new MorePathsSetting(_settings, pathSpecification.Name));
            }
        }
        
        public MorePathsSetting GetSetting(string name) => Settings.First(setting => setting.PathName == name);
        
        public void ChangeSetting(string settingName, bool value)
        {
            var setting = GetSetting(settingName);
            setting.Enabled = value;
            _eventBus.Post(new MorePathsSettingsChanged());
        }
    }
}
