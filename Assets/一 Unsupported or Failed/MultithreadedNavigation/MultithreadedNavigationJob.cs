using Unity.Collections;
using Unity.Jobs;

namespace MultithreadedNavigation
{
    struct MultithreadedNavigationJob: IJobParallelFor
    {
        public NativeArray<MyBehaviorManager> MyBehaviorManagers;
        
        public void Execute(int index)
        {
            
            // Stopwatch stopwatch = Stopwatch.StartNew();
            // var data = MyBehaviorManagers[index];
            // var behaviorManager = data.BehaviorManager;   
            
            // try
            // {
            //     behaviorManager.Tick();
            //     
            // }
            // catch (Exception e)
            // {
            //     Console.WriteLine(e);
            // }
            // Plugin.Log.LogFatal("complete");
            // data.BehaviorManager = BehaviorManager;
            // MyBehaviorManagers[index] = data;
            
            // stopwatch.Stop();
            // var time = stopwatch.ElapsedTicks;
            // if (time > 20000)
            // {
            //     Plugin.Log.LogFatal(time);
            //     Plugin.Log.LogFatal(behaviorManager.name);
            //     Plugin.Log.LogFatal(behaviorManager.RunningBehavior.Name);
            // }
            // stopwatch.Reset();
        }
    }
}
