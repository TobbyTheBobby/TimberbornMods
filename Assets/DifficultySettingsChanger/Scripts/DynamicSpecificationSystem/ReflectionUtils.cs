using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using UnityEngine;

namespace DifficultySettingsChanger
{
    public static class ReflectionUtils
    {
        private static readonly BindingFlags BindingFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static;
        
        public static IEnumerable<FieldInfo> GetSerializeableFieldInfos(Type type)
        {
            return type
                .GetFields(BindingFlags)
                .Where(field => field.IsDefined(typeof(SerializeField), true))
                .Where(IsAllowedFieldType);
        }

        public static IEnumerable<FieldInfo> GetFieldInfos(Type type)
        {
            return type?
                .GetFields(BindingFlags)
                .Where(IsAllowedFieldType);
        }
        
        public static IEnumerable<PropertyInfo> GetPropertyInfos([CanBeNull] Type type)
        {
            return type?
                .GetProperties(BindingFlags);
        }
        
        public static bool IsAllowedFieldType(FieldInfo fieldInfo)
        {
            return !UnallowedTypes.Types.Contains(fieldInfo.FieldType);
        }
        
        public static bool IsAllowedFieldName(FieldInfo fieldInfo)
        {
            return !UnallowedFieldNames.Types.Contains(fieldInfo.Name);
        }
    }
}