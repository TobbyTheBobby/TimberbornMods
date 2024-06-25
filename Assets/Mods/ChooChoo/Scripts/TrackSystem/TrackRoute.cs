using System.Linq;
using UnityEngine;

namespace ChooChoo.TrackSystem
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

        public TrackRoute CreateCopy()
        {
            return new TrackRoute(Entrance.CreateCopy(), Exit.CreateCopy(), RouteCorners.ToArray());
        }
    }
}