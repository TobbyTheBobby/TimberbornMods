using System;
using System.Collections.Generic;
using Timberborn.BlockSystem;
using Timberborn.PrefabSystem;
using Timberborn.SingletonSystem;
using UnityEngine;

namespace Ladder
{
    public class LadderService : ILoadableSingleton
    {
        public static LadderService Instance;

        private readonly EventBus _eventBus;

        private readonly HashSet<Vector3Int> _verticalObjectsList = new ();

        private LadderService(EventBus eventBus)
        {
            Instance = this;
            _eventBus = eventBus;
        }

        public void Load()
        {
            _eventBus.Register( this);
        }
        
        public bool ChangeVertical(ref List<Vector3> pathCorners, int startIndex, int endIndex)
        { 
            var pathCorner1 = pathCorners[startIndex];
            var pathCorner2 = pathCorners[endIndex];

            if (!(Math.Abs(pathCorner1.y - pathCorner2.y) > 0.5))
                return false;

            pathCorner1 += new Vector3(0, (pathCorner2.y - pathCorner1.y) / 2, 0);

            return IsLadder(pathCorner1);
        }

        [OnEvent]
        public void OnBlockObjectSet(BlockObjectSetEvent blockObjectSetEvent)
        {
            if (blockObjectSetEvent.BlockObject.GetComponentFast<Prefab>() == null)
                return;
            
            if (blockObjectSetEvent.BlockObject.GetComponentFast<Prefab>().PrefabName.ToLower().Contains("ladder"))
            {
                var coordinate = blockObjectSetEvent.BlockObject.Coordinates;
                _verticalObjectsList.Add(coordinate);
            }
        }
        
        [OnEvent]
        public void OnBlockObjectUnset(BlockObjectUnsetEvent blockObjectUnsetEvent)
        {
            if (blockObjectUnsetEvent.BlockObject == null)
                return;
            
            var coordinate = blockObjectUnsetEvent.BlockObject.Coordinates;
            _verticalObjectsList.Remove(coordinate);
        }

        private bool IsLadder(Vector3 coordinates)
        {
            var checkCoordinates = new Vector3Int
            {
                x = Convert.ToInt32(Math.Floor(coordinates.x)),
                y = Convert.ToInt32(Math.Floor(coordinates.z)),
                z = Convert.ToInt32(Math.Floor(coordinates.y))
            };
            
            return !_verticalObjectsList.Contains(checkCoordinates);
        }
    }
}
