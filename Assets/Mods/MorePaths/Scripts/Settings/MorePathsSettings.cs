using ModSettings.Core;
using Timberborn.Modding;
using Timberborn.SettingsSystem;

namespace MorePaths.Settings
{
    public class MorePathsSettings : ModSettingsOwner
    {
        public MorePathsSettings(ISettings settings, ModSettingsOwnerRegistry modSettingsOwnerRegistry, ModRepository modRepository) : base(settings, modSettingsOwnerRegistry, modRepository)
        {
        }

        public override string HeaderLocKey => "Tobbert.MorePaths.SettingsHeader";

        protected override string ModId => "Tobbert.MorePaths";

        public ModSetting<bool> CornersEnabledSetting { get; } = new(true, ModSettingDescriptor.CreateLocalized("Tobbert.MorePaths.PathCornersEnabled"));
    }
}