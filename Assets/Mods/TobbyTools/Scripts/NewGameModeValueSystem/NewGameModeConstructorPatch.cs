// using System;
// using System.Reflection;
// using HarmonyLib;
// using TimberApi.DependencyContainerSystem;
// using Timberborn.CoreUI;
// using TobbyTools.UsedImplicitlySystem;
// using UnityEngine.UIElements;
//
// namespace TobbyTools.NewGameModeValueSystem
// {
//     [UsedImplicitlyHarmonyPatch]
//     public class NewGameModeConstructorPatch
//     {
//         public static MethodBase TargetMethod()
//         {
//             return AccessTools.Method(AccessTools.TypeByName("NewGameModePanel"), "GetPanel");
//         }
//
//         public static void Postfix(object __instance, VisualElement ____customModeSettings)
//         {
//             DependencyContainer.GetInstance<NewGameValueRepository>().AddVisualElements(____customModeSettings);
//             // var newGameValueRepository = DependencyContainer.GetInstance<NewGameValueRepository>();
//             //
//             // // Plugin.Log.LogError(newGameValueRepository.NewGameModeValues.Count() + "");
//             //
//             LogChildren(____customModeSettings);
//             //
//             // var visualElementInitializer = DependencyContainer.GetInstance<VisualElementInitializer>();
//             // foreach (var newGameModeValue in newGameValueRepository.NewGameModeValues)
//             // {
//             //     var visualElement = newGameModeValue.GetVisualElement();
//             //     visualElementInitializer.InitializeVisualElement(visualElement);
//             //     ____customModeSettings.Add(visualElement);
//             // }
//         }
//         
//         public static bool ImplementsInterfaceRecursively(Type type, Type interfaceType)
//         {
//             if (type == null)
//                 return false;
//
//             if (interfaceType.IsAssignableFrom(type))
//                 return true;
//
//             foreach (var @interface in type.GetInterfaces())
//             {
//                 if (ImplementsInterfaceRecursively(@interface, interfaceType))
//                     return true;
//             }
//
//             return ImplementsInterfaceRecursively(type.BaseType, interfaceType);
//         }
//         
//         private static void LogChildren(VisualElement parent)
//         {
//             foreach (var child in parent.Children())
//             {
//                 Plugin.Log.LogError(child + "");
//                 foreach (var cClass in child.GetClasses()) 
//                     Plugin.Log.LogWarning(cClass);
//                 LogChildren(child);
//             }
//         }
//     }
// }