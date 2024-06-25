using System.Collections.Generic;
using UnityEngine;

namespace GlobalMarket
{
  public interface IAirDestination
  {
    bool GeneratePath(Vector3 start, List<Vector3> pathCorners);
  }
}
