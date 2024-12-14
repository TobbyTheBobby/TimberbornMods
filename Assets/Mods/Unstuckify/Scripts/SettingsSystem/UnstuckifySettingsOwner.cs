using ModSettings.Core;
using Timberborn.Modding;
using Timberborn.SettingsSystem;

namespace Unstuckify.SettingsSystem
{
    public class UnstuckifySettingsOwner : ModSettingsOwner
    {
        private const string LabelLocKey = "Tobbert.Unstuckify.Setting.Label";

        public ModSetting<bool> UnstuckifyEnabledSetting { get; } = new(LabelLocKey, true);

        public UnstuckifySettingsOwner(ISettings settings, ModSettingsOwnerRegistry modSettingsOwnerRegistry, ModRepository modRepository) : base(settings, modSettingsOwnerRegistry, modRepository)
        {
        }

        public override string HeaderLocKey => "Tobbert.Unstuckify.Setting.Header";

        protected override string ModId => Unstuckify.Id;
    }
}