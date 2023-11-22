using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace DynamicSpecifications
{
    public class DynamicPropertyObtainer
    {
        public List<DynamicProperty> FromComponent(Type type, object instance)
        {
            return type
                .GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                .Where(field => field.IsDefined(typeof(SerializeField), true))
                .Where(ReflectionUtils.IsAllowedFieldType)
                .Select(field => new DynamicProperty(field.Name, field.GetValue(instance)))
                .ToList();
        }
        
        public List<DynamicProperty> FromSingleton(Type type, object instance)
        {
            return type
                .GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                // .Where(property => !property.CanWrite)
                .Select(property => new DynamicProperty(property.Name, property.GetValue(instance)))
                .ToList();
        }
    }
}