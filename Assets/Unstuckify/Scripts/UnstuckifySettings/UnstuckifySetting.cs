using Timberborn.SettingsSystem;
using Timberborn.SingletonSystem;

namespace Unstuckify
{
    public class UnstuckifySetting : ILoadableSingleton
    {
        private readonly ISettings _settings;
        private readonly EventBus _eventBus;

        private AutomaticUnstukcifySetting _automaticUnstukcifySetting;
        
        public AutomaticUnstukcifySetting AutomaticUnstukcifySetting => _automaticUnstukcifySetting;
        
        UnstuckifySetting(ISettings settings, EventBus eventBus)
        {
            _settings = settings;
            _eventBus = eventBus;
        }
        
        public void Load()
        {
            _automaticUnstukcifySetting = new AutomaticUnstukcifySetting(_settings);
        }

        public void ChangeTrainModelSetting(bool value)
        {
            _automaticUnstukcifySetting.AutomaticUnstuckifyEnabled = value;
            _eventBus.Post(new UnstuckifySettingChangedEvent());
        }
    }
}
