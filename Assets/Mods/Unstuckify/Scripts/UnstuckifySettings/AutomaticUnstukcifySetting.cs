using Timberborn.SettingsSystem;

namespace Unstuckify
{
    public class AutomaticUnstukcifySetting
    {
        private readonly ISettings _settings;
        
        private string AutomaticUnstuckifyEnabledKey => nameof(AutomaticUnstuckifyEnabled);

        public AutomaticUnstukcifySetting(ISettings settings)
        {
            _settings = settings;
        }
        
        public bool AutomaticUnstuckifyEnabled
        {
            get => _settings.GetBool(AutomaticUnstuckifyEnabledKey, true);
            set => _settings.SetBool(AutomaticUnstuckifyEnabledKey, value);
        }
    }
}
