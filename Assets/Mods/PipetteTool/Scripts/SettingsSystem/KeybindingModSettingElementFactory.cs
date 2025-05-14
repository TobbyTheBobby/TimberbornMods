using ModSettings.Core;
using ModSettings.CoreUI;
using Timberborn.KeyBindingSystem;
using Timberborn.KeyBindingSystemUI;
using UnityEngine.UIElements;

namespace PipetteTool.SettingsSystem
{
    public class KeybindingModSettingElementFactory : IModSettingElementFactory
    {
        private readonly KeyBindingRowFactory _keyBindingRowFactory;
        private readonly KeyBindingRegistry _keyBindingRegistry;

        public KeybindingModSettingElementFactory(
            KeyBindingRowFactory keyBindingRowFactory,
            KeyBindingRegistry keyBindingRegistry)
        {
            _keyBindingRowFactory = keyBindingRowFactory;
            _keyBindingRegistry = keyBindingRegistry;
        }

        public int Priority => 1000;

        public bool TryCreateElement(ModSetting modSetting, out IModSettingElement element)
        {
            if (modSetting is KeyBindModSetting)
            {
                var container = new VisualElement();
                _keyBindingRowFactory.CreateKeyBindingRow(_keyBindingRegistry.Get(PipetteToolSystem.PipetteTool.PipetteToolShortcutKey), container);
                element = new KeybindingModSettingElement(container, modSetting);
                return true;
            }

            element = null;
            return false;
        }
    }
}