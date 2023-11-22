using System;
using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using Timberborn.Common;

namespace DifficultySettingsChanger
{
    public static class InaccessibilityUtilities
    {
        private static readonly Dictionary<string, FieldInfo> FieldInfos = new();
        private static readonly Dictionary<string, PropertyInfo> PropertyInfos = new();

        private const BindingFlags BindingFlags = System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Static;
        
        public static void SetInaccesibleProperty(object instance, string fieldName, object newValue)
        {
            var instanceType = instance.GetType();
            var instanceName = instanceType.Name;
            var key = $"{instanceName}.{fieldName}";
            var propertyInfo = PropertyInfos.GetOrAdd(key, () => AccessTools.TypeByName(instanceName).GetProperty(fieldName, BindingFlags));

            propertyInfo.SetValue(instance, newValue);
        }
        
        public static object GetInaccessibleField(object instance, string fieldName)
        {
            var instanceType = instance.GetType();
            var instanceName = instanceType.Name;
            var key = $"{instanceName}.{fieldName}";
            FieldInfos.TryAdd(key, instanceType.GetField(fieldName, BindingFlags));
            return FieldInfos[key].GetValue(instance);
        }
        
        public static object GetInaccessibleProperty(object instance, string propertyName)
        {
            var instanceName = instance.GetType().Name;
            var key = $"{instanceName}.{propertyName}";
            if (!PropertyInfos.ContainsKey(key))
            {
                PropertyInfos.Add(key, AccessTools.TypeByName(instanceName).GetProperty(propertyName, BindingFlags));
            }

            return PropertyInfos[key].GetValue(instance);
        }
        
        public static void SetInaccessibleField(object instance, string fieldName, object newValue)
        {
            var instanceType = instance.GetType();
            var instanceName = instanceType.Name;
            var key = $"{instanceName}.{fieldName}";
            FieldInfos.TryAdd(key, instanceType.GetField(key, BindingFlags));
            FieldInfos[key].SetValue(instance, newValue);
        }
    }
}