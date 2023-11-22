using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using Timberborn.Common;

namespace TobbyTools.InaccessibilityUtilitySystem
{
    public static class InaccessibilityUtilities
    {
        private static readonly Dictionary<string, FieldInfo> FieldInfos = new();
        private static readonly Dictionary<string, MethodInfo> MethodInfos = new();
        private static readonly Dictionary<string, PropertyInfo> PropertyInfos = new();

        private const BindingFlags BindingFlags = System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Static;

        public static object InvokeInaccesableMethod(object instance, string methodName, object[] args = null)
        {
            var instanceName = instance.GetType().Name;
            return MethodInfos.GetOrAdd(
                $"{instanceName}.{methodName}", 
                () => AccessTools.TypeByName(instanceName).GetMethod(methodName, BindingFlags)).Invoke(instance, args);
        }
        
        public static void SetInaccesibleProperty(object instance, string fieldName, object newValue)
        {
            var instanceName = instance.GetType().Name;
            var key = $"{instanceName}.{fieldName}";
            var propertyInfo = PropertyInfos.GetOrAdd(key, () => AccessTools.TypeByName(instance.GetType().Name).GetProperty(fieldName, BindingFlags));

            propertyInfo.SetValue(instance, newValue);
        }
        
        public static object GetInaccessibleField(object instance, string fieldName)
        {
            var instanceName = instance.GetType().Name;
            var key = $"{instanceName}.{fieldName}";
            if (!FieldInfos.ContainsKey(key))
            {
                FieldInfos.Add(key, AccessTools.TypeByName(instanceName).GetField(fieldName, BindingFlags));
            }

            return FieldInfos[key].GetValue(instance);
        }
        
        public static void SetInaccessibleField(object instance, string fieldName, object newValue)
        {
            if (!FieldInfos.ContainsKey(fieldName))
            {
                FieldInfos.Add(fieldName, AccessTools.TypeByName(instance.GetType().Name).GetField(fieldName, BindingFlags));
            }

            FieldInfos[fieldName].SetValue(instance, newValue);
        }
    }
}