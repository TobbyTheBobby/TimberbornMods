using System.Linq;
using Timberborn.SingletonSystem;
using Timberborn.WalkingSystemUI;
using UnityEngine;

namespace ChooChoo.Debugging
{
    public class DebuggingMarkers : ILoadableSingleton
    {
        public static GameObject WalkerGameObjectMarker { get; set; }
        public static GameObject WalkerModelMarker { get; set; }
        public static GameObject DestinationMarker { get; set; }
        public static GameObject CornerMarkerPrefab { get; set; }

        public void Load()
        {
            var walkerDebugger = Object.FindObjectsByType<WalkerDebugger>(FindObjectsInactive.Include, FindObjectsSortMode.None).First();

            WalkerGameObjectMarker = walkerDebugger._walkerGameObjectMarker;
            WalkerModelMarker = walkerDebugger._walkerModelMarker;
            DestinationMarker = walkerDebugger._destinationMarker;
            CornerMarkerPrefab = walkerDebugger._cornerMarkerPrefab;
        }
    }
}