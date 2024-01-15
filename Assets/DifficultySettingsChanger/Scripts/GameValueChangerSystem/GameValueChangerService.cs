using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using TimberApi.Common.SingletonSystem;
using Timberborn.SingletonSystem;
using UnityEngine.UIElements;

namespace DifficultySettingsChanger
{
    public class GameValueChangerService : IEarlyLoadableSingleton
    {
        private readonly GameValueSpecificationDeserializer _gameValueSpecificationDeserializer;
        private readonly GameValueSpecificationRepository _gameValueSpecificationRepository;
        private readonly GameValueChangerUiPresetFactory _gameValueChangerUiPresetFactory;
        private readonly GameValueChangerRepository _gameValueChangerRepository;
        private readonly EventBus _eventBus;
        
        GameValueChangerService(
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

        public IEnumerable<VisualElement> GetElements()
        {
            foreach (var groupedValueChangers in _gameValueChangerRepository.GamevalueChangers
                         .GroupBy(changer => changer.ClassName)
                     // .OrderBy(changers => changers.Key)
                    )
            {
                yield return _gameValueChangerUiPresetFactory.GetGroupHeader(groupedValueChangers.Key);

                foreach (var gameValueChanger in groupedValueChangers
                             .OrderBy(changer => changer.FieldName)
                             .Where((test, index) => index < 6))
                {
                    yield return _gameValueChangerUiPresetFactory.GetUiPreset(gameValueChanger);
                }
            }
        }
        
        [OnEvent]
        public void OnGameValueChangerBoxConfirmed(OnGameValueChangerBoxConfirmed onGameValueChangerBoxConfirmed)
        {
            Save();
        }

        private void Save()
        {
            var gameValueSpecifications = new List<GameValueSpecification>();
            foreach (var gameValueChangers in _gameValueChangerRepository.GamevalueChangers
                         .GroupBy(changer => changer.ClassName))
            {
                foreach (var gameValueChanger in gameValueChangers
                             .OrderBy(changer => changer.FieldName)
                             .Where((_, index) => index < 6))
                {
                    var gameValueSpecification = GetSpecification(gameValueChanger);

                    if (gameValueSpecification == null)
                        continue;
                
                    gameValueSpecifications.Add(gameValueSpecification);
                }
            }

            var fileName = "GameValuesSpecification.original.json";
            
            var path = Path.Combine(Plugin.Mod.DirectoryPath, "Specifications", fileName);
            
            File.WriteAllText(path, JsonConvert.SerializeObject(new GameValuesSpecification(gameValueSpecifications), Formatting.Indented));
        }

        private GameValueSpecification GetSpecification(GameValueChanger gameValueChanger)
        {
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
            
                    var gameValueSpecification = GetSpecification(valueChanger);
            
                    // Plugin.Log.LogInfo("gameValueSpecification == null " + (gameValueSpecification == null) + "  " +
                                       // valueChanger.ClassName + " " + valueChanger.FieldName + "   " +
                                       // valueChanger.SerializedInitialValue + "   " + valueChanger.FieldRef.Value);
            
                    // Plugin.Log.LogWarning("Value added");
            
                    values.Add(gameValueSpecification);
                    
                    if (gameValueSpecification == null)
                        continue;
                    
                    shouldSave = true;
                }
            
                if (!shouldSave)
                    return null;
            
                return new GameValueSpecification(
                    collectionSaveableGameValueChanger.ClassName,
                    collectionSaveableGameValueChanger.FieldName,
                    values);
            }
            
            if (gameValueChanger is ValueTypeSaveableGameValueChanger valueTypeSaveableGameValueChanger)
            {
                // Plugin.Log.LogInfo("Saving ValueTypeSaveableGameValueChanger");
                
                var values = new List<GameValueSpecification>();
                
                // Plugin.Log.LogWarning("Are the same: " + Equals(valueTypeSaveableGameValueChanger.SerializedValue, valueTypeSaveableGameValueChanger.SerializedInitialValue));
                
                Plugin.Log.LogWarning(gameValueChanger.ClassName + " " + gameValueChanger.FieldName + "   " + gameValueChanger.SerializedInitialValue + "   " + gameValueChanger.SerializedValue);
                
                foreach (var fieldValueChanger in valueTypeSaveableGameValueChanger.Fields)
                {
                    // Plugin.Log.LogInfo("BEFORE: " + fieldValueChanger.ClassName + " " + fieldValueChanger.FieldName + "   " + fieldValueChanger.SerializedInitialValue + "   " + fieldValueChanger.SerializedValue);
                    
                    var gameValueSpecification = GetSpecification(fieldValueChanger);

                    // Plugin.Log.LogInfo("After: " + "gameValueSpecification == null " + (gameValueSpecification == null));

                    if (gameValueSpecification == null)
                        continue;

                    // Plugin.Log.LogWarning("Value added");

                    values.Add(gameValueSpecification);
                }

                if (!values.Any())
                    return null;

                return new GameValueSpecification(
                    valueTypeSaveableGameValueChanger.ClassName,
                    valueTypeSaveableGameValueChanger.FieldName,
                    values);
            }

