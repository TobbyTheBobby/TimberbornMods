// using System;
// using Timberborn.Common;
// using Timberborn.CoreUI;
// using TobbyTools.NewGameModeValueSystem.UiPresets;
// using UnityEngine.Assertions;
// using UnityEngine.UIElements;
//
// namespace TobbyTools.NewGameModeValueSystem
// {
//     public abstract class NewGameModeValue<T> : INewGameModeValue
//     {
//         private T _currentValue;
//
//         public T CurrentValue
//         {
//             get
//             {
//                 if (SectionIsEnabled)
//                     return _currentValue;
//                 throw new InvalidOperationException($"Accessing CurrentValue from NewGameModeValue not allowed when setting was disabled.");
//             }
//         }
//         
//         public virtual string Section { get; } = null;
//         public bool SectionIsEnabled { get; set; } = true;
//         
//         protected abstract string SettingLabelLocKey { get; }
//         protected virtual string MinFieldLocKey { get; } = null;
//         protected virtual string MaxFieldLocKey { get; } = null;
//         
//         public abstract T EasyDifficultyValue { get; }
//         public abstract T MediumDifficultyValue { get; }
//         public abstract T HardDifficultyValue { get; }
//
//         public virtual VisualElement GetVisualElement()
//         {
//             switch (typeof(T))
//             {
//                 case { IsGenericType: true } t when t.GetGenericTypeDefinition() == typeof(MinMax<>):
//                     Assert.IsNotNull(SettingLabelLocKey, $"{nameof(SettingLabelLocKey)} was null when creating the relevant UI.");
//                     Assert.IsNotNull(MinFieldLocKey, $"{nameof(MinFieldLocKey)} was null when creating the relevant UI.");
//                     Assert.IsNotNull(MaxFieldLocKey, $"{nameof(MaxFieldLocKey)} was null when creating the relevant UI.");
//                     return MinMaxUiPreset.GetVisualElement(SettingLabelLocKey, MinFieldLocKey, MaxFieldLocKey, () => (MinMax<int>)(object)CurrentValue, (asd) => UpdateCurrentValue((T)asd));
//                 default:
//                     throw new ArgumentOutOfRangeException($"No visual element for type: '{typeof(T)}'.");
//             }
//         }
//
//         public void OnEasyModeButtonClicked()
//         {
//             UpdateCurrentValue(EasyDifficultyValue);
//         }
//
//         public void OnNormalModeButtonClicked()
//         {
//             UpdateCurrentValue(MediumDifficultyValue);
//         }
//
//         public void OnHardModeButtonClicked()
//         {
//             UpdateCurrentValue(HardDifficultyValue);
//         }
//
//         public void SetState(bool newState)
//         {
//             SectionIsEnabled = newState;
//         }
//
//         private void UpdateCurrentValue(T newCurrentValue)
//         {
//             Plugin.Log.LogInfo($"newCurrentValue: {newCurrentValue}");
//             _currentValue = newCurrentValue;
//         }
//     }
// }