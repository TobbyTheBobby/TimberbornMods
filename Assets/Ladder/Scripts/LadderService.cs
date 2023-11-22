using System;
using System.Collections.Generic;
using System.Diagnostics;
using Timberborn.BlockSystem;
using Timberborn.EntitySystem;
using Timberborn.Navigation;
using Timberborn.PrefabSystem;
using Timberborn.SingletonSystem;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Ladder
{
    public class LadderService : ILoadableSingleton
    {

        private EventBus _eventBus;

        private readonly HashSet<Vector3Int> _verticalObjectsList = new ();

        LadderService(EventBus eventBus)
        {
            _eventBus = eventBus;
        }

        public void Load()
        {
            _eventBus.Register( this);
        }
        
        public bool ChangeVertical(ref List<Vector3> pathCorners, int startIndex, int endIndex)
        { 
            Vector3 pathCorner1 = pathCorners[startIndex];
            Vector3 pathCorner2 = pathCorners[endIndex];

            if (!(Math.Abs(pathCorner1.y - pathCorner2.y) > 0.5))
                return false;

            pathCorner1 += new Vector3(0, (pathCorner2.y - pathCorner1.y) / 2, 0);
            
            // pathCorners.Clear();
            // pathCorners.Add(new Vector3Int(10, 10, 10));
            // pathCorners.Add(new Vector3Int(9, 10, 10));

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

        public bool IsLadder(Vector3 coordinates)
        {
            Vector3Int checkCoordinates = new Vector3Int
            {
                x = Convert.ToInt32(Math.Floor(coordinates.x)),
                y = Convert.ToInt32(Math.Floor(coordinates.z)),
                z = Convert.ToInt32(Math.Floor(coordinates.y))
            };

            // foreach (var vector3Int in _verticalObjectsList)
            // {
            //     Plugin.Log.LogFatal(vector3Int);
            // }
            //
            // Plugin.Log.LogInfo(checkCoordinates);
            
            return !_verticalObjectsList.Contains(checkCoordinates);
        }
    }
}
