using System.Collections.Generic;
using System.IO;
using System.Linq;
using DifficultySettingsChanger.GameValueChangerSystemUI;
using Newtonsoft.Json;
using TimberApi.Common.SingletonSystem;
using Timberborn.SingletonSystem;

namespace DifficultySettingsChanger.GameValueChangerSystem
{
    public class GameValueChangerService : IEarlyLoadableSingleton
    {
        private readonly GameValueSpecificationDeserializer _gameValueSpecificationDeserializer;
        private readonly GameValueSpecificationRepository _gameValueSpecificationRepository;
        private readonly GameValueChangerUiPresetFactory _gameValueChangerUiPresetFactory;
        private readonly GameValueChangerRepository _gameValueChangerRepository;
        private readonly EventBus _eventBus;

        private GameValueChangerService(
            GameValueSpecificationDeserializer gameValueSpecificationDeserializer,
            GameValueSpecificationRepository gameValueSpecificationRepository,
            GameValueChangerUiPresetFactory gameValueChangerUiPresetFactory, 
            GameValueChangerRepository gameValueChangerRepository,
            EventBus eventBus)
        {
            _gameValueSpecificationDeserializer = gameValueSpecificationDeserializer;
            _gameValueSpecificationRepository = gameValueSpecificationRepository;
            _gameValueChangerUiPresetFactory = gameValueChangerUiPresetFactory;
            _gameValueChangerRepository = gameValueChangerRepository;
            _eventBus = eventBus;
        }
        
        [OnEvent]
        public void OnGameValueChangerBoxConfirmed(OnGameValueChangerBoxConfirmed onGameValueChangerBoxConfirmed)
        {
            Save();
        }

        private void Save()
        {
            var gameValueSpecifications = new List<GameValueSpecification>();
            foreach (var gameValueChangers in _gameValueChangerRepository.GamevalueChangers.GroupBy(changer => changer.ObjectName))
            {
                foreach (var gameValueChanger in gameValueChangers.OrderBy(changer => changer.ClassFieldNameCombined))
                {
                    var gameValueSpecification = GetSpecification(gameValueChanger, out _);

                    if (gameValueSpecification == null)
                        continue;
                
                    gameValueSpecifications.Add(gameValueSpecification);
                }
            }

            var fileName = "GameValuesSpecification.original.json";
            
            var path = Path.Combine(Plugin.Mod.DirectoryPath, "Specifications", fileName);
            
            File.WriteAllText(path, JsonConvert.SerializeObject(new GameValuesSpecification(gameValueSpecifications), Formatting.Indented));
        }

        private GameValueSpecification GetSpecification(GameValueChanger gameValueChanger, out bool wasForced, bool tryForcingSpecification = false)
        {
            wasForced = false;
            
            if (gameValueChanger is not SaveableGameValueChanger saveableGameValueChanger) 
                return null;

            if (gameValueChanger is CollectionSaveableGameValueChanger collectionSaveableGameValueChanger)
            {
                Plugin.Log.LogInfo("Saving CollectionSaveableGameValueChanger");

                var shouldSave = false;
                
                var values = new List<GameValueSpecification>();
                foreach (var valueChanger in collectionSaveableGameValueChanger.GameValueChangers)
                {
                    // Plugin.Log.LogError("Trying to add value");
                    var gameValueSpecification = GetSpecification(valueChanger, out wasForced, true);
            
                    // Plugin.Log.LogInfo("gameValueSpecification == null " + (gameValueSpecification == null) + "  " +
                    //                    valueChanger.ClassName + " " + valueChanger.ClassFieldNameCombined + "   " +
                    //                    valueChanger.SerializedInitialValue + "   " + valueChanger.FieldRef.Value);
            
                    // Plugin.Log.LogWarning("Value added");
            
                    values.Add(gameValueSpecification);

                    if (!shouldSave)
                    {
                        shouldSave = !wasForced;
                    }
                    
                    // if (gameValueSpecification == null || wasForced)
                    //     continue;
                    //
                    // shouldSave = true;
                }
            
                if (!shouldSave)
                    return null;
            
                return new GameValueSpecification(
                    collectionSaveableGameValueChanger.ObjectName,
                    collectionSaveableGameValueChanger.ClassFieldNameCombined,
                    values);
            }
            
            if (gameValueChanger is ValueTypeSaveableGameValueChanger valueTypeSaveableGameValueChanger)
            {
                // Plugin.Log.LogInfo("Saving ValueTypeSaveableGameValueChanger");
                
                var values = new List<GameValueSpecification>();
                
                // Plugin.Log.LogWarning("Are the same: " + Equals(valueTypeSaveableGameValueChanger.SerializedValue, valueTypeSaveableGameValueChanger.SerializedInitialValue));
                
                // Plugin.Log.LogWarning(gameValueChanger.ClassName + " " + gameValueChanger.ClassFieldNameCombined + "   " + gameValueChanger.SerializedInitialValue + "   " + gameValueChanger.SerializedValue);
                
                foreach (var fieldValueChanger in valueTypeSaveableGameValueChanger.Fields)
                {
                    // Plugin.Log.LogInfo("BEFORE: " + fieldValueChanger.ClassName + " " + fieldValueChanger.ClassFieldNameCombined + "   " + fieldValueChanger.SerializedInitialValue + "   " + fieldValueChanger.SerializedValue);
                    
                    var gameValueSpecification = GetSpecification(fieldValueChanger, out wasForced, tryForcingSpecification);

                    // Plugin.Log.LogInfo("After: " + "gameValueSpecification == null " + (gameValueSpecification == null));

                    if (gameValueSpecification == null)
                        continue;

                    // Plugin.Log.LogWarning("Value added");

                    values.Add(gameValueSpecification);
                }

                if (!values.Any())
                {
                    if (tryForcingSpecification)
                    {
                        wasForced = true;
                        return new GameValueSpecification(
                            valueTypeSaveableGameValueChanger.ObjectName,
                            valueTypeSaveableGameValueChanger.ClassFieldNameCombined,
                            values);
                    }

                    return null;
                }

                return new GameValueSpecification(
                    valueTypeSaveableGameValueChanger.ObjectName,
                    valueTypeSaveableGameValueChanger.ClassFieldNameCombined,
                    values);
            }

            // Plugin.Log.LogError("Are the same: " + (gameValueChanger.SerializedValue == gameValueChanger.SerializedInitialValue));
            
            // Plugin.Log.LogError(gameValueChanger.ClassName + " " + gameValueChanger.ClassFieldNameCombined + "   " + gameValueChanger.SerializedInitialValue + "   " + gameValueChanger.SerializedValue);

            if (gameValueChanger.SerializedValue == gameValueChanger.SerializedInitialValue)
            {
                if (tryForcingSpecification)
                {
                    wasForced = true;
                    return new GameValueSpecification(
                        saveableGameValueChanger.ObjectName,
                        saveableGameValueChanger.ClassFieldNameCombined,
                        saveableGameValueChanger.FieldRef.Value);
                }
                return null;
            }
            
            // Plugin.Log.LogError("Field Changed: " + gameValueChanger.FieldRef.FieldWasChanged);
            // Plugin.Log.LogError(gameValueChanger.ClassName + " " + gameValueChanger.ClassFieldNameCombined + "   " + gameValueChanger.SerializedInitialValue + "   " + gameValueChanger.FieldRef.Value);
            //
            // if (!gameValueChanger.FieldRef.FieldWasChanged)
            //     return null;

            return new GameValueSpecification(
                saveableGameValueChanger.ObjectName,
                saveableGameValueChanger.ClassFieldNameCombined,
                saveableGameValueChanger.FieldRef.Value);
        }

