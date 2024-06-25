// using System;
// using System.Collections.Generic;
// using System.Collections.ObjectModel;
// using System.Diagnostics;
// using System.Reflection;
// using System.Threading;
// using BepInEx;
// using BepInEx.Logging;
// using HarmonyLib;
// using Timberborn.BehaviorSystem;
// using Timberborn.DistributionSystem;
// using Timberborn.DwellingSystem;
// using Timberborn.Goods;
// using Timberborn.InventorySystem;
// using Timberborn.Metrics;
// using Timberborn.Navigation;
// using Timberborn.NeedBehaviorSystem;
// using Timberborn.NeedSystem;
// using Timberborn.Planting;
// using Timberborn.WalkingSystem;
// using Timberborn.WorkSystem;
// using Timberborn.YielderFinding;
// using Timberborn.Yielding;
// using TimberbornAPI;
// using TimberbornAPI.Common;
// using UnityEngine;
//
// namespace MultithreadedNavigation
// {
//     [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
//     [BepInDependency("com.timberapi.timberapi")]
//     public class Plugin : BaseUnityPlugin
//     {
//         public const string PluginGuid = "tobbert.multithreadednavigation";
//         public const string PluginName = "Multithreaded Navigation";
//         public const string PluginVersion = "1.0.0";
//         
//         public static ManualLogSource Log;
//         
//         void Awake()
//         {
//             Log = Logger;
//             
//             Log.LogInfo($"Loaded {PluginName} Version: {PluginVersion}!");
//                         
//             TimberAPI.AssetRegistry.AddSceneAssets(PluginGuid, SceneEntryPoint.InGame);
//             TimberAPI.DependencyRegistry.AddConfigurator(new MultithreadedNavigationConfigurator());
//             new Harmony(PluginGuid).PatchAll();
//         }
//     }
//     
//     // [HarmonyPatch(typeof(BehaviorManager), "InjectDependencies", typeof(IDayNightCycle), typeof(IDayNightCycle), typeof(TimerMetricCache<RootBehavior>))]
//     [HarmonyPatch]
//     public class Patch
//     {
//         static MethodInfo TargetMethod()
//         {
//             return AccessTools.Method(AccessTools.TypeByName("BehaviorManager"), "InjectDependencies", new Type[] { AccessTools.TypeByName("IDayNightCycle"), AccessTools.TypeByName("IInstantiator"), typeof(TimerMetricCache<RootBehavior>)});
//         }
//     
//         static void Postfix(BehaviorManager __instance)
//         {
//             // Plugin.Log.LogInfo(__instance.name);
//             TimberAPI.DependencyContainer.GetInstance<MultithreadedNavigationService>().MyBehaviorManagers.Add(new MyBehaviorManager(__instance));
//         }
//     }
//     
//     
//     [HarmonyPatch(typeof(BehaviorManager), "Tick")]
//     public class BehaviorManagerPatch
//     {
//         static Stopwatch stopwatch = Stopwatch.StartNew();
//         public static long Time;
//     
//         private static readonly StackTrace StackTrace = new StackTrace();
//         
//         // static bool Prefix(BehaviorManager __instance)
//         // {
//         //     stopwatch.Start();
//         //     var test = _stackTrace.GetFrame(2).GetMethod().ReflectedType.Name;
//         //     stopwatch.Stop();
//         //     Plugin.Log.LogInfo(test);
//         //     Plugin.Log.LogFatal(stopwatch.ElapsedTicks);
//         //     stopwatch.Reset();
//         //     return false;
//         // }
//         
//         static bool Prefix()
//         {
//             // stopwatch.Start();
//             var className = StackTrace.GetFrame(2).GetMethod().ReflectedType.Name;
//             // stopwatch.Stop();
//             // Plugin.Log.LogFatal(stopwatch.ElapsedTicks);
//             // stopwatch.Reset();
//             return className == "MultithreadedNavigationJob";
//         
//             // TimberAPI.DependencyContainer.GetInstance<MultithreadedNavigationService>().BehaviorManagers.Add(new MyBehaviorManager(__instance));
//         }
//     
//         // static void Prefix()
//         // {
//         //     stopwatch.Start();
//         //     
//         // }
//         
//         // static void Postfix()
//         // {
//         //     stopwatch.Stop();
//         //     Time += stopwatch.ElapsedTicks;
//         //     Plugin.Log.LogFatal(stopwatch.ElapsedTicks);
//         //     stopwatch.Reset();
//         // }
//     }
//     
//     [HarmonyPatch]
//     public class TestPatch1
//     { 
//         public static IEnumerable<MethodBase> TargetMethods()
//         {
//             IEnumerable<MethodBase> targetMethods = new[]
//             {
//                 AccessTools.Method(AccessTools.TypeByName("HaulingCenter"), "GetWorkplaceBehaviorsOrdered"),
//             };
//     
//             return targetMethods;
//         }
//         
//         static bool Prefix(object __instance, MethodBase __originalMethod, ref IEnumerable<WorkplaceBehavior> __result)
//         {
//             __result = new List<WorkplaceBehavior>();
//             
//            return false;
//         }
//     }
//     
//     // [HarmonyPatch]
//     // public class TimerPatch
//     // { 
//     //     static Stopwatch stopwatch = Stopwatch.StartNew();
//     //     public static IEnumerable<MethodBase> TargetMethods()
//     //     {
//     //         IEnumerable<MethodBase> targetMethods = new[]
//     //         {
//     //             AccessTools.Method(AccessTools.TypeByName("GatherWorkplaceBehavior"), "Decide", new []
//     //                 {
//     //                     typeof(GameObject)
//     //                 }),
//     //         };
//     //
//     //         return targetMethods;
//     //     }
//     //     
//     //     static void Prefix(object __instance, MethodBase __originalMethod, ref Decision __result)
//     //     {
//     //         stopwatch.Start();
//     //     }
//     //
//     //     static void Postfix()
//     //     {
//     //         stopwatch.Stop();
//     //         var time2 = stopwatch.ElapsedTicks;
//     //         if (time2 > 10000)
//     //         {
//     //             Plugin.Log.LogInfo("GatherWorkplaceBehavior     " + time2);
//     //         }
//     //         stopwatch.Reset();
//     //     }
//     // }
//     //
//     // [HarmonyPatch]
//     // public class TestPatch3
//     // { 
//     //     static Stopwatch stopwatch = Stopwatch.StartNew();
//     //     
//     //     public static IEnumerable<MethodBase> TargetMethods()
//     //     {
//     //         IEnumerable<MethodBase> targetMethods = new[]
//     //         {
//     //             AccessTools.Method(AccessTools.TypeByName("WorkerRootBehavior"), "WorkAtWorkplace"),
//     //         };
//     //
//     //         return targetMethods;
//     //     }
//     //     
//     //     static bool Prefix(WorkerRootBehavior __instance, Worker ____worker, ref Decision __result)
//     //     {
//     //         // stopwatch.Start();
//     //         
//     //         foreach (WorkplaceBehavior workplaceBehavior in ____worker.Workplace.WorkplaceBehaviors)
//     //         {
//     //             stopwatch.Start();
//     //             Decision decision = workplaceBehavior.Decide(Traverse.Create(__instance).Property("gameObject").GetValue<GameObject>());
//     //             stopwatch.Stop();
//     //             var time2 = stopwatch.ElapsedTicks;
//     //             if (time2 > 10000)
//     //             {
//     //                 Plugin.Log.LogInfo("AAAAAAAAAAAA     " + time2);
//     //                 Plugin.Log.LogInfo(workplaceBehavior.Name);
//     //                 
//     //             }
//     //             stopwatch.Reset();
//     //             
//     //             if (!decision.ShouldReleaseNow)
//     //                 __result = Decision.TransferNow((Behavior) workplaceBehavior, in decision);
//     //         }
//     //         __result = Decision.ReleaseNow();
//     //         
//     //         stopwatch.Stop();
//     //         var time = stopwatch.ElapsedTicks;
//     //         // if (time > 10000)
//     //         // {
//     //         //     Plugin.Log.LogInfo("AAAAAAAAAAAA     " + time);
//     //         //     
//     //         //     
//     //         //     foreach (var workplace in ____worker.Workplace.WorkplaceBehaviors)
//     //         //     {
//     //         //         
//     //         //         Plugin.Log.LogInfo(workplace.name);
//     //         //     }
//     //         //
//     //         // }
//     //         
//     //         stopwatch.Reset();
//     //         
//     //         // __result = Decision.ReleaseNow();
//     //         return false;
//     //     }
//     //
//     //     // static void Postfix(Worker ____worker, ref Decision __result)
//     //     // {
//     //     //     stopwatch.Stop();
//     //     //     
//     //     //     
//     //     //     var time = stopwatch.ElapsedTicks;
//     //     //     if (time > 10000)
//     //     //     {
//     //     //         Plugin.Log.LogInfo("AAAAAAAAAAAA     " + time);
//     //     //         
//     //     //         
//     //     //         foreach (var workplace in ____worker.Workplace.WorkplaceBehaviors)
//     //     //         {
//     //     //             
//     //     //             Plugin.Log.LogInfo(workplace.name);
//     //     //         }
//     //     //
//     //     //     }
//     //     //     
//     //     //     stopwatch.Reset();
//     //     // }
//     // }
//     
//     [HarmonyPatch]
//     public class NotMainThreadPatch
//     { 
//         static readonly Thread MainThread = Thread.CurrentThread;
//         private static readonly object InstantiateLock = new object();
//         static Stopwatch stopwatch = Stopwatch.StartNew();
//         
//         public static IEnumerable<MethodBase> TargetMethods()
//         {
//             IEnumerable<MethodBase> targetMethods = new[]
//             {
//                 AccessTools.Method(AccessTools.TypeByName("CharacterModel"), "UpdateVisibility"),
//                 AccessTools.Method(AccessTools.TypeByName("Child"), "GrowUp"),
//                 AccessTools.Method(AccessTools.TypeByName("Mortal"), "DieIfItIsTime"),
//                 AccessTools.Method(AccessTools.TypeByName("AttractionFragment"), "UpdateButtons"),
//                 AccessTools.Method(AccessTools.TypeByName("BehaviorManager"), "TickRunningExecutor"),
//                 // AccessTools.Method(AccessTools.TypeByName("EntityService"), "Delete"),
//             };
//     
//             return targetMethods;
//         }
//         
//         static bool Prefix(object __instance, MethodBase __originalMethod)
//         {
//             lock (InstantiateLock)
//             {
//                 // stopwatch.Reset();
//                 if (Thread.CurrentThread != MainThread)
//                 {
//                     var list = TimberAPI.DependencyContainer.GetInstance<MultithreadedNavigationService>().RunMethodsOnMainThread.MethodsList;
//                     list.AddItem(new RunMethodsOnMainThread.OriginalMethod(__instance, __originalMethod));
//                     // stopwatch.Stop();
//                     // Plugin.Log.LogFatal(stopwatch.ElapsedTicks);
//                     return false;
//                 }
//             
//                 return true;
//             }
//         }
//     }
//     
//     [HarmonyPatch]
//     public class NonVoidPatches
//     {
//         public static IEnumerable<MethodBase> TargetMethods()
//         {
//             IEnumerable<MethodBase> targetMethods = new[]
//             {
//                 // AccessTools.Method(typeof(TerrainAStarPathfinder), "FillFlowFieldWithPath",
//                 //     new[]
//                 //     {
//                 //         typeof(TerrainNavMeshGraph), typeof(PathFlowField), typeof(float), typeof(int),
//                 //         typeof(IReadOnlyList<int>),
//                 //         typeof(int).MakeByRefType()
//                 //     }),
//                 // AccessTools.Method(AccessTools.TypeByName("PathfindingService"), "FindTerrainPathUncached",
//                 //     new[]
//                 //     {
//                 //         typeof(Vector3), typeof(Vector3), typeof(float), typeof(float).MakeByRefType(),
//                 //         typeof(List<Vector3>)
//                 //     }),
//                 // AccessTools.Method(AccessTools.TypeByName("PathfindingService"), "FindTerrainPathUncached",
//                 //     new[]
//                 //     {
//                 //         typeof(Vector3), typeof(IReadOnlyList<int>), typeof(float), typeof(float).MakeByRefType(),
//                 //         typeof(List<Vector3>)
//                 //     }),
//                 // AccessTools.Method(AccessTools.TypeByName("PathfindingService"), "FindRoadPathCached",
//                 //     new[]
//                 //     {
//                 //         typeof(Vector3), typeof(Vector3), typeof(float).MakeByRefType(),
//                 //         typeof(List<Vector3>)
//                 //     }),
//                 // AccessTools.Method(AccessTools.TypeByName("PathfindingService"), "FindTerrainPathCached",
//                 //     new[]
//                 //     {
//                 //         typeof(Vector3), typeof(Vector3), typeof(float), typeof(float).MakeByRefType(),
//                 //         typeof(List<Vector3>)
//                 //     }),
//                 AccessTools.Method(AccessTools.TypeByName("NavigationService"), "FindTerrainPath",
//                     new[]
//                     {
//                         typeof(Vector3), typeof(Vector3), typeof(float).MakeByRefType()
//                     }),
//                 AccessTools.Method(AccessTools.TypeByName("NavigationService"), "FindPathUnlimitedRange",
//                     new[]
//                     {
//                         typeof(Vector3), typeof(Vector3), typeof(List<Vector3>), typeof(float).MakeByRefType()
//                     }),
//                 AccessTools.Method(AccessTools.TypeByName("NavigationService"), "FindPathUnlimitedRange",
//                     new[]
//                     {
//                         typeof(Vector3), typeof(IReadOnlyList<Vector3>), typeof(List<Vector3>), typeof(float).MakeByRefType()
//                     }),
//                 // AccessTools.Method(typeof(FlowFieldPathFinder), "FindPathInFlowField",
//                 //     new[]
//                 //     {
//                 //         typeof(PathFlowField), typeof(Vector3), typeof(Vector3), typeof(float).MakeByRefType(),
//                 //         typeof(List<Vector3>)
//                 //     }),
//                 // AccessTools.Method(typeof(FlowFieldPathFinder), "FindPathInFlowField",
//                 //     new[]
//                 //     {
//                 //         typeof(AccessFlowField), typeof(Vector3), typeof(Vector3), typeof(float).MakeByRefType(),
//                 //         typeof(List<Vector3>)
//                 //     }),
//                 // AccessTools.Method(typeof(FlowFieldPathFinder), "FindPathInFlowField",
//                 //     new[]
//                 //     {
//                 //         typeof(AccessFlowField), typeof(RoadSpillFlowField), typeof(Vector3), typeof(Vector3),
//                 //         typeof(float).MakeByRefType(), typeof(List<Vector3>)
//                 //     }),
//                 // AccessTools.Method(typeof(FlowFieldPathFinder), "FindPathInFlowField",
//                 //     new[]
//                 //     {
//                 //         typeof(PathFlowField), typeof(RoadSpillFlowField), typeof(Vector3), typeof(Vector3),
//                 //         typeof(float).MakeByRefType(), typeof(List<Vector3>)
//                 //     }),
//                 // AccessTools.Method(typeof(FlowFieldPathFinder), "FindPathInFlowField",
//                 //     new[]
//                 //     {
//                 //         typeof(Vector3), typeof(IReadOnlyList<Vector3>), typeof(AccessFlowField),
//                 //         typeof(float).MakeByRefType(), typeof(List<Vector3>)
//                 //     }),
//                 AccessTools.Method(AccessTools.TypeByName("DistrictDestinationPicker"), "GetRandomDestination",
//                     new[]
//                     {
//                         typeof(District), typeof(Vector3)
//                     }),
//                 // AccessTools.Method(AccessTools.TypeByName("NavigationService"), "DestinationIsReachableUnlimitedRange",
//                 //     new[]
//                 //     {
//                 //         typeof(Vector3), typeof(Vector3)
//                 //     }),
//                 // AccessTools.Method(AccessTools.TypeByName("DistrictNeedBehaviorService"), "PickBestAction",
//                 //     new[]
//                 //     {
//                 //         typeof(NeedManager), typeof(Vector3), typeof(float), typeof(NeedFilter)
//                 //     }),
//                 AccessTools.Method(AccessTools.TypeByName("InventoryNeedBehavior"), "Decide",
//                     new[]
//                     {
//                         typeof(GameObject)
//                     }),
//                 AccessTools.Method(AccessTools.TypeByName("FarmHouseWorkplaceBehavior"), "Decide",
//                     new[]
//                     {
//                         typeof(GameObject)
//                     }),
//                 AccessTools.Method(AccessTools.TypeByName("LumberjackFlagWorkplaceBehavior"), "Decide",
//                     new[]
//                     {
//                         typeof(GameObject)
//                     }),
//                 AccessTools.Method(AccessTools.TypeByName("WaitInsideIdlyWorkplaceBehavior"), "Decide",
//                     new[]
//                     {
//                         typeof(GameObject)
//                     }),
//                 // AccessTools.Method(AccessTools.TypeByName("WorkerRootBehavior"), "Decide",
//                 //     new[]
//                 //     {
//                 //         typeof(GameObject)
//                 //     }),
//                 // AccessTools.Method(AccessTools.TypeByName("FarmHouseWorkplaceBehavior"), "DecideForAllowed",
//                 //     new[]
//                 //     {
//                 //         typeof(PlantBehavior), AccessTools.TypeByName("HarvestStarter"), typeof(GameObject)
//                 //     }),
//                 AccessTools.Method(AccessTools.TypeByName("PlanterWorkplaceBehavior"), "Decide",
//                     new[]
//                     {
//                         typeof(GameObject)
//                     }),
//                 AccessTools.Method(AccessTools.TypeByName("BringNutrientRootBehavior"), "Decide",
//                     new[]
//                     {
//                         typeof(GameObject)
//                     }),
//                 AccessTools.Method(AccessTools.TypeByName("GoodStackRetrieverBehavior"), "Decide",
//                     new[]
//                     {
//                         typeof(GameObject)
//                     }),
//                 // AccessTools.Method(AccessTools.TypeByName("NeederRootBehavior"), "Decide",
//                 //     new[]
//                 //     {
//                 //         typeof(GameObject)
//                 //     }),
//                 AccessTools.Method(AccessTools.TypeByName("BeaverNeedBehaviorPicker"), "GetBestNeedBehavior"),
//                 AccessTools.Method(AccessTools.TypeByName("PlantBehavior"), "Decide",
//                     new[]
//                     {
//                         typeof(GameObject)
//                     }),
//                 AccessTools.Method(AccessTools.TypeByName("CarryRootBehavior"), "Decide",
//                     new[]
//                     {
//                         typeof(GameObject)
//                     }),
//                 // AccessTools.Method(AccessTools.TypeByName("WanderRootBehavior"), "Decide",
//                 //     new[]
//                 //     {
//                 //         typeof(GameObject)
//                 //     }),
//                 AccessTools.Method(AccessTools.TypeByName("WanderRootBehavior"), "WalkToRandomDestination"),
//                 // AccessTools.Method(AccessTools.TypeByName("HaulWorkplaceBehavior"), "Decide",
//                 //     new[]
//                 //     {
//                 //         typeof(GameObject)
//                 //     }),
//                 // AccessTools.Method(AccessTools.TypeByName("FillInputWorkplaceBehavior"), "Decide",
//                 //     new[]
//                 //     {
//                 //         typeof(GameObject)
//                 //     }),
//                 // AccessTools.Method(AccessTools.TypeByName("WorkerRootBehavior"), "WorkAtWorkplace"),
//                 // AccessTools.Method(AccessTools.TypeByName("ClosestYielderFinder"), "FindYielder",
//                 //     new[]
//                 //     {
//                 //         typeof(Inventory), typeof(int), typeof(IEnumerable<ReachableYielder>), typeof(bool)
//                 //     }),
//                 // AccessTools.Method(AccessTools.TypeByName("CarrierInventoryFinder"), "TryCarryFromAnyInventory",
//                 //     new[]
//                 //     {
//                 //         typeof(string), typeof(Inventory)
//                 //     }),
//                 // AccessTools.Method(AccessTools.TypeByName("CarrierInventoryFinder"), "GetClosestInventoryWithCapacity",
//                 //     new[]
//                 //     {
//                 //         typeof(string), typeof(Accessible)
//                 //     }),
//                 // AccessTools.Method(AccessTools.TypeByName("CarrierInventoryFinder"), "GetDistrict",
//                 //     new[]
//                 //     {
//                 //         typeof(Accessible)
//                 //     }),
//                 // AccessTools.Method(AccessTools.TypeByName("HaulingCenter"), "GetWorkplaceBehaviorsOrdered"),
//                 // AccessTools.Method(AccessTools.TypeByName("FlowFieldCache"), "GetFlowFieldAtNode",
//                 //     new[]
//                 //     {
//                 //         typeof(int)
//                 //     }),
//                 // AccessTools.Method(AccessTools.TypeByName("FlowFieldCache"), "TryGetFlowFieldAtNode",
//                 //     new[]
//                 //     {
//                 //         typeof(int), typeof(AccessFlowField).MakeByRefType()
//                 //     }),
//                 // AccessTools.Method(AccessTools.TypeByName("YielderFinder"), "FindLivingYielderWithoutAccessible",
//                 //     new[]
//                 //     {
//                 //         typeof(Inventory), typeof(Accessible), typeof(int), typeof(IEnumerable<Yielder>)
//                 //     }),
//                 // AccessTools.Method(AccessTools.TypeByName("HarvestStarter"), "FindYielder",
//                 //     new[]
//                 //     {
//                 //         typeof(Inventory), typeof(IEnumerable<Yielder>)
//                 //     }),
//                 // AccessTools.Method(AccessTools.TypeByName("YielderFinder"), "FindYielderWithAccessible",
//                 //     new[]
//                 //     {
//                 //         typeof(Inventory), typeof(Accessible), typeof(int), typeof(IEnumerable<Yielder>)
//                 //     }),
//                 // AccessTools.Method(AccessTools.TypeByName("TerrainFlowFieldCache"), "GetFlowFieldAtNode",
//                 //     new[]
//                 //     {
//                 //         typeof(int)
//                 //     }),
//                 // AccessTools.Method(AccessTools.TypeByName("TerrainFlowFieldCache"), "TryGetFlowFieldAtNode",
//                 //     new[]
//                 //     {
//                 //         typeof(int), typeof(AccessFlowField).MakeByRefType()
//                 //     }),
//                 // AccessTools.Method(AccessTools.TypeByName("DistributableGoodBringer"), "BringDistributableGoods",
//                 //     new[]
//                 //     {
//                 //         typeof(DistributionPost)
//                 //     }),
//                 AccessTools.Method(AccessTools.TypeByName("ConstructionJob"), "StartConstructionJob",
//                     new[]
//                     {
//                         typeof(GameObject)
//                     }),
//                 AccessTools.Method(AccessTools.TypeByName("SleepNeedBehavior"), "GetEssentialAction"),
//                 AccessTools.Method(AccessTools.TypeByName("HaulingCenter"), "GetWorkplaceBehaviorsOrdered"),
//                 AccessTools.Method(AccessTools.TypeByName("HaulCandidate"), "GetWeightedBehaviors"),
//                 AccessTools.Method(AccessTools.TypeByName("Walker"), "FindPath",
//                     new[]
//                     {
//                         typeof(IDestination)
//                     }),
//             };
//     
//             return targetMethods;
//         }
//     
//     
//         static bool Prefix(object __instance, object[] __args, MethodBase __originalMethod, ref object __result)
//         {
//             var result = TimberAPI.DependencyContainer.GetInstance<MultithreadedNavigationService>().LockedNonVoidFunction(__instance, __args, __originalMethod);
//             if (result.Item2)
//             {
//                 return true;
//             }
//         
//             __result = result.Item1;
//             return false;
//         }
//     }
//
//     [HarmonyPatch]
//     public class VoidPatches
//     {
//         public static IEnumerable<MethodBase> TargetMethods()
//         {
//             IEnumerable<MethodBase> targetMethods = new[]
//             {
//                 AccessTools.Method(AccessTools.TypeByName("TerrainAStarPathfinder"), "FillFlowFieldWithPath",
//                     new[]
//     
//                     {
//                         AccessTools.TypeByName("TerrainNavMeshGraph"), AccessTools.TypeByName("PathFlowField"), typeof(float), typeof(int), typeof(int)
//                     }),
//                 // AccessTools.Method(AccessTools.TypeByName("RoadSpillFlowField"), "AddNode",
//                 //     new[]
//                 //     {
//                 //         typeof(int), typeof(int), typeof(int), typeof(float)
//                 //     }),
//                 // AccessTools.Method(AccessTools.TypeByName("RegularRoadFlowFieldGenerator"), "FillFlowField",
//                 //     new[]
//                 //     {
//                 //         typeof(RoadNavMeshGraph), typeof(AccessFlowField), typeof(AccessFlowField), typeof(int)
//                 //     }),
//                 // AccessTools.Method(AccessTools.TypeByName("TerrainFlowFieldGenerator"), "FillFlowFieldUpToDistance",
//                 //     new[]
//                 //     {
//                 //         typeof(TerrainNavMeshGraph), typeof(AccessFlowField), typeof(float), typeof(int)
//                 //     }),
//                 AccessTools.Method(AccessTools.TypeByName("PlantBehavior"), "ReserveCoordinates",
//                     new[]
//                     {
//                         typeof(GameObject), typeof(bool)
//                     }),
//                 AccessTools.Method(AccessTools.TypeByName("PlantingService"), "UnsetPlantingCoordinates",
//                     new[]
//                     {
//                         typeof(Vector3Int)
//                     }),
//                 AccessTools.Method(AccessTools.TypeByName("PlantingService"), "ReservePlantingCoordinates",
//                     new[]
//                     {
//                         typeof(Vector3Int)
//                     }),
//                 AccessTools.Method(AccessTools.TypeByName("PlantingService"), "UpdatePlantingSpot",
//                     new[]
//                     {
//                         typeof(Vector3Int)
//                     }),
//                 AccessTools.Method(AccessTools.TypeByName("GoodReserver"), "UnreserveStock"),
//                 AccessTools.Method(AccessTools.TypeByName("GoodReserver"), "ReserveStock",
//                     new[]
//                     {
//                         typeof(Inventory), typeof(GoodAmount), typeof(bool)
//                     }),
//                 AccessTools.Method(AccessTools.TypeByName("GoodReserver"), "ReserveExactStockAmount",
//                     new[]
//                     {
//                         typeof(Inventory), typeof(GoodAmount)
//                     }),
//                 AccessTools.Method(AccessTools.TypeByName("GoodReserver"), "ReserveCapacity",
//                     new[]
//                     {
//                         typeof(Inventory), typeof(GoodAmount)
//                     }),
//                 AccessTools.Method(AccessTools.TypeByName("GoodReserver"), "UnreserveCapacity"),
//                 AccessTools.Method(AccessTools.TypeByName("NotificationPanel"), "PostLoad"),
//                 AccessTools.Method(AccessTools.TypeByName("BringNutrientHaulBehaviorProvider"), "GetWeightedBehaviors"),
//                 // AccessTools.Method(AccessTools.TypeByName("HaulingCenter"), "UpdateHaulCandidates"),
//                 // AccessTools.Method(AccessTools.TypeByName("HaulingCenter"), "AddHaulCandidateInThisDistrict",
//                 //     new[]
//                 //     {
//                 //         typeof(GameObject)
//                 //     }),
//                 // AccessTools.Method(AccessTools.TypeByName("HaulingCenter"), "RemoveHaulCandidate",
//                 //     new[]
//                 //     {
//                 //         typeof(GameObject)
//                 //     }),
//                 // AccessTools.Method(AccessTools.TypeByName("FlowFieldCache"), "StartCachingAtNode",
//                 //     new[]
//                 //     {
//                 //         typeof(int)
//                 //     }),
//                 // AccessTools.Method(AccessTools.TypeByName("FlowFieldCache"), "StopCachingAtNode",
//                 //     new[]
//                 //     {
//                 //         typeof(int)
//                 //     }),
//                 // AccessTools.Method(AccessTools.TypeByName("FlowFieldCache"), "OnNodesChanged",
//                 //     new[]
//                 //     {
//                 //         typeof(ReadOnlyCollection<int>)
//                 //     }),
//                 // AccessTools.Method(AccessTools.TypeByName("TerrainFlowFieldCache"), "StartCachingAtNode",
//                 //     new[]
//                 //     {
//                 //         typeof(int)
//                 //     }),
//                 // AccessTools.Method(AccessTools.TypeByName("TerrainFlowFieldCache"), "StopCachingAtNode",
//                 //     new[]
//                 //     {
//                 //         typeof(int)
//                 //     }),
//                 // AccessTools.Method(AccessTools.TypeByName("Dweller"), "AssignToHome",
//                 //     new[]
//                 //     {
//                 //         typeof(Dwelling)
//                 //     }),
//                 // AccessTools.Method(AccessTools.TypeByName("Dweller"), "UnassignFromHome"),
//             };
//     
//             return targetMethods;
//         }
//     
//     
//         static bool Prefix(object __instance, object[] __args, MethodBase __originalMethod)
//         {
//             var result = TimberAPI.DependencyContainer.GetInstance<MultithreadedNavigationService>().LockedVoidFunction(__instance, __args, __originalMethod);
//             if (result)
//             {
//                 return true;
//             }
//         
//             return false;
//         }
//     }
// }