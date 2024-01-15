using System.Collections.Generic;
using System.Linq;
using MorePaths.Core;
using Timberborn.PrefabSystem;
using UnityEngine;

namespace MorePaths
{
    public class CustomPathsObjectCollection : IObjectCollection
    {
        private readonly MorePathsCore _morePathsCore;

        CustomPathsObjectCollection(MorePathsCore morePathsCore)
        {
            _morePathsCore = morePathsCore;
        }
        
        public IEnumerable<GameObject> GetObjects()
        {
            return _morePathsCore.CustomPaths.Select(path => path.GameObjectFast);
        }
    }
}