        public void EarlyLoad()
        {
            _eventBus.Register(this);

            var saveableGameValueChangers = _gameValueChangerRepository.GamevalueChangers.OfType<SaveableGameValueChanger>().ToArray();

            foreach (var gameValueSpecification in _gameValueSpecificationRepository.GameValueSpecifications)
            {
                var saveableGameValueChanger = saveableGameValueChangers.FirstOrDefault(changer => gameValueSpecification.ClassName == changer.ObjectName && gameValueSpecification.FieldName == changer.ClassFieldNameCombined);
            
                if(saveableGameValueChanger == null)
                    return;
                
                UpdateValues(gameValueSpecification, saveableGameValueChanger);
            }
        }

        private void UpdateValues(GameValueSpecification gameValueSpecification, SaveableGameValueChanger saveableGameValueChanger)
        {
            // Plugin.Log.LogWarning($"UPDATING: {saveableGameValueChanger?.ClassName}.{saveableGameValueChanger?.ClassFieldNameCombined}");

            if (saveableGameValueChanger is CollectionSaveableGameValueChanger collectionSaveableGameValueChanger)
            {
                // Plugin.Log.LogInfo("Updating CollectionSaveableGameValueChanger");
                // Plugin.Log.LogInfo($"{collectionSaveableGameValueChanger.Property.OriginalName}: {collectionSaveableGameValueChanger.Property.Value}");
                
                var fieldGameValueSpecifications = (List<GameValueSpecification>)gameValueSpecification.Value;
                
                // Plugin.Log.LogInfo("fieldGameValueSpecifications.Any()  " + fieldGameValueSpecifications.Any());
                
                if (!fieldGameValueSpecifications.Any())
                    return;
                
                collectionSaveableGameValueChanger.ClearCollection();
                foreach (var fieldGameValueSpecification in fieldGameValueSpecifications)
                {
                    if (collectionSaveableGameValueChanger.GameValueType == typeof(ValueTypeSaveableGameValueChanger))
                    {
                        // Plugin.Log.LogInfo($"{fieldGameValueSpecification.ClassName} {fieldGameValueSpecification.ClassFieldNameCombined}");
                        collectionSaveableGameValueChanger.AddValueTypeItem(fieldGameValueSpecification);
                        continue;
                    }

                    var item = collectionSaveableGameValueChanger.AddItem();
                    UpdateValues(fieldGameValueSpecification, item);
                }
                
                collectionSaveableGameValueChanger.UpdateCollection();
                return;
            }
            
            if (saveableGameValueChanger is ValueTypeSaveableGameValueChanger valueTypeSaveableGameValueChanger)
            {
                var fieldGameValueSpecifications = (List<GameValueSpecification>)gameValueSpecification.Value;

                foreach (var fieldGameValueSpecification in fieldGameValueSpecifications)
                {
                    if (fieldGameValueSpecification == null)
                        continue;

                    var fieldSaveableGameValueChanger = (SaveableGameValueChanger)valueTypeSaveableGameValueChanger.Fields.FirstOrDefault(changer => changer != null && fieldGameValueSpecification.ClassName == changer.ObjectName && fieldGameValueSpecification.FieldName == changer.ClassFieldNameCombined);
                    
                    if(fieldSaveableGameValueChanger == null)
                        continue;
                    
                    UpdateValues(fieldGameValueSpecification, fieldSaveableGameValueChanger);
                }
                return;
            }

            saveableGameValueChanger.FieldRef.Value = gameValueSpecification.Value;
        }
    }
}