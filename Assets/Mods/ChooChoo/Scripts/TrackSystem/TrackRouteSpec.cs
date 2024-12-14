using System;
using System.Collections.Generic;
using UnityEngine;

namespace ChooChoo.TrackSystem
{
    [Serializable]
    public class TrackRouteSpec
    {
        [SerializeField]
        private TrackConnectionSpec _entrance;
        [SerializeField]
        private TrackConnectionSpec _exit;
        [SerializeField]
        private List<Vector3> _routeCorners;
        
        public TrackConnectionSpec Entrance => _entrance;
        public TrackConnectionSpec Exit => _exit;
        public List<Vector3> RouteCorners => _routeCorners;
    }
}