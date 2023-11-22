// using System.Collections.Generic;
// using System.Linq;
// using Timberborn.PrefabSystem;
// using UnityEngine;
//
// namespace MorePaths
// {
//     public class PathObjectCollection : IObjectCollection
//     {
//         private readonly MorePathsCore _morePathsCore;
//         
//         PathObjectCollection(MorePathsCore morePathsCore)
//         {
//             _morePathsCore = morePathsCore;
//         }
//         
//         public IEnumerable<Object> GetObjects()
//         {
//             return _morePathsCore.CustomPaths.Select(path => path.GameObjectFast);
//         }
//     }
// }