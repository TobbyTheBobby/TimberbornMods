using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TimberApi.Common.SingletonSystem;
using TimberApi.SpecificationSystem;
using Timberborn.Common;
using ExpandoObject = System.Dynamic.ExpandoObject;

namespace DifficultySettingsChanger
{
    public class DynamicSpecificationApplier : IEarlyLoadableSingleton
    {
        private readonly DynamicSpecificationDeserializer _dynamicSpecificationDeserializer;
        private readonly ActiveComponentRetriever _activeComponentRetriever;
        private readonly IApiSpecificationService _apiSpecificationService;

        private readonly Dictionary<string, DynamicSpecification[]> _dynamicSpecifications = new();

        public DynamicSpecificationApplier(DynamicSpecificationDeserializer dynamicSpecificationDeserializer, ActiveComponentRetriever activeComponentRetriever, IApiSpecificationService apiSpecificationService)
        {
            _dynamicSpecificationDeserializer = dynamicSpecificationDeserializer;
            _activeComponentRetriever = activeComponentRetriever;
            _apiSpecificationService = apiSpecificationService;
        }

        public void EarlyLoad()
        {
            ProcessChanges();
        }

        private void ProcessChanges()
        {
            foreach (var group in _activeComponentRetriever.GetAllComponents().GroupBy(component => component.gameObject.name))
            {
                foreach (var component in group)
                {
                    var prefabName = component.gameObject.name;
                    var componentType = component.GetType();
                    var componentName = componentType.Name;

                    DynamicSpecification[] dynamicSpecifications;
                    
                    try
                    {
                        if (Plugin.LoggingEnabled)
                        {
                            if (componentType.Name == "Manufactory")
                            {
                                // foreach ((ISpecification, string) variable in _specificationJsons.GetOrAdd(componentName + "Specification", () => _apiSpecificationService.GetSpecificationJsons(componentName + "Specification")))
                                // {
                                //     if (variable.Item2.ToLower().Contains("grill"))
                                //     {
                                //         Plugin.Log.LogWarning(variable.Item2);
                                //     }
                                // }
                            }
                        }
                        

                        dynamicSpecifications = _dynamicSpecifications.GetOrAdd(componentName + "Specification", () => _apiSpecificationService.GetSpecifications(componentName + "Specification", _dynamicSpecificationDeserializer).ToArray());
                    }
                    catch (Exception)
                    {
                        continue;
                    }

                    if (!dynamicSpecifications.Any())
                        continue;

                    foreach (var dynamicSpecification in dynamicSpecifications)
                    {
                        if (dynamicSpecification.PrefabName != prefabName)
                            continue;

                        if (SkippableTypes.Types.Contains(componentType))
                            continue;

                        if (Plugin.LoggingEnabled) Plugin.Log.LogWarning("Name Component: " + componentType);

                        var fieldInfos = ReflectionUtils.GetSerializeableFieldInfos(componentType);
                        var dynamicProperties = DynamicPropertiesUtils.GetAll(fieldInfos, component).ToArray();

                        if (Plugin.LoggingEnabled) Plugin.Log.LogWarning("Number of fields: " + dynamicProperties.Count());
                        if (!dynamicProperties.Any())
                            continue;

                        ExpandoObject objectContaingValues = dynamicSpecification.SerializedObject.ToExpandoObject();

                        foreach (var property in dynamicProperties)
                        {
                            ApplyChanges(property, objectContaingValues, component,
                                objectContaingValues.GetPropertyValue(property.StyledName));
                        }
                    }
                }
            }
        }

