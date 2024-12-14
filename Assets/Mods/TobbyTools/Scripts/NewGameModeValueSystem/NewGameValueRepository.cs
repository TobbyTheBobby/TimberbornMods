// using System.Collections.Generic;
// using System.Linq;
// using Timberborn.CoreUI;
// using UnityEngine.UIElements;
//
// namespace TobbyTools.NewGameModeValueSystem
// {
//     public class NewGameValueRepository
//     {
//         private readonly VisualElementInitializer _visualElementInitializer;
//         
//         public readonly IEnumerable<INewGameModeValue> NewGameModeValues;
//
//         private NewGameValueRepository(VisualElementInitializer visualElementInitializer, IEnumerable<INewGameModeValue> newGameModeValues)
//         {
//             _visualElementInitializer = visualElementInitializer;
//             NewGameModeValues = newGameModeValues;
//         }
//
//         public void AddVisualElements(VisualElement root)
//         {
//             var grouped = NewGameModeValues.GroupBy(value => value.Section);
//             
//             foreach (var newGameModeValueGroup in grouped)
//             {
//                 if (newGameModeValueGroup.Key != null)
//                 {
//                     var settingSection = new SettingSection(newGameModeValueGroup.Key, newGameModeValueGroup.ToList());
//                     _visualElementInitializer.InitializeVisualElement(settingSection.Root);
//                     root.Add(settingSection.Root);
//                     _visualElementInitializer.InitializeVisualElement(settingSection.SectionWrapperRoot);
//                     root.Add(settingSection.SectionWrapperRoot);
//                     continue;
//                 }
//                 foreach (var newGameModeValue in newGameModeValueGroup)
//                 {
//                     var visualElement = newGameModeValue.GetVisualElement();
//                     _visualElementInitializer.InitializeVisualElement(visualElement);
//                     root.Add(visualElement);
//                 }
//             }
//         }
//     }
// }