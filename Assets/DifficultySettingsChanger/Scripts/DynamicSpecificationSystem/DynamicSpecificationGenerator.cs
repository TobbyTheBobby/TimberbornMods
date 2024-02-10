using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DifficultySettingsChanger.GameValueChangerSystem;
using Timberborn.SingletonSystem;
using TobbyTools.InaccessibilityUtilitySystem;
using UnityEngine;

namespace DifficultySettingsChanger
{
    public class DynamicSpecificationGenerator : IGameValueChangerGenerator
    {
        private readonly ActiveComponentRetriever _activeComponentRetriever;
        private readonly DynamicPropertyObtainer _dynamicPropertyObtainer;
        private readonly ISingletonRepository _singletonRepository;

        private readonly Stopwatch _stopwatch = new();

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
                foreach (var gameValueChanger in GenerateComponentSpecification(component))
                    yield return gameValueChanger;
            }

            // var singletonRepository = (SingletonRepository)_singletonRepository;
            // foreach (var singleton in singletonRepository._singletonListener.Collect())
            // {
            //     Plugin.Log.LogError(singleton.GetType().Name);
            //     foreach (var gameValueChanger in GenerateSingletonSpecification(singleton))
            //         yield return gameValueChanger;
            // }

            stopwatch.Stop();
            Plugin.Log.LogWarning($"Finished generation of Dynamic Values in {stopwatch.Elapsed.Ticks} ticks ({stopwatch.Elapsed.Seconds} seconds).");
            // Plugin.Log.LogWarning($"{Stopwatch.Elapsed.Ticks} ticks {Stopwatch.Elapsed.Seconds} seconds");
        }

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
            
            _stopwatch.Start();
            foreach (var gameValueChanger in GetGameValueChangers(component, component.gameObject.name, type.Name, values))
                yield return gameValueChanger;
            _stopwatch.Stop();
        }

        public IEnumerable<GameValueChanger> GenerateSingletonSpecification(object singleton)
        {
            var type = singleton.GetType();

            if (SkippableTypes.Types.Contains(type))
                yield break;

            if (Plugin.LoggingEnabled) Plugin.Log.LogWarning("Name Singleton: " + type);
            
            var values = _dynamicPropertyObtainer.FromSingleton(type, singleton);
            
            var typeName = type.Name;
            
            if (!values.Any())
                yield break;

            foreach (var gameValueChanger in GetGameValueChangers(singleton, typeName, type.Name, values))
                yield return gameValueChanger;
        }

        private IEnumerable<GameValueChanger> GetGameValueChangers(object instance, string className, string fieldName, DynamicProperty[] dynamicProperties)
        {
            foreach (var dynamicProperty in dynamicProperties)
            {
                var fieldRef = new FieldRef(
                    () => InaccessibilityUtilities.GetInaccessibleField(instance, dynamicProperty.OriginalName),
                    value => InaccessibilityUtilities.SetInaccessibleField(instance, dynamicProperty.OriginalName, value));

                var gameValueChanger = GetGameValueChanger(dynamicProperty.Value, instance.GetType(), className, fieldName, dynamicProperty, fieldRef);
                if (gameValueChanger != null)
                    yield return gameValueChanger;
            }
        }

        public GameValueChanger GetGameValueChanger(object value, Type parentType, string className, string fieldName, DynamicProperty dynamicProperty, FieldRef fieldRef)
        {
            var combinedFieldName = fieldName + "." + dynamicProperty.StyledName;

            switch (value)
            {
                case int:
                    return new SaveableGameValueChanger(
                        fieldRef,
                        parentType,
                        className,
                        combinedFieldName,
                        false,
                        dynamicProperty
                    );
                case float:
                    return new SaveableGameValueChanger(
                        fieldRef,
                        parentType,
                        className,
                        combinedFieldName,
                        false,
                        dynamicProperty
                    );
                case string:
                    return new SaveableGameValueChanger(
                        fieldRef,
                        parentType,
                        className,
                        combinedFieldName,
                        false,
                        dynamicProperty
                    );
                case bool:
                    return new SaveableGameValueChanger(
                        fieldRef,
                        parentType,
                        className,
                        combinedFieldName,
                        false,
                        dynamicProperty
                    );
                case IEnumerable enumerable:

                    var gameValueType = typeof(GameValueChanger);
                    
                    var list = new List<GameValueChanger>();
                    foreach (var item in enumerable)
                    {
                        list.Add(GetGameValueChanger(item, null, className, fieldName, dynamicProperty, FieldRef.GetterOnly(item)));
                    }

                    if (list.Any())
                    {
                        gameValueType = list.First().GetType();
                    }

                    return new CollectionSaveableGameValueChanger(
                        fieldRef,
                        parentType,
                        className,
                        combinedFieldName,
                        false,
                        dynamicProperty,
                        list,
                        fieldName,
                        gameValueType
                    );
                case not null:
                    return new ValueTypeSaveableGameValueChanger(
                        fieldRef,
                        parentType,
                        className,
                        combinedFieldName,
                        false,
                        dynamicProperty,
                        GenerateSingletonSpecification(value).ToList()
                    );
                default:
                    // Plugin.Log.LogWarning($"Type {dynamicProperty.Value.GetType()} is not supported.");
                    return null;
            }
        }
    }
}