using UnityEngine;

namespace ChooChoo
{
    public class TrackRoute
    {
        public TrackRoute(
            TrackConnection entrance, 
            TrackConnection exit, 
            Vector3[] routeCorners)
        {
            Entrance = entrance;
            Exit = exit;
            RouteCorners = routeCorners;
        }

        public readonly TrackConnection Entrance;

        public readonly TrackConnection Exit;
        
        public Vector3[] RouteCorners { get; set; }
    }
}