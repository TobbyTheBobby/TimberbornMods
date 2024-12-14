using System.Collections.Generic;
using UnityEngine;

namespace MorePaths.CustomPaths
{
    public class CustomPathsRepository
    {
        public readonly Dictionary<string, GameObject> CustomPaths = new();

        public void Add(CustomPath customPath)
        {
            CustomPaths.TryAdd(customPath.PathSpecification.Name.ToLower(), customPath.GameObjectFast);
        }
    }
}