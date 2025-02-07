using Timberborn.SingletonSystem;
using Timberborn.WalkingSystemUI;
using UnityEngine;

namespace ChooChoo.Debugging
{
    public class StaticWalkerDebugger : ILoadableSingleton
    {
        public static GameObject WalkerGameObjectMarker { get; private set; }
        public static GameObject WalkerModelMarker { get; private set; }
        public static GameObject DestinationMarker { get; private set; }
        public static GameObject CornerMarkerPrefab { get; private set; }

        private readonly WalkerDebugger _walkerDebugger;

        public StaticWalkerDebugger(WalkerDebugger walkerDebugger)
        {
            _walkerDebugger = walkerDebugger;
        }

        public void Load()
        {
            WalkerGameObjectMarker = _walkerDebugger._walkerGameObjectMarker;
            WalkerModelMarker = _walkerDebugger._walkerModelMarker;
            DestinationMarker = _walkerDebugger._destinationMarker;
            CornerMarkerPrefab = _walkerDebugger._cornerMarkerPrefab;
        }
    }
}