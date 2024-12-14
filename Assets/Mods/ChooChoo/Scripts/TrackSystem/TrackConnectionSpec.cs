using System;
using Timberborn.Coordinates;
using UnityEngine;

namespace ChooChoo.TrackSystem
{
    [Serializable]
    public class TrackConnectionSpec
    {
        [SerializeField]
        private Vector3Int _coordinatesKey;
        [SerializeField]
        private Direction2D _directionKey;
        
        public Vector3Int CoordinatesKey => _coordinatesKey;
        public Direction2D DirectionKey => _directionKey;
    }
}