            // Plugin.Log.LogError("Are the same: " + (gameValueChanger.SerializedValue == gameValueChanger.SerializedInitialValue));
            
            // Plugin.Log.LogError(gameValueChanger.ClassName + " " + gameValueChanger.FieldName + "   " + gameValueChanger.SerializedInitialValue + "   " + gameValueChanger.SerializedValue);

            if (gameValueChanger.SerializedValue == gameValueChanger.SerializedInitialValue)
                return null;
            
            // Plugin.Log.LogError("Field Changed: " + gameValueChanger.FieldRef.FieldWasChanged);
            //
            // Plugin.Log.LogError(gameValueChanger.ClassName + " " + gameValueChanger.FieldName + "   " + gameValueChanger.SerializedInitialValue + "   " + gameValueChanger.FieldRef.Value);
            //
            // if (!gameValueChanger.FieldRef.FieldWasChanged)
            //     return null;

            return new GameValueSpecification(
                saveableGameValueChanger.ClassName,
                saveableGameValueChanger.FieldName,
                saveableGameValueChanger.FieldRef.Value);
        }

        public void EarlyLoad()
        {
            _eventBus.Register(this);

            GameValueChanger[] saveableGameValueChangers = _gameValueChangerRepository.GamevalueChangers.OfType<SaveableGameValueChanger>().ToArray();

            foreach (var gamevalueSpecification in _gameValueSpecificationRepository.GameValueSpecifications)
            {
                UpdateValues(saveableGameValueChangers, gamevalueSpecification);
                // var saveableGameValueChanger = saveableGameValueChangers.FirstOrDefault(changer => gamevalueSpecification.ClassName == changer.ClassName &&
                //     gamevalueSpecification.FieldName == changer.FieldName);
                //
                // if(saveableGameValueChanger == null)
                //     continue;
                //
                // saveableGameValueChanger.FieldRef.Value = gamevalueSpecification.Value;
            }
        }

        private void UpdateValues(GameValueChanger[] list, GameValueSpecification gameValueSpecification)
        {
            var saveableGameValueChanger = list.FirstOrDefault(changer => gameValueSpecification.ClassName == changer.ClassName && gameValueSpecification.FieldName == changer.FieldName);

            if(saveableGameValueChanger == null)
                return;

            if (saveableGameValueChanger is CollectionSaveableGameValueChanger collectionSaveableGameValueChanger)
            {
                // var fieldGameValueSpecifications = (List<GameValueSpecification>)gameValueSpecification.Value;
                //
                // if (!fieldGameValueSpecifications.Any())
                //     return;
                //
                // collectionSaveableGameValueChanger.ClearCollection();
                // foreach (var fieldGameValueSpecification in fieldGameValueSpecifications)
                // {
                //     
                //     var test = collectionSaveableGameValueChanger.AddItem();
                //
                //     UpdateValues(new[] { test }, fieldGameValueSpecification);
                //
                // }
                // // collectionSaveableGameValueChanger.UpdateCollection();
                return;
            }
            
            if (saveableGameValueChanger is ValueTypeSaveableGameValueChanger valueTypeSaveableGameValueChanger)
            {
                var fieldGameValueSpecifications = (List<GameValueSpecification>)gameValueSpecification.Value;
                
                foreach (var fieldGameValueSpecification in fieldGameValueSpecifications) 
                    UpdateValues(valueTypeSaveableGameValueChanger.Fields.ToArray(), fieldGameValueSpecification);
                return;
            }

            saveableGameValueChanger.FieldRef.Value = gameValueSpecification.Value;
        }
    }
}