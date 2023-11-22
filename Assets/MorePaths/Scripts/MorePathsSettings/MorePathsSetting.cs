using Timberborn.SettingsSystem;

namespace MorePaths
{
    public class MorePathsSetting
    {
        private readonly ISettings _settings;
        
        private string PathEnabledKey => nameof(Enabled) + PathName;

        public readonly string PathName;

        public MorePathsSetting(ISettings settings, string pathName)
        {
            _settings = settings;
            PathName = pathName;
        }
        
        public bool Enabled
        {
            get => _settings.GetBool(PathEnabledKey, true);
            set => _settings.SetBool(PathEnabledKey, value);
        }
    }
}
