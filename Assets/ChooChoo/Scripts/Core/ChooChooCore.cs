using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using Timberborn.Common;
using UnityEngine;

namespace ChooChoo
{
    public static class ChooChooCore
    {
        private static readonly Dictionary<string, FieldInfo> _fieldInfos = new();
        private static readonly Dictionary<string, MethodInfo> _methodInfos = new();
        private static readonly Dictionary<string, PropertyInfo> _propertyInfos = new();

        private static BindingFlags _bindingFlags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static;

        public static void SetInaccesibleProperty(object instance, string propertyName, object newValue)
        {
            var propertyInfo = _propertyInfos.GetOrAdd(propertyName, () => AccessTools.TypeByName(instance.GetType().Name).GetProperty(propertyName, _bindingFlags));

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
                _methodInfos.Add(methodName, AccessTools.TypeByName(instance.GetType().Name).GetMethod(methodName, _bindingFlags));
            }
            
            return _methodInfos[methodName].Invoke(instance, args);
        }

        public static void SetInaccessibleField(object instance, string fieldName, object newValue)
        {
            if (!_fieldInfos.ContainsKey(fieldName))
            {
                _fieldInfos.Add(fieldName, AccessTools.TypeByName(instance.GetType().Name).GetField(fieldName, _bindingFlags));
            }
            
            _fieldInfos[fieldName].SetValue(instance, newValue);
        }

        public static object GetInaccessibleField(object instance, string fieldName)
        {
            if (!_fieldInfos.ContainsKey(fieldName))
            {
                _fieldInfos.Add(fieldName, AccessTools.TypeByName(instance.GetType().Name).GetField(fieldName, _bindingFlags));
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