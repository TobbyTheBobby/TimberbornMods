using Timberborn.BehaviorSystem;

namespace MultithreadedNavigation
{
    public struct MyBehaviorManager
    {
        public readonly BehaviorManager BehaviorManager;

        public MyBehaviorManager(BehaviorManager behaviorManager)
        {
            BehaviorManager = behaviorManager;
        }
    }
}

