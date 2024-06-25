using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using Timberborn.Common;
using UnityEngine;

namespace MorePlatforms
{
    public static class MorePlatformsCore
    {
        private static readonly Dictionary<string, FieldInfo> _fieldInfos = new();
        private static readonly Dictionary<string, MethodInfo> _methodInfos = new();
        private static readonly Dictionary<string, PropertyInfo> _propertyInfos = new();
        
        public static object GetPublicProperty(object instance, string fieldName)
        {
            var propertyInfo = _propertyInfos.GetOrAdd(fieldName, () => AccessTools.TypeByName(instance.GetType().Name).GetProperty(fieldName,BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance));

            return propertyInfo.GetValue(instance);
        }
        
        public static void SetPrivateProperty(object instance, string fieldName, object newValue)
        {
            var propertyInfo = _propertyInfos.GetOrAdd(fieldName, () => AccessTools.TypeByName(instance.GetType().Name).GetProperty(fieldName,BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance));

            propertyInfo.SetValue(instance, newValue);
        }
        
        public static object InvokePublicMethod(object instance, string methodName, object[] args)
        {
            if (!_methodInfos.ContainsKey(methodName))
            {
                _methodInfos.Add(methodName, AccessTools.TypeByName(instance.GetType().Name).GetMethod(methodName, BindingFlags.Public | BindingFlags.Instance));
            }
            
            return _methodInfos[methodName].Invoke(instance, args);
        }
        
        public static object InvokePrivateMethod(object instance, string methodName, object[] args = null)
        {
            if (!_methodInfos.ContainsKey(methodName))
            {
                _methodInfos.Add(methodName, AccessTools.TypeByName(instance.GetType().Name).GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance));
            }
            
            return _methodInfos[methodName].Invoke(instance, args);
        }

        public static void SetInaccessibleField(object instance, string fieldName, object newValue)
        {
            if (!_fieldInfos.ContainsKey(fieldName))
            {
                _fieldInfos.Add(fieldName, AccessTools.TypeByName(instance.GetType().Name).GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance));
            }
            
            _fieldInfos[fieldName].SetValue(instance, newValue);
        }

        public static object GetInaccessibleField(object instance, string fieldName)
        {
            if (!_fieldInfos.ContainsKey(fieldName))
            {
                _fieldInfos.Add(fieldName, AccessTools.TypeByName(instance.GetType().Name).GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance));
            }
            
            return _fieldInfos[fieldName].GetValue(instance);
        }
        
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