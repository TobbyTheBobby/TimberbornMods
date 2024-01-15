using System;
using System.Collections.Generic;
using ChooChoo.Extensions;
using ChooChoo.PassengerSystem;
using Timberborn.BlockSystem;
using UnityEngine;

namespace ChooChoo.BeaverNavigationUtilities
{
    public class PathCorrector
    {
        private readonly BlockService _blockService;

        private PathCorrector(BlockService blockService)
        {
            _blockService = blockService;
        }

        public bool IsBetweenPassengerStations(ref List<Vector3> pathCorners, int startIndex, int endIndex)
        {
            var pathCorner1 = pathCorners[startIndex];
            var pathCorner2 = pathCorners[endIndex];

            if (!(Math.Abs(pathCorner1.y - pathCorner2.y) > 0.5))
                return false;

            // Plugin.Log.LogInfo("pathCorner1: " + pathCorner1 + " pathCorner2:" + pathCorner2);

            return IsPassengerStation(pathCorner1) || IsPassengerStation(pathCorner2);
        }

        private bool IsPassengerStation(Vector3 coordinates)
        {
            return _blockService.GetFloorObjectComponentAt<PassengerStation>(coordinates.ToBlockServicePosition());
            // return _blockService.GetFloorObjectComponentAt<PassengerStation>(Vector3Int.FloorToInt(coordinates));
        }
    }
}