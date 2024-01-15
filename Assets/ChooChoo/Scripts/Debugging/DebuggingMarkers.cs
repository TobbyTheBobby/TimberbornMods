using System.Linq;
using Timberborn.SingletonSystem;
using Timberborn.WalkingSystemUI;
using TobbyTools.InaccessibilityUtilitySystem;
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

            WalkerGameObjectMarker = (GameObject)InaccessibilityUtilities.GetInaccessibleField(walkerDebugger, "_walkerGameObjectMarker");
            WalkerModelMarker = (GameObject)InaccessibilityUtilities.GetInaccessibleField(walkerDebugger, "_walkerModelMarker");
            DestinationMarker = (GameObject)InaccessibilityUtilities.GetInaccessibleField(walkerDebugger, "_destinationMarker");
            CornerMarkerPrefab = (GameObject)InaccessibilityUtilities.GetInaccessibleField(walkerDebugger, "_cornerMarkerPrefab");
        }
    }
}