        private void ApplyChanges(DynamicProperty property, ExpandoObject objectContaingValues, object objectToBeUpdated, object newValue)
        {
            if (newValue == null)
             return;
            
            var toBeUpdatedType = objectToBeUpdated.GetType();
            var toBeUpdateFieldInfo = toBeUpdatedType.GetField(property.OriginalName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            var toBeUpdatePropertyInfo = toBeUpdatedType.GetProperty(property.OriginalName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            
            if (Plugin.LoggingEnabled) Plugin.Log.LogError("New value: " + newValue);
            if (Plugin.LoggingEnabled) Plugin.Log.LogError("New value type: " + newValue.GetType());

            if (toBeUpdateFieldInfo != null && toBeUpdateFieldInfo.FieldType.IsArray || (toBeUpdatePropertyInfo != null && toBeUpdatePropertyInfo.PropertyType.IsArray))
            {
                ProcessArray(property, objectContaingValues, objectToBeUpdated, toBeUpdateFieldInfo, toBeUpdatePropertyInfo);
                return;
            }

            if ((toBeUpdateFieldInfo != null && IsStruct(toBeUpdateFieldInfo.FieldType)) || 
                (toBeUpdatePropertyInfo != null && IsStruct(toBeUpdatePropertyInfo.PropertyType)))
            {
                ProcessStruct(property, objectContaingValues, objectToBeUpdated, toBeUpdateFieldInfo, toBeUpdatePropertyInfo);
                return;
            }

            UpdateOppropriateValue(objectToBeUpdated, toBeUpdateFieldInfo, toBeUpdatePropertyInfo, newValue);
        }

        private void ProcessArray(DynamicProperty property,  ExpandoObject objectContaingValues, object objectToBeUpdated, FieldInfo toBeUpdateFieldInfo, PropertyInfo toBeUpdatePropertyInfo)
        {
            if (Plugin.LoggingEnabled) Plugin.Log.LogInfo("ARRAY");
            if (Plugin.LoggingEnabled) Plugin.Log.LogInfo(property.StyledName);
            var elementType = toBeUpdateFieldInfo != null ? toBeUpdateFieldInfo.FieldType.GetElementType() : toBeUpdatePropertyInfo.PropertyType.GetElementType();
            var dynamicProperties = DynamicPropertiesUtils.GetAllWithoutValues(ReflectionUtils.GetSerializeableFieldInfos(elementType));

            var list = new List<object>();
            
            if (elementType == typeof(string))
            {
                var valueList = objectContaingValues.GetValueList(property.StyledName);
            
                if (Plugin.LoggingEnabled) Plugin.Log.LogError("ADSDSASDAD00: " + valueList.Count());
            
                if (Plugin.LoggingEnabled) Plugin.Log.LogInfo(elementType + "");
                if (Plugin.LoggingEnabled) Plugin.Log.LogInfo(typeof(string) + "");

                foreach (var value in valueList)
                {
                    // var test = value as string;
                    if (Plugin.LoggingEnabled) Plugin.Log.LogInfo(value.GetType() + "");
                    if (Plugin.LoggingEnabled) Plugin.Log.LogError(value + "");
                }
                
                list.AddRange(valueList);
            }
            else
            {
                foreach (ExpandoObject item in objectContaingValues.GetExpandoObjectList(property.StyledName))
                {
                    CreateInstancedObject(elementType, out object newObject);
                    if (Plugin.LoggingEnabled) Plugin.Log.LogWarning(newObject.GetType() + "");
                    
                    foreach (var dynamicProperty in dynamicProperties)
                    {
                        if (Plugin.LoggingEnabled) Plugin.Log.LogWarning(dynamicProperty.StyledName);
                        if (Plugin.LoggingEnabled) Plugin.Log.LogWarning(item.GetPropertyValue(dynamicProperty.StyledName) + "");
                        ApplyChanges(dynamicProperty, objectContaingValues, newObject, item.GetPropertyValue(dynamicProperty.StyledName));
                    }
                    list.Add(newObject);
                }
            }

            var generatedArray = list.ToArray();
            if (Plugin.LoggingEnabled) Plugin.Log.LogInfo("generatedArray length: " + generatedArray.Length);
            foreach (var obj in generatedArray)
                if (Plugin.LoggingEnabled) Plugin.Log.LogInfo(obj + "");
            Array typedArray = Array.CreateInstance(elementType, generatedArray.Length);
            generatedArray.CopyTo(typedArray, 0);
            if (Plugin.LoggingEnabled) Plugin.Log.LogInfo("typedArray length: " + typedArray.Length + "");
            if (Plugin.LoggingEnabled) foreach (var obj in typedArray) Plugin.Log.LogInfo(obj + "");


            UpdateOppropriateValue(objectToBeUpdated, toBeUpdateFieldInfo, toBeUpdatePropertyInfo, typedArray);
        }

        private bool IsStruct(Type type)
        {
            return type.IsValueType && !type.IsPrimitive && !type.IsEnum;
        }

        private void CreateInstancedObject(Type type, out object newObject)
        {
            try
            {
                newObject = Activator.CreateInstance(type);
            }
            catch (Exception e)
            {
                if (Plugin.LoggingEnabled) Plugin.Log.LogError(e.ToString());
                        
                var constructor = type.GetConstructors().First();   
                    
                var parameters = constructor.GetParameters();
                var args = new object[parameters.Length];
                    
                for (int i = 0; i < parameters.Length; i++)
                {
                    var parameterType = parameters[i].ParameterType;
                    args[i] = parameterType.IsValueType ? Activator.CreateInstance(parameterType) : null;
                }
                    
                newObject = constructor.Invoke(args);
            }
        }

        private void ProcessStruct(DynamicProperty property,  ExpandoObject objectContaingValues, object objectToBeUpdated, FieldInfo toBeUpdateFieldInfo, PropertyInfo toBeUpdatePropertyInfo)
        {
            if (Plugin.LoggingEnabled) Plugin.Log.LogInfo("STRUCT");
            if (Plugin.LoggingEnabled) Plugin.Log.LogInfo(property.StyledName);

            var parentObjectContainingValues = (ExpandoObject)objectContaingValues.GetPropertyValue(property.StyledName);

            if (parentObjectContainingValues == null)
                return;

            var structInstance = toBeUpdateFieldInfo != null ? 
                Activator.CreateInstance(toBeUpdateFieldInfo.FieldType) : 
                Activator.CreateInstance(toBeUpdatePropertyInfo.PropertyType);
            
            var alldynamicProperties = toBeUpdateFieldInfo != null ?
                DynamicPropertiesUtils.GetAllWithoutValues(toBeUpdateFieldInfo.FieldType) :
                DynamicPropertiesUtils.GetAllWithoutValues(toBeUpdatePropertyInfo.PropertyType);

            if (Plugin.LoggingEnabled) Plugin.Log.LogWarning("Number of dynamic properties: " + alldynamicProperties.Count());
            if (Plugin.LoggingEnabled) Plugin.Log.LogWarning(structInstance.GetType() + "");
            foreach (var dynamicProperty in alldynamicProperties)
            {
                if (Plugin.LoggingEnabled) Plugin.Log.LogWarning(dynamicProperty.StyledName);
                var value = parentObjectContainingValues.GetPropertyValue(dynamicProperty.StyledName);
                if (Plugin.LoggingEnabled) Plugin.Log.LogInfo(value + "");
                if (value == null)
                {
                    value = parentObjectContainingValues.GetPropertyValue(dynamicProperty.OriginalName);
                }
                
                ApplyChanges(dynamicProperty, parentObjectContainingValues, structInstance, value);
            }

            UpdateOppropriateValue(objectToBeUpdated, toBeUpdateFieldInfo, toBeUpdatePropertyInfo, structInstance);
        }

        private void UpdateOppropriateValue(object objectToBeUpdated, FieldInfo toBeUpdateFieldInfo, PropertyInfo toBeUpdatePropertyInfo, object newValue)
        {
            if (Plugin.LoggingEnabled) Plugin.Log.LogInfo("New value: " + newValue);
            
            if (toBeUpdateFieldInfo != null)
            {
                var correctType = toBeUpdateFieldInfo.FieldType;
                if (newValue.GetType() != correctType)
                    newValue = Convert.ChangeType(newValue, correctType);
                toBeUpdateFieldInfo.SetValue(objectToBeUpdated, newValue);
                if (Plugin.LoggingEnabled) LogCurrentValue(toBeUpdateFieldInfo.GetValue(objectToBeUpdated));
                return;
            }
            if (toBeUpdatePropertyInfo != null && toBeUpdatePropertyInfo.GetSetMethod() != null)
            {
                var correctType = toBeUpdatePropertyInfo.PropertyType;
                if (newValue.GetType() != correctType)
                    newValue = Convert.ChangeType(newValue, correctType);
                toBeUpdatePropertyInfo.SetValue(objectToBeUpdated, newValue);
                if (Plugin.LoggingEnabled) LogCurrentValue(toBeUpdatePropertyInfo.GetValue(objectToBeUpdated));
            }
        }

        private void LogCurrentValue(object value)
        {
            if (value is Array array)
                foreach (var valueItem in array)
                    Plugin.Log.LogInfo("Current value item: " + valueItem);
            else
                Plugin.Log.LogInfo("Current value: " + value);
        }
    }
}