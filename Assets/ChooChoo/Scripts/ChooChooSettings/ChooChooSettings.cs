using Timberborn.SettingsSystem;
using Timberborn.SingletonSystem;

namespace ChooChoo
{
    public class ChooChooSettings : ILoadableSingleton
    {
        private readonly ISettings _settings;

        private readonly EventBus _eventBus;

        private DefaultModelSettings _defaultModelSettings;

        public DefaultModelSettings DefaultModelSettings => _defaultModelSettings;
        
        ChooChooSettings(ISettings settings, EventBus eventBus)
        {
            _settings = settings;
            _eventBus = eventBus;
        }
        
        public void Load()
        {
            _defaultModelSettings = new DefaultModelSettings(_settings);
        }

        public void ChangeTrainModelSetting(string value)
        {
            _defaultModelSettings.DefaultTrainModel = value;
            _eventBus.Post(new ChooChooSettingsChanged());
        }
        
        public void ChangeWagonModelSetting(string value)
        {
            _defaultModelSettings.DefaultWagonModel = value;
            _eventBus.Post(new ChooChooSettingsChanged());
        }
    }
}
