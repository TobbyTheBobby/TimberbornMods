using System;
using UnityEngine;

namespace ChooChoo
{
    public static class Vector3Extensions
    {
        public static Vector3Int ToBlockServicePosition(this Vector3 vector3)
        {
            return new Vector3Int((int)Math.Floor(vector3.x), (int)Math.Floor(vector3.z), (int)Math.Round(vector3.y));
        }
    }
}