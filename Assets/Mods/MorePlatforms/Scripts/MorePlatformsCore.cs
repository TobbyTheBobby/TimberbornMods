using System.Collections.Generic;
using UnityEngine;

namespace MorePlatforms
{
    public static class MorePlatformsCore
    {
        public static Transform FindBodyPart(Transform parent, string bodyPartName)
        {
            foreach (Transform child in parent)
            {
                if (child.name == bodyPartName)
                    return child;
                
                var result = FindBodyPart(child, bodyPartName);
                if (result != null)
                    return result;
            }

            return null;
        }
        
        public static List<Transform> FindAllBodyParts(Transform parent, string bodyPartName)
        {
            var list = new List<Transform>();
            foreach (Transform child in parent)
            {
                if (child.name == bodyPartName)
                    list.Add(child);
                
                FindBodyPart(child, bodyPartName);
            }

            return list;
        }
    }
}