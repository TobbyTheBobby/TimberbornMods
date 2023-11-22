using System.Collections.Generic;
using Timberborn.BlockSystem;
using Timberborn.Common;
using Timberborn.Navigation;
using UnityEngine;

namespace ChooChoo
{
    public class PathCornerBlockObjectRepository : INavMeshListener
    {
        private readonly BlockService _blockService;
        
        private readonly Dictionary<Vector3, BlockObject> _blockObjects = new();

        PathCornerBlockObjectRepository(BlockService blockService)
        {
            _blockService = blockService;
        }
        
        public BlockObject Get(Vector3 coordinates)
        {
            return _blockObjects.GetOrAdd(coordinates, () => _blockService.GetFloorObjectAt(coordinates.ToBlockServicePosition()));
        }

        public void OnNavMeshUpdated(NavMeshUpdate navMeshUpdate)
        {
            _blockObjects.Clear();
        }
    }
}