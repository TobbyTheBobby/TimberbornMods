using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using Timberborn.TickSystem;
using Unity.Collections;
using Unity.Jobs;

namespace MultithreadedNavigation
{
    public class MultithreadedNavigationService : ITickableSingleton
    {
        public readonly RunMethodsOnMainThread RunMethodsOnMainThread;

        MultithreadedNavigationService(RunMethodsOnMainThread runMethodsOnMainThread)
        {
            RunMethodsOnMainThread = runMethodsOnMainThread;
        }
        
        // public readonly Dictionary<int, MyBehaviorManager> BehaviorManagers = new();
        public readonly List<MyBehaviorManager> MyBehaviorManagers = new();

        Stopwatch stopwatch1 = Stopwatch.StartNew();
        Stopwatch stopwatch2 = Stopwatch.StartNew();
        Stopwatch stopwatch3 = Stopwatch.StartNew();
        private List<long> times = new();

        public void Tick()
        {
            // var time = BehaviorManagerPatch.Time;
            // times.Add(time);
            // if (times.Count>3)
            // {
            //     Plugin.Log.LogFatal(times.GetRange(2, times.Count-3).Average());
            // }
            // BehaviorManagerPatch.Time = 0;
            
            // Plugin.Log.LogInfo("Tick");

            stopwatch1.Start();
            stopwatch2.Start();
            // for (int i = 0; i < MyBehaviorManagers.Count; i++)
            // {
            //     var newObject = MyBehaviorManagers[i];
            //     if (newObject.BehaviorManager == null)
            //     {
            //         MyBehaviorManagers.Remove(newObject);
            //     }
            // }
            var behaviorManagers = new NativeArray<MyBehaviorManager>(MyBehaviorManagers.ToArray(), Allocator.TempJob);
            var job = new MultithreadedNavigationJob { MyBehaviorManagers = behaviorManagers };
            var jobHandle = job.Schedule(behaviorManagers.Length, 3);
            stopwatch2.Stop();
            stopwatch3.Start();
            jobHandle.Complete();
            stopwatch2.Start();
            stopwatch3.Stop();
            behaviorManagers.Dispose();
            
            RunMethodsOnMainThread.UpdateAllVisibilities();
            // LockObjects.Clear();

            // stopwatch1.Stop();
            // var time = stopwatch1.ElapsedTicks;
            // Plugin.Log.LogFatal("Total this tick: " + time);
            // // Plugin.Log.LogFatal("Total without the complete function: " + stopwatch2.ElapsedTicks);
            // // Plugin.Log.LogFatal("Total of the multithreading: " + stopwatch3.ElapsedTicks);
            // times.Add(time);
            // if (times.Count>3)
            // {
            //     Plugin.Log.LogFatal("Average: " + times.GetRange(2, times.Count-3).Average());
            // }
            stopwatch1.Reset();
            stopwatch2.Reset();
            stopwatch3.Reset();
        }
        
        private readonly Dictionary<string, bool> _secondCalls = new();
        static readonly ConcurrentDictionary<string, object> LockObjects = new();
        private static readonly Dictionary<string, string> MethodGroups = new()
        {
            // { "HaulingCenter.GetWorkplaceBehaviorsOrdered", "Hauling" },
            // { "HaulingCenter.UpdateHaulCandidates", "Hauling" },
            // { "HaulingCenter.AddHaulCandidateInThisDistrict", "Hauling" },
            // { "HaulingCenter.RemoveHaulCandidate", "Hauling" },
            // { "HaulCandidate.GetWeightedBehaviors", "Hauling" },
            
            { "PlantBehavior.ReserveCoordinates", "Planting" },
            { "PlantingService.UnsetPlantingCoordinates", "Planting" },
            { "PlantingService.ReservePlantingCoordinates", "Planting" },
            { "PlantingService.UpdatePlantingSpot", "Planting" },

            { "Walker.FindPath", "Walker" },
            { "NavigationService.FindPathUnlimitedRange", "Walker" },

            // { "FlowFieldCache.GetFlowFieldAtNode", "FlowFieldCache" },
            // { "FlowFieldCache.TryGetFlowFieldAtNode", "FlowFieldCache" },
            // { "FlowFieldCache.OnNodesChanged", "FlowFieldCache" },
            //
            // { "TerrainFlowFieldCache.GetFlowFieldAtNode", "TerrainFlowFieldCache" },
            // { "TerrainFlowFieldCache.TryGetFlowFieldAtNode", "TerrainFlowFieldCache" },
            
            { "GoodReserver.ReserveStock", "Reserver" },
            { "GoodReserver.ReserveExactStockAmount", "Reserver" },
            { "InventoryNeedBehavior.Decide", "Reserver" },
            //
            // { "SleepNeedBehavior.GetEssentialAction", "Dwelling" },
            // { "Dweller.AssignToHome", "Dwelling" },
            // { "Dweller.UnassignFromHome", "Dwelling" },

        };


        private Stopwatch functionStopwatch = Stopwatch.StartNew();
        
        
        public Tuple<object, bool> LockedNonVoidFunction(object __instance, object[] __args, MethodBase __originalMethod)
        {
            // functionStopwatch.Start();
            
            var methodName = __originalMethod.ReflectedType.Name + "." + __originalMethod.Name;
            var lockObject = LockObjects.GetOrAdd(methodName, new object());
            

            if (MethodGroups.ContainsKey(methodName))
            {
                lockObject = LockObjects.GetOrAdd(MethodGroups[methodName], new object());
            }

            try
            {
                lock (lockObject)
            {

                if (!_secondCalls.ContainsKey(methodName))
                {
                    _secondCalls.Add(methodName, false);
                }
                if (_secondCalls[methodName])
                {
                    _secondCalls[methodName] = false;
                    return Tuple.Create(default(object), true);
                }
                _secondCalls[methodName] = true;

                var result = __originalMethod.Invoke(__instance, __args);
                
                // functionStopwatch.Stop();
                // var time = functionStopwatch.ElapsedTicks;
                // if (time > 50000)
                // {
                //     Plugin.Log.LogFatal(time);
                //     Plugin.Log.LogFatal(methodName);
                //     // foreach (var stackFrame in new StackTrace().GetFrames())
                //     // {
                //     //     Plugin.Log.LogInfo(stackFrame.GetMethod().ReflectedType.Name + "." + methodName);
                //     //
                //     // }
                // }
                // functionStopwatch.Reset();

                return Tuple.Create(result, false);
            }}
            finally
            {
                LockObjects.TryRemove(methodName, out _);
                // if (MethodGroups.ContainsKey(methodName))
                // {
                //     LockObjects.TryRemove(MethodGroups[methodName], out _);
                // }
            }
        }
        
        public bool LockedVoidFunction(object __instance, object[] __args, MethodBase __originalMethod)
        {
            // functionStopwatch.Start();
            
            var methodName = __originalMethod.ReflectedType.Name + "." + __originalMethod.Name;
            var lockObject = LockObjects.GetOrAdd(methodName, new object());

            if (MethodGroups.ContainsKey(methodName))
            {
                lockObject = LockObjects.GetOrAdd(MethodGroups[methodName], new object());
            }
            try{
                lock (lockObject)
                {
                    // if (__originalMethod.Name != "Decide")
                    // {
                    //     methodName = __originalMethod.Name;
                    // }
                
                    if (!_secondCalls.ContainsKey(methodName))
                    {
                        _secondCalls.Add(methodName, false);
                    }
                    if (_secondCalls[methodName])
                    {
                        _secondCalls[methodName] = false;
                        return true;
                    }
                    _secondCalls[methodName] = true;
                
                    __originalMethod.Invoke(__instance, __args);
                
                    // functionStopwatch.Stop();
                    // var time = functionStopwatch.ElapsedTicks;
                    // if (time > 50000)
                    // {
                    //     Plugin.Log.LogFatal(time);
                    //     Plugin.Log.LogFatal(methodName);
                    //     // foreach (var stackFrame in new StackTrace().GetFrames())
                    //     // {
                    //     //     Plugin.Log.LogInfo(stackFrame.GetMethod().ReflectedType.Name + "." + methodName);
                    //     //
                    //     // }
                    // }
                    // functionStopwatch.Reset();
                
                    return false;
                }}
            finally
            {
                LockObjects.TryRemove(methodName, out _);
                // if (MethodGroups.ContainsKey(methodName))
                // {
                //     LockObjects.TryRemove(MethodGroups[methodName], out _);
                // }
            }
        }
    }
}