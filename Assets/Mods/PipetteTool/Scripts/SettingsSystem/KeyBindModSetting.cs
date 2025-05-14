using ModSettings.Core;

namespace PipetteTool.SettingsSystem
{
    public class KeyBindModSetting : ModSetting<string>
    {
        public KeyBindModSetting(string defaultValue, ModSettingDescriptor descriptor) : base(defaultValue, descriptor)
        {
        }
    }
}