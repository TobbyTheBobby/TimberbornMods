using System.Collections.Generic;
using System.Reflection;

namespace TobbyTools.InaccessibilityUtilitySystem
{
    public static class InaccessibilityUtilities
    {
        private static readonly Dictionary<string, FieldInfo> FieldInfos = new();
        private static readonly Dictionary<string, MethodInfo> MethodInfos = new();
        private static readonly Dictionary<string, PropertyInfo> PropertyInfos = new();

        private const BindingFlags BindingFlags = System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Static;

        public static object GetInaccessibleField(object instance, string fieldName)
        {
            return GetOrCreateFieldInfo(instance, fieldName).GetValue(instance);
        }
        
        public static void SetInaccessibleField(object instance, string fieldName, object newValue)
        {
            GetOrCreateFieldInfo(instance, fieldName).SetValue(instance, newValue);
        }
        
        public static object InvokeInaccessibleMethod(object instance, string methodName, object[] args = null)
        {
            var instanceType = instance.GetType();
            var instanceName = instanceType.Name;
            var key = $"{instanceName}.{methodName}";
            MethodInfos.TryAdd(key, instanceType.GetMethod(methodName, BindingFlags));
            return MethodInfos[key].Invoke(instance, args);
        }
        
        public static object GetInaccessibleProperty(object instance, string propertyName)
        {
            return GetOrCreatePropertyInfo(instance, propertyName).GetValue(instance);
        }
        
        public static void SetInaccessibleProperty(object instance, string propertyName, object newValue)
        {
            GetOrCreatePropertyInfo(instance, propertyName).SetValue(instance, newValue);
        }
        
        private static FieldInfo GetOrCreateFieldInfo(object instance, string fieldName)
        {
            var instanceType = instance.GetType();
            var instanceName = instanceType.Name;
            var key = $"{instanceName}.{fieldName}";
            if (FieldInfos.TryGetValue(key, out var fieldInfo)) 
                return fieldInfo;
            fieldInfo = instanceType.GetField(fieldName, BindingFlags);
            FieldInfos.TryAdd(key, fieldInfo);
            return fieldInfo;
        }
        
        private static PropertyInfo GetOrCreatePropertyInfo(object instance, string propertyName)
        {
            var instanceType = instance.GetType();
            var instanceName = instanceType.Name;
            var key = $"{instanceName}.{propertyName}";
            if (PropertyInfos.TryGetValue(key, out var fieldInfo)) 
                return fieldInfo;
            fieldInfo = instanceType.GetProperty(propertyName, BindingFlags);
            PropertyInfos.TryAdd(key, fieldInfo);
            return fieldInfo;
        }
    }
}