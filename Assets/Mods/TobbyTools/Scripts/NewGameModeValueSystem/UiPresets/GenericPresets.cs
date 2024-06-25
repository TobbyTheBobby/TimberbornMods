using Timberborn.CoreUI;
using UnityEngine.UIElements;

namespace TobbyTools.NewGameModeValueSystem.UiPresets
{
    public abstract class GenericPresets
    {
        public static VisualElement SettingWrapper()
        {
            var wrapper = new VisualElement();
            wrapper.AddToClassList("new-game-mode-panel__setting-wrapper");
            return wrapper;
        }
        
        public static LocalizableLabel SettingLabel()
        {
            var label = new LocalizableLabel();
            label.AddToClassList("new-game-mode-panel__setting-label");
            return label;
        }
        
        public static NineSliceIntegerField IntegerField()
        {
            var integerField = new NineSliceIntegerField();
            integerField.AddToClassList("new-game-mode-panel__setting-input");
            integerField.AddToClassList("text-field");
            return integerField;
        }

        public static LocalizableLabel LocalizableLabel()
        {
            var localizableLabel = new LocalizableLabel();
            localizableLabel.AddToClassList("new-game-mode-panel__setting-additional-text");
            return localizableLabel;
        }

        public static Toggle Toggle()
        {
            var toggle = new Toggle();
            toggle.AddToClassList("new-game-mode-panel__setting-toggle");
            return toggle;
        }
    }
}