// using System;
// using System.Collections.Generic;
// using System.Reflection;
// using HarmonyLib;
//
// namespace TobbyTools.CustomRangeBoundSystem
// {
//     public class NavigationRangeSericePatch
//     {
//         [HarmonyPatch]
//         public class RoadNavMeshGraphPatch
//         {
//             // private static readonly Dictionary<Transform, Passenger> PathLinkAwaiters = new();
//
//             public static IEnumerable<MethodInfo> TargetMethods() => new[]
//             {
//                 AccessTools.Method(AccessTools.TypeByName("NavigationRangeService"), "MoveAlongPath", new Type[3]
//                 {
//                     typeof (float),
//                     typeof (string),
//                     typeof (float)
//                 }),
//                 AccessTools.Method(AccessTools.TypeByName("NavigationRangeService"), "MoveAlongPath", new Type[3]
//                 {
//                     typeof (float),
//                     typeof (string),
//                     typeof (float)
//                 })
//             };
//
//             private static void Prefix()
//             {
//                 
//             }
//         }
//     }
// }