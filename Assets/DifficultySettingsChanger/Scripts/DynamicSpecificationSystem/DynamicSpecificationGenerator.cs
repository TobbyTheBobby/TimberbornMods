using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Timberborn.SingletonSystem;
using UnityEngine;

namespace DifficultySettingsChanger
{
    public class DynamicSpecificationGenerator : IGameValueChangerGenerator
    {
        private readonly ActiveComponentRetriever _activeComponentRetriever;
        private readonly DynamicPropertyObtainer _dynamicPropertyObtainer;
        private readonly ISingletonRepository _singletonRepository;

        private Stopwatch Stopwatch = new Stopwatch();

        public DynamicSpecificationGenerator(ActiveComponentRetriever activeComponentRetriever, DynamicPropertyObtainer dynamicPropertyObtainer, ISingletonRepository singletonRepository)
        {
            _activeComponentRetriever = activeComponentRetriever;
            _dynamicPropertyObtainer = dynamicPropertyObtainer;
            _singletonRepository = singletonRepository;
        }
        
        public IEnumerable<GameValueChanger> Generate()
        {
            // Plugin.Log.LogInfo("Generation of Dynamic specifications started. Please wait until finished. This can sometimes take several minutes. If it takes longer than 10 minutes, please report.");           
            var stopwatch = Stopwatch.StartNew();
            foreach (var component in _activeComponentRetriever.GetAllComponents())
            {
                var gamevalueChangers = GenerateComponentSpecification(component).ToArray();
                
                if (!gamevalueChangers.Any())
                    continue;
            
                foreach (var gamevalueChanger in gamevalueChangers)
                    yield return gamevalueChanger;
            }

            // foreach (var singleton in _singletonRepository.GetSingletons<object>())
            // {
            //     Plugin.Log.LogError(singleton.GetType().Name);
            //     
            //     var gamevalueChangers = GenerateSingletonSpecification(singleton).ToArray();
            //     
            //     if (!gamevalueChangers.Any())
            //         continue;
            //
            //     foreach (var gamevalueChanger in gamevalueChangers)
            //         yield return gamevalueChanger;
            // }

            stopwatch.Stop();
            Plugin.Log.LogWarning($"Finished generation of Dynamic Values in {stopwatch.Elapsed.Ticks} ticks ({stopwatch.Elapsed.Seconds} seconds).");
            // Plugin.Log.LogWarning($"{Stopwatch.Elapsed.Ticks} ticks {Stopwatch.Elapsed.Seconds} seconds");
        }
        
        // public void GenerateAllSpecifications()
        // {
        //     Plugin.Log.LogInfo("Generation of Dynamic specifications started. Please wait until finished. This can sometimes take several minutes. If it takes longer than 10 minutes, please report.");
        //     var stopwatch = Stopwatch.StartNew();
        //     
        //     var generatedLocation = Path.Combine(Plugin.Mod.DirectoryPath, "GeneratedSpecifications");
        //     if (!Directory.Exists(generatedLocation)) 
        //         Directory.CreateDirectory(generatedLocation);
        //
        //     foreach (var component in _activeComponentRetriever.GetAllComponents())
        //     {
        //         GenerateComponentSpecification(component);
        //     }
        //
        //     foreach (var singleton in _singletonRepository.GetSingletons<object>())
        //     {
        //         GenerateSingletonSpecification(singleton);
        //     }
        //     
        //     stopwatch.Stop();
        //     Plugin.Log.LogInfo($"Finished generation of Dynamic specifications in {stopwatch.Elapsed.Seconds} seconds. You can find them in the '<mods folder>/DynamicSpecifications/GeneratedSpecifications.'");
        // }

        private IEnumerable<GameValueChanger> GenerateComponentSpecification(Component component)
        {
            var type = component.GetType();
            if (SkippableTypes.Types.Contains(type))
                yield break;

            if (Plugin.LoggingEnabled) Plugin.Log.LogWarning(type + "");
            if (Plugin.LoggingEnabled) Plugin.Log.LogWarning("Name Component: " + type);
            
            // PERFORMANCE = 2 SECONDEN
            var values = _dynamicPropertyObtainer.FromComponent(type, component);

            if (!values.Any())
                yield break;
            
            // var gameObjectName = component.gameObject.TryGetComponent(out Prefab prefab) ? prefab.PrefabName : component.gameObject.name;
            
            Stopwatch.Start();
            foreach (var gamevalueChanger in GetGamevalueChangers(component, component.gameObject.name, type.Name, values))
                yield return gamevalueChanger;
            Stopwatch.Stop();
        }

        public IEnumerable<GameValueChanger> GenerateSingletonSpecification(object singleton)
        {
            var type = singleton.GetType();

            if (SkippableTypes.Types.Contains(type))
                yield break;

            if (Plugin.LoggingEnabled) Plugin.Log.LogWarning(type + "");
            if (Plugin.LoggingEnabled) Plugin.Log.LogWarning("Name Singleton: " + type);
            
            var values = _dynamicPropertyObtainer.FromSingleton(type, singleton);
            
            var typeName = type.Name;
            
            if (!values.Any())
                yield break;

            foreach (var gamevalueChanger in GetGamevalueChangers(singleton, typeName, type.Name, values))
                yield return gamevalueChanger;
        }

        private IEnumerable<GameValueChanger> GetGamevalueChangers(object instance, string className, string fieldname, DynamicProperty[] dynamicProperties)
        {
            foreach (var dynamicProperty in dynamicProperties)
            {
                var fieldRef = new FieldRef(
                    () => InaccessibilityUtilities.GetInaccessibleField(instance, dynamicProperty.OriginalName),
                    value => InaccessibilityUtilities.SetInaccessibleField(instance, dynamicProperty.OriginalName, value));
                
                yield return GetGameValueChanger(dynamicProperty.Value, className, fieldname, dynamicProperty, fieldRef);
            }
        }

        public GameValueChanger GetGameValueChanger(object value, string className, string fieldname, DynamicProperty dynamicProperty, FieldRef fieldRef)
        {
            var combinedFieldName = fieldname + "." + dynamicProperty.StyledName;

            switch (value)
            {
                case int:
                    return new SaveableGameValueChanger(
                        fieldRef,
                        className,
                        combinedFieldName,
                        combinedFieldName + ":",
                        false
                    );
                case float:
                    return new SaveableGameValueChanger(
                        fieldRef,
                        className,
                        combinedFieldName,
                        combinedFieldName + ":",
                        false
                    );
                case string:
                    return new SaveableGameValueChanger(
                        fieldRef,
                        className,
                        combinedFieldName,
                        combinedFieldName + ":",
                        false
                    );
                case bool:
                    return new SaveableGameValueChanger(
                        fieldRef,
                        className,
                        combinedFieldName,
                        combinedFieldName + ":",
                        false
                    );
                case IEnumerable enumerable:

                    var list = new List<GameValueChanger>();
                    foreach (var item in enumerable)
                    {
                        list.Add(GetGameValueChanger(item, className, fieldname, dynamicProperty, FieldRef.GetterOnly(item)));
                    }

                    return new CollectionSaveableGameValueChanger(
                        fieldRef,
                        className,
                        combinedFieldName,
                        combinedFieldName + ":",
                        false,
                        dynamicProperty,
                        list
                    );
                case not null:
                    return new ValueTypeSaveableGameValueChanger(
                        fieldRef,
                        className,
                        combinedFieldName,
                        combinedFieldName + ":",
                        false,
                        GenerateSingletonSpecification(value).ToList()
                    );
                default:
                    Plugin.Log.LogWarning($"Type {dynamicProperty.Value.GetType()} is not supported.");
                    return null;
            }
        }
    }
}