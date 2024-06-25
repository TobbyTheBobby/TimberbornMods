using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Timberborn.PrefabSystem;
using Timberborn.SingletonSystem;
using UnityEngine;

namespace DynamicSpecifications
{
    public class DynamicSpecificationGenerator
    {
        private readonly ActiveComponentRetriever _activeComponentRetriever;
        private readonly DynamicPropertyObtainer _dynamicPropertyObtainer;
        private readonly ISingletonRepository _singletonRepository;

        public DynamicSpecificationGenerator(ActiveComponentRetriever activeComponentRetriever, DynamicPropertyObtainer dynamicPropertyObtainer, ISingletonRepository singletonRepository)
        {
            _activeComponentRetriever = activeComponentRetriever;
            _dynamicPropertyObtainer = dynamicPropertyObtainer;
            _singletonRepository = singletonRepository;
        }

        public void GenerateAllSpecifications()
        {
            Plugin.Log.LogInfo("Generation of Dynamic specifications started. Please wait until finished. This can sometimes take several minutes. If it takes longer than 10 minutes, please report.");
            var stopwatch = Stopwatch.StartNew();
            
            var generatedLocation = Path.Combine(Plugin.Mod.DirectoryPath, "GeneratedSpecifications");
            if (!Directory.Exists(generatedLocation)) 
                Directory.CreateDirectory(generatedLocation);

            foreach (var component in _activeComponentRetriever.GetAllComponents())
            {
                GenerateComponentSpecification(component, generatedLocation, true);
                GenerateComponentSpecification(component, generatedLocation, false);
            }

            foreach (var singleton in _singletonRepository.GetSingletons<object>())
            {
                GenerateSingletonSpecification(singleton, generatedLocation, true);
                GenerateSingletonSpecification(singleton, generatedLocation, false);
            }
            
            stopwatch.Stop();
            Plugin.Log.LogInfo($"Finished generation of Dynamic specifications in {stopwatch.Elapsed.Seconds} seconds. You can find them in the '<mods folder>/DynamicSpecifications/GeneratedSpecifications.'");
        }

        private void GenerateComponentSpecification(Component component, string generatedLocation, bool withValues)
        {
            var type = component.GetType();
            if (SkippableTypes.Types.Contains(type))
                return;
            
            var gameObjectName = component.gameObject.TryGetComponent(out Prefab prefab) ? prefab.PrefabName : component.gameObject.name;
            var fileName = type.Name + "Specification." + gameObjectName + (withValues ? "" : ".original") + ".json";
            var filePath = Path.Combine(generatedLocation, fileName);
            if (File.Exists(filePath))
                return;

            if (Plugin.LoggingEnabled) Plugin.Log.LogWarning(type + "");
            if (Plugin.LoggingEnabled) Plugin.Log.LogWarning("Name Component: " + type);
            
            var values = _dynamicPropertyObtainer.FromComponent(type, component);

            if (!values.Any())
                return;

            if (!withValues)
            {
                values.Clear();
                values.Insert(0, new DynamicProperty("_prefabName", gameObjectName));
            }

            if (Plugin.LoggingEnabled) Plugin.Log.LogWarning("Number of fields: " + values.Count);
            
            var newObject = DynamicClassFactory.CreateNewObject(values);
            var newObjectType = newObject.GetType();
            foreach (var property in values)
            {
                newObjectType.GetProperty(property.StyledName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)?.SetValue(newObject, property.Value);
            }

            try
            {
                File.WriteAllText(filePath, JsonConvert.SerializeObject(newObject, Formatting.Indented));
                if (Plugin.LoggingEnabled) Plugin.Log.LogInfo("Written");
            }
            catch (JsonSerializationException e)
            {
                // skipped
                if (Plugin.LoggingEnabled) Plugin.Log.LogError($"Cannot create {fileName}");
            }
        }
        
        private void GenerateSingletonSpecification(object singleton, string generatedLocation, bool withValues)
        {
            var type = singleton.GetType();

            if (SkippableTypes.Types.Contains(type))
                return;

            var typeName = type.Name;
            
            var fileName = typeName + "Specification" + (withValues ? "" : ".original") + ".json";
            
            var filePath = Path.Combine(generatedLocation, fileName);

            if (File.Exists(filePath))
                return;

            if (Plugin.LoggingEnabled) Plugin.Log.LogWarning(type + "");
            if (Plugin.LoggingEnabled) Plugin.Log.LogWarning("Name Singleton: " + type);
            
            var values = _dynamicPropertyObtainer.FromSingleton(type, singleton);

            if (!values.Any())
                return;

            if (!withValues)
            {
                values.Clear();
                values.Insert(0, new DynamicProperty("_prefabName", typeName));
            }

            if (Plugin.LoggingEnabled) Plugin.Log.LogWarning("Number of fields: " + values.Count);

            
            object newObject;
            try
            {
                newObject = DynamicClassFactory.CreateNewObject(values);
            }
            catch (Exception e)
            {
                if (Plugin.LoggingEnabled) Plugin.Log.LogError($"InvalidOperationException {fileName}");
                return;
            }
            var newObjectType = newObject.GetType();
            foreach (var property in values)
            {
                newObjectType.GetProperty(property.StyledName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)?.SetValue(newObject, property.Value);
            }

            try
            {
                File.WriteAllText(filePath, JsonConvert.SerializeObject(newObject, Formatting.Indented));
                if (Plugin.LoggingEnabled) Plugin.Log.LogInfo("Written");
            }
            catch (JsonSerializationException e)
            {
                // skipped
                if (Plugin.LoggingEnabled) Plugin.Log.LogError($"Cannot create {fileName}");
            }
            catch (InvalidOperationException e)
            {
                // skipped
                if (Plugin.LoggingEnabled) Plugin.Log.LogError($"InvalidOperationException {fileName}");
            }
        }
    }
}