// using System;
// using Timberborn.Common;
// using Timberborn.CoreUI;
// using UnityEngine.UIElements;
//
// namespace TobbyTools.NewGameModeValueSystem.UiPresets
// {
//     public abstract class MinMaxUiPreset
//     {
//         public static VisualElement GetVisualElement(
//             string settingLabelLocKey,
//             string minFieldLocKey,
//             string maxFieldLocKey, 
//             Func<MinMax<int>> minMaxGetter,
//             Action<object> valueChanger)
//         {
//             var minMaxWrapper = GenericPresets.SettingWrapper();
//             
//             var label = GenericPresets.SettingLabel();
//             label._textLocKey = settingLabelLocKey;
//             minMaxWrapper.Add(label);
//             
//             var minNineSliceIntegerField = GenericPresets.IntegerField();
//             minNineSliceIntegerField.SetValueWithoutNotify(minMaxGetter().Min);
//             minNineSliceIntegerField.RegisterValueChangedCallback(evt => valueChanger(new MinMax<int>(evt.newValue, minMaxGetter().Max)));
//             minMaxWrapper.Add(minNineSliceIntegerField);
//
//             var minNineSliceIntegerFieldLabel = GenericPresets.LocalizableLabel();
//             minNineSliceIntegerFieldLabel._textLocKey = minFieldLocKey;
//             minMaxWrapper.Add(minNineSliceIntegerFieldLabel);
//             
//             var maxNineSliceIntegerField = GenericPresets.IntegerField();    
//             maxNineSliceIntegerField.SetValueWithoutNotify(minMaxGetter().Max);
//             maxNineSliceIntegerField.RegisterValueChangedCallback(evt => valueChanger(new MinMax<int>(minMaxGetter().Min, evt.newValue)));
//             minMaxWrapper.Add(maxNineSliceIntegerField);
//             
//             var maxNineSliceIntegerFieldLabel = GenericPresets.LocalizableLabel();
//             maxNineSliceIntegerFieldLabel._textLocKey = maxFieldLocKey;
//             minMaxWrapper.Add(maxNineSliceIntegerFieldLabel);
//             
//             return minMaxWrapper;
//         }
//     }
// }