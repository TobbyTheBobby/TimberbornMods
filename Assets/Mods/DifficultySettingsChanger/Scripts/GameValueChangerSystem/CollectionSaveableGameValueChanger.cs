using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TimberApi.DependencyContainerSystem;

namespace DifficultySettingsChanger.GameValueChangerSystem
{
    public class CollectionSaveableGameValueChanger : SaveableGameValueChanger
    {
        public readonly List<GameValueChanger> GameValueChangers;
        public readonly Type CollectionType;
        public readonly string CollectionFieldName;
        public readonly Type GameValueType;

        public CollectionSaveableGameValueChanger(
            FieldRef fieldRef, 
            Type parentType,
            string objectName, 
            string fieldName, 
            bool isLiveUpdateable,
            DynamicProperty dynamicProperty,
            List<GameValueChanger> gameValueChangers,
            string collectionFieldName,
            Type gameValueType) : 
            
            base(
                fieldRef, 
                parentType,
                objectName, 
                fieldName, 
                isLiveUpdateable,
                dynamicProperty)
        {
            GameValueChangers = gameValueChangers;
            CollectionType = GetItemType(fieldRef.Value);
            CollectionFieldName = collectionFieldName;
            GameValueType = gameValueType;
        }

        public void ClearCollection()
        {
            GameValueChangers.Clear();
        }

        public SaveableGameValueChanger AddItem()
        {
            var dynamicSpecificationGenerator = DependencyContainer.GetInstance<DynamicSpecificationGenerator>();
            
            // Plugin.Log.LogInfo($"CollectionType {CollectionType}");
            
            var newInstance = Activator.CreateInstance(CollectionType);
            
            // Plugin.Log.LogInfo($"ClassName {ClassName}  FieldName {FieldName}");
            
            // var dynamicPropertyObtainer = DependencyContainer.GetInstance<DynamicPropertyObtainer>();
            // var values = dynamicPropertyObtainer.FromSingleton(CollectionType, newInstance);

            var saveableGameValueChanger = dynamicSpecificationGenerator.GetGameValueChanger(newInstance, null, ObjectName, CollectionFieldName, DynamicProperty, FieldRef.GetterOnly(newInstance));

            // Plugin.Log.LogInfo($"saveableGameValueChanger.GetType() {saveableGameValueChanger.GetType()}");
            GameValueChangers.Add(saveableGameValueChanger);

            return (SaveableGameValueChanger)saveableGameValueChanger;
        }
        
        public SaveableGameValueChanger AddValueTypeItem(GameValueSpecification gameValueSpecification)
        {
            var dynamicSpecificationGenerator = DependencyContainer.GetInstance<DynamicSpecificationGenerator>();
            
            // Plugin.Log.LogInfo($"CollectionType {CollectionType}");
            // Plugin.Log.LogInfo($"saveableGameValueChanger.GetType() {gameValueSpecification.Value.GetType()}");

            var gameValueSpecifications = (List<GameValueSpecification>)gameValueSpecification.Value;
            var values = gameValueSpecifications.Select(specification => specification.Value).ToArray();
            var newInstance = Activator.CreateInstance(CollectionType, values);
            
            // Plugin.Log.LogInfo($"ClassName {ClassName}  FieldName {FieldName}");

            var saveableGameValueChanger = dynamicSpecificationGenerator.GetGameValueChanger(newInstance, null, ObjectName, CollectionFieldName, DynamicProperty, FieldRef.GetterOnly(newInstance));
            
            GameValueChangers.Add(saveableGameValueChanger);

            return (SaveableGameValueChanger)saveableGameValueChanger;
        }
        
        public void UpdateCollection()
        {
            switch (FieldRef.Value)
            {
                case Array:
                    var list = new List<object>();

                    foreach (var gameValueChanger in GameValueChangers)
                    {
                        Plugin.Log.LogInfo($"{gameValueChanger.ObjectName} {gameValueChanger.FieldName} {gameValueChanger.FieldRef.Value} ");
                        list.Add(gameValueChanger.FieldRef.Value);
                    }
                    
                    FieldRef.Value = CastArray(list.ToArray());
                    break;
                case IEnumerable:
                    FieldRef.Value = GameValueChangers.Select(changer => changer.FieldRef.Value).ToList();
                    break;
                default:
                    FieldRef.Value = GameValueChangers.Select(changer => changer.FieldRef.Value).ToList();
                    break;
            }
        }
        
        private Type GetItemType(object someCollection)
        {
            var type = someCollection.GetType();
            var ienum = type.GetInterface(typeof(IEnumerable<>).Name);
            return ienum != null
                ? ienum.GetGenericArguments()[0]
                : null;
        }
        
        private object CastArray(object[] inputArray)
        {
            var targetArrayType = CollectionType.MakeArrayType();
            var outputArray = Array.CreateInstance(CollectionType, inputArray.Length);
            inputArray.CopyTo(outputArray, 0);
            return Convert.ChangeType(outputArray, targetArrayType);
        }
    }
}