using System.Collections.Generic;
using JetBrains.Annotations;
using ModSettings.Common;
using ModSettings.Core;
using Timberborn.Modding;
using Timberborn.SettingsSystem;

namespace PipetteTool.SettingsSystem
{
    public class PipetteToolSettingsOwner : ModSettingsOwner
    {
        [UsedImplicitly]
        public KeyBindModSetting KeyBindModSetting { get; } = new("Ignored", ModSettingDescriptor.CreateLocalized("Pipette Tool Key Bind"));

        private ModSetting<string> _pipetteToolModeModSetting;

        public ModSetting<string> PipetteToolModeModSetting
        {
            get
            {
                var modSettingDescriptor = ModSettingDescriptor.CreateLocalized("Tobbert.PipetteTool.Setting.PipetteToolMode");
                modSettingDescriptor.SetLocalizedTooltip("Tobbert.PipetteTool.Setting.PipetteToolModeToolTip");
                return _pipetteToolModeModSetting ??= new LimitedStringModSetting(
                    0,
                    new List<LimitedStringModSettingValue>
                    {
                        new("Tobbert.PipetteTool.Setting.HoldMode", "Tobbert.PipetteTool.Setting.HoldMode"),
                        new("Tobbert.PipetteTool.Setting.PressMode", "Tobbert.PipetteTool.Setting.PressMode"),
                        new("Tobbert.PipetteTool.Setting.DirectModeKeyBind", "Tobbert.PipetteTool.Setting.DirectModeKeyBind"),
                        new("Tobbert.PipetteTool.Setting.DirectModeMiddleMouseButton", "Tobbert.PipetteTool.Setting.DirectModeMiddleMouseButton"),
                        new("Tobbert.PipetteTool.Setting.InvertedMode", "Tobbert.PipetteTool.Setting.InvertedMode"),
                    },
                    modSettingDescriptor);
            }
        }

        private ModSetting<bool> _pipetteToolForcePipette;

        public ModSetting<bool> PipetteToolForcePipette
        {
            get
            {
                var modSettingDescriptor = ModSettingDescriptor.CreateLocalized("Tobbert.PipetteTool.Setting.ForcePipette");
                modSettingDescriptor.SetLocalizedTooltip("Tobbert.PipetteTool.Setting.ForcePipetteTooltip");
                return _pipetteToolForcePipette ??= new ModSetting<bool>(false, modSettingDescriptor);
            }
        }

        public PipetteToolSettingsOwner(ISettings settings, ModSettingsOwnerRegistry modSettingsOwnerRegistry, ModRepository modRepository) : base(settings, modSettingsOwnerRegistry, modRepository)
        {
        }

        public override string HeaderLocKey => "Tobbert.PipetteTool.Setting.Header";

        public override ModSettingsContext ChangeableOn => ModSettingsContext.MainMenu | ModSettingsContext.Game | ModSettingsContext.MapEditor;

        protected override string ModId => Plugin.Id;
    }
}