using System.Collections.Generic;
using ChooChoo.TrackSystem;
using UnityEngine;

namespace ChooChoo.NavigationSystem
{
    public interface ITrainDestination
    {
        bool GeneratePath(Transform transform, List<TrackRoute> pathCorners);
    }
}