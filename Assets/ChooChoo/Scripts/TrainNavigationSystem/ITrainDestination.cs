using System.Collections.Generic;
using UnityEngine;

namespace ChooChoo
{
  public interface ITrainDestination
  {
    bool GeneratePath(Transform transform, List<TrackRoute> pathCorners);
  }
}
