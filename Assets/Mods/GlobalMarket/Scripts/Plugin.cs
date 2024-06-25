// using System.Collections.Generic;
// using System.Reflection;
// using HarmonyLib;
// using TimberApi.ConsoleSystem;
// using TimberApi.ModSystem;
// using Timberborn.GameDistricts;
// using Timberborn.InventorySystem;
// using Timberborn.Persistence;
// using Timberborn.PrefabSystem;
// using Timberborn.Stockpiles;
// using UnityEngine;
//
// namespace GlobalMarket
// {
//     public class Plugin : IModEntrypoint
//     {
//         public const string PluginGuid = "tobbert.globalmarket";
//         
//         public static IConsoleWriter Log;
//         
//         public void Entry(IMod mod, IConsoleWriter consoleWriter)
//         {
//             Log = consoleWriter; 
//             
//             new Harmony(PluginGuid).PatchAll();
//         }
//     }
//     
//     [HarmonyPatch]
//     public class StockpileInventoryInitializerPatch
//     {
//         public static MethodInfo TargetMethod()
//         {
//             return AccessTools.Method(AccessTools.TypeByName("StockpileInventoryInitializer"), "Initialize", new []
//             {
//                 typeof(Stockpile), typeof(Inventory)
//             });
//         }
//         
//         static void Prefix(Stockpile subject, Inventory decorator)
//         {
//             if (subject.GetComponent<Prefab>().PrefabName.Contains("GlobalMarket"))
//             {
//                 var fieldInfo = typeof(Inventory).GetField("_storage", BindingFlags.NonPublic | BindingFlags.Instance);
//                 fieldInfo.SetValue(decorator, GlobalMarket.GlobalMarketStorage);
//                 
//                 var fieldInfo2 = typeof(Inventory).GetField("_reservedStock", BindingFlags.NonPublic | BindingFlags.Instance);
//                 fieldInfo2.SetValue(decorator, GlobalMarket.GlobalMarketReservedStorage);
//             
//                 var fieldInfo3 = typeof(Inventory).GetField("_reservedCapacity", BindingFlags.NonPublic | BindingFlags.Instance);
//                 fieldInfo3.SetValue(decorator, GlobalMarket.GlobalMarketReservedCapacity);
//             }
//         }
//
//         // static void Postfix(Stockpile subject, Inventory decorator)
//         // {
//         //     Prefab.Destroy(subject.gameObject.GetComponent<GoodDesirer>());
//         // }
//     }
//     
//     [HarmonyPatch]
//     public class InventoryPatch
//     {
//         public static MethodInfo TargetMethod()
//         {
//             return AccessTools.Method(AccessTools.TypeByName("Inventory"), "Load", new []
//             {
//                 typeof(IEntityLoader)
//             });
//         }
//         
//         static bool Prefix(Inventory __instance)
//         {
//             if (__instance.GetComponent<Prefab>().PrefabName.Contains("GlobalMarket"))
//             {
//                 AddGlobalMarketStorage(__instance);
//
//                 return false;
//             }
//
//             return true;
//         }
//
//         static void AddGlobalMarketStorage(Inventory inventory)
//         {
//             var fieldInfo1 = typeof(Inventory).GetField("_storage", BindingFlags.NonPublic | BindingFlags.Instance);
//             fieldInfo1.SetValue(inventory, GlobalMarket.GlobalMarketStorage);
//             
//             var fieldInfo2 = typeof(Inventory).GetField("_reservedStock", BindingFlags.NonPublic | BindingFlags.Instance);
//             fieldInfo2.SetValue(inventory, GlobalMarket.GlobalMarketReservedStorage);
//             
//             var fieldInfo3 = typeof(Inventory).GetField("_reservedCapacity", BindingFlags.NonPublic | BindingFlags.Instance);
//             fieldInfo3.SetValue(inventory, GlobalMarket.GlobalMarketReservedCapacity);
//         }
//     }
//     
//     [HarmonyPatch]
//     public class MigrationTriggerPatch
//     {
//         public static MethodInfo TargetMethod()
//         {
//             return AccessTools.Method(AccessTools.TypeByName("MigrationTrigger"), "RegisterDistributorToCheck", new []
//             {
//                 typeof(Citizen)
//             });
//         }
//         
//         static bool Prefix(Citizen citizen)
//         {
//             if (citizen.GetComponent<Prefab>().PrefabName.Contains("AirBalloon"))
//             {
//                 return false;
//             }
//
//             return true;
//         }
//     }
//     
//     [HarmonyPatch]
//     public class InventoryInitializerPatch
//     {
//         private static readonly List<string> GoodTypes = new()
//         {
//             "Log",
//             "Barrel",
//             // "FuelBarrel",
//             // "CatalystBarrel"
//         };
//
//         public static MethodInfo TargetMethod()
//         {
//             return AccessTools.Method(AccessTools.TypeByName("InventoryInitializer"), "Initialize");
//         }
//         
//         static void Prefix(InventoryInitializer __instance)
//         {
//             var fieldInfo1 = typeof(InventoryInitializer).GetField("_inventory", BindingFlags.NonPublic | BindingFlags.Instance);
//             var inventory = fieldInfo1.GetValue(__instance) as Inventory;
//             
//             var fieldInfo2 = typeof(InventoryInitializer).GetField("_componentName", BindingFlags.NonPublic | BindingFlags.Instance);
//             var componentName = fieldInfo2.GetValue(__instance) as string;
//
//             if (componentName == "Stockpile" && inventory.GetComponent<Prefab>().PrefabName.Contains("GlobalMarket"))
//             {
//                 foreach (var goodType in GoodTypes)
//                 {
//                     __instance.AddAllowedGoodType(goodType);
//                 }
//             }
//         }
//     }
//     
//     [HarmonyPatch]
//     public class EmptiableFragmentPatch
//     {
//         public static MethodInfo TargetMethod()
//         {
//             return AccessTools.Method(AccessTools.TypeByName("EmptiableFragment"), "ShowFragment", new []
//             {
//                 typeof(GameObject)
//             });
//         }
//         
//         static bool Prefix(GameObject entity)
//         {
//             return !entity.GetComponent<Prefab>().PrefabName.Contains("GlobalMarket");
//         }
//     }
// }