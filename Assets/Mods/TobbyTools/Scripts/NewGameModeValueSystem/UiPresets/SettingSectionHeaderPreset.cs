// using System;
// using UnityEngine.UIElements;
//
// namespace TobbyTools.NewGameModeValueSystem.UiPresets
// {
//     public abstract class SettingSectionHeaderPreset
//     {
//         public static VisualElement GetVisualElement(
//             string settingLabelLocKey,
//             bool enabled,
//             Action<bool> onToggleValueChange)
//         {
//             var settingSectionHeaderWrapper = GenericPresets.SettingWrapper();
//
//             var toggle = GenericPresets.Toggle();
//             toggle.SetValueWithoutNotify(enabled);
//             toggle.RegisterValueChangedCallback(evt => onToggleValueChange(evt.newValue));
//             settingSectionHeaderWrapper.Add(toggle);
//             
//             
//             var label = GenericPresets.SettingLabel();
//             label._textLocKey = settingLabelLocKey;
//             settingSectionHeaderWrapper.Add(label);
//
//             var settingsWrapper = new VisualElement
//             {
//                 name = "SettingsWrapper"
//             };
//             settingSectionHeaderWrapper.Add(settingsWrapper);
//             
//             
//             return settingSectionHeaderWrapper;
//         }
//     }
// }