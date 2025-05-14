using ModSettings.Core;
using ModSettings.CoreUI;
using UnityEngine.UIElements;

namespace PipetteTool.SettingsSystem
{
    public class KeybindingModSettingElement : IModSettingElement
    {
        public VisualElement Root { get; }

        public ModSetting ModSetting { get; }

        public KeybindingModSettingElement(
            VisualElement root,
            ModSetting modSetting)
        {
            Root = root;
            ModSetting = modSetting;
        }

        public bool ShouldBlockInput => false;
    }
}