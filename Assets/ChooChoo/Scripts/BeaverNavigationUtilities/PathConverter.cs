using System.Collections.Generic;
using Timberborn.BlockSystem;
using UnityEngine;

namespace ChooChoo
{
    public class PathConverter
    {
        private readonly PathCornerBlockObjectRepository _pathCornerBlockObjectRepository;
        
        PathConverter(PathCornerBlockObjectRepository pathCornerBlockObjectRepository)
        {
            _pathCornerBlockObjectRepository = pathCornerBlockObjectRepository;
        }
        
        public BlockObject[] ConvertPath(IReadOnlyList<Vector3> pathCorners)
        {      
            // Plugin.Log.LogInfo("Converting Path");
            int index = 0;
            var convertedPath = new List<BlockObject>();
            foreach (var pathCorner in pathCorners)
            {
                var blockObject = _pathCornerBlockObjectRepository.Get(pathCorner);
                // Plugin.Log.LogInfo(index + "   " +pathCorner + "   " + blockObject);
                convertedPath.Add(blockObject);
                index++;
            }

            return convertedPath.ToArray();
        }
    }
}