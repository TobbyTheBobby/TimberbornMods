// using Timberborn.CoreUI;
// using Timberborn.DropdownSystem;
// using UnityEngine;
// using UnityEngine.UIElements;
//
// namespace ChooChoo.Settings
// {
//     public class ChooChooSettingsUI
//     {
//         private readonly TrainTypeSettingDropdownProvider _trainTypeSettingDropdownProvider;
//         private readonly WagonTypeSettingDropdownProvider _wagonTypeSettingDropdownProvider;
//         private readonly DropdownItemsSetter _dropdownItemsSetter;
//         private readonly VisualElementLoader _visualElementLoader;
//         private readonly DropdownListDrawer _dropdownListDrawer;
//         private readonly UIBuilder _builder;
//
//         private ChooChooSettingsUI(
//             TrainTypeSettingDropdownProvider trainTypeSettingDropdownProvider,
//             WagonTypeSettingDropdownProvider wagonTypeSettingDropdownProvider,
//             DropdownItemsSetter dropdownItemsSetter,
//             VisualElementLoader visualElementLoader,
//             DropdownListDrawer dropdownListDrawer,
//             UIBuilder uiBuilder)
//         {
//             _trainTypeSettingDropdownProvider = trainTypeSettingDropdownProvider;
//             _wagonTypeSettingDropdownProvider = wagonTypeSettingDropdownProvider;
//             _dropdownItemsSetter = dropdownItemsSetter;
//             _visualElementLoader = visualElementLoader;
//             _dropdownListDrawer = dropdownListDrawer;
//             _builder = uiBuilder;
//         }
//
//         public void InitializeSelectorSettings(ref VisualElement root)
//         {
//             var container = _builder.CreateComponentBuilder().CreateVisualElement()
//                 .SetWidth(new Length(100, LengthUnit.Percent))
//                 .SetJustifyContent(Justify.Center)
//                 .SetAlignContent(Align.Center)
//                 .SetAlignItems(Align.Center)
//                 .BuildAndInitialize();
//
//             var customPathsHeader = _builder.CreateComponentBuilder()
//                 .CreateVisualElement()
//                 .AddPreset(factory =>
//                 {
//                     var test = factory.Labels().DefaultHeader();
//                     test.TextLocKey = "Tobbert.ChooChooSettings.Header";
//                     test.style.fontSize = new Length(16, LengthUnit.Pixel);
//                     test.style.unityFontStyleAndWeight = FontStyle.Bold;
//                     return test;
//                 })
//                 .BuildAndInitialize();
//
//             var trainModelSelector = _builder.CreateComponentBuilder()
//                 .CreateVisualElement()
//                 .SetFlexDirection(FlexDirection.Row)
//                 .SetWidth(new Length(100, LengthUnit.Percent))
//                 .SetHeight(new Length(60, LengthUnit.Pixel))
//                 .SetJustifyContent(Justify.Center)
//                 .SetAlignItems(Align.Center)
//                 .AddPreset(builder =>
//                 {
//                     var label = builder.Labels().DefaultText();
//                     label.TextLocKey = "Tobbert.ChooChooSettings.DefaultTrainModel";
//                     return label;
//                 })
//                 .AddPreset(_ =>
//                 {
//                     var fragment = _visualElementLoader.LoadVisualElement("Options/SettingsBox");
//                     var dropDown = fragment.Q<Dropdown>("ScreenResolution");
//                     _dropdownItemsSetter.SetLocalizableItems(dropDown, _trainTypeSettingDropdownProvider);
//                     dropDown.Initialize(_dropdownListDrawer);
//                     dropDown.Q<Label>("Label").ToggleDisplayStyle(false);
//                     return dropDown;
//                 })
//                 .BuildAndInitialize();
//
//             var wagonModelSelector = _builder.CreateComponentBuilder()
//                 .CreateVisualElement()
//                 .SetFlexDirection(FlexDirection.Row)
//                 .SetWidth(new Length(100, LengthUnit.Percent))
//                 .SetHeight(new Length(60, LengthUnit.Pixel))
//                 .SetJustifyContent(Justify.Center)
//                 .SetAlignItems(Align.Center)
//                 .AddPreset(builder =>
//                 {
//                     var label = builder.Labels().DefaultText();
//                     label.TextLocKey = "Tobbert.ChooChooSettings.DefaultWagonModel";
//                     return label;
//                 })
//                 .AddPreset(_ =>
//                 {
//                     var fragment = _visualElementLoader.LoadVisualElement("Options/SettingsBox");
//                     var dropDown = fragment.Q<Dropdown>("ScreenResolution");
//                     _dropdownItemsSetter.SetLocalizableItems(dropDown, _wagonTypeSettingDropdownProvider);
//                     dropDown.Initialize(_dropdownListDrawer);
//                     dropDown.Q<Label>("Label").ToggleDisplayStyle(false);
//                     return dropDown;
//                 })
//                 .BuildAndInitialize();
//
//             container.Add(customPathsHeader);
//             container.Add(trainModelSelector);
//             container.Add(wagonModelSelector);
//
//             var toggle = root.Q<Toggle>("AutoSavingOn");
//             toggle.parent.Add(container);
//         }
//     }
// }