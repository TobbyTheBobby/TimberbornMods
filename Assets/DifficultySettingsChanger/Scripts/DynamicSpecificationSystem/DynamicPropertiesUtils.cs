using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DifficultySettingsChanger
{
    public static class DynamicPropertiesUtils
    {
        public static IEnumerable<DynamicProperty> GetAll(IEnumerable<FieldInfo> fieldInfos, object instance)
        {
            return fieldInfos.Select(field => new DynamicProperty(field.Name, field.GetValue(instance)));
        }
        
        public static IEnumerable<DynamicProperty> GetAllWithoutValues(Type type)
        {
            var a = GetAllWithoutValues(ReflectionUtils.GetFieldInfos(type));
            var b = GetAllWithoutValues(ReflectionUtils.GetPropertyInfos(type));
            
            return a.Concat(b);
        }
        
        public static IEnumerable<DynamicProperty> GetAllWithoutValues(IEnumerable<FieldInfo> fieldInfos)
        {
            return fieldInfos.Select(field => new DynamicProperty(field.Name));
        }
        
        public static IEnumerable<DynamicProperty> GetAllWithoutValues(IEnumerable<PropertyInfo> fieldInfos)
        {
            return fieldInfos.Select(field => new DynamicProperty(field.Name));
        }
    }
}