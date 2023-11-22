using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using TimberApi.DependencyContainerSystem;
using Timberborn.Persistence;

namespace DifficultySettingsChanger
{
    public class CollectionSaveableGameValueChanger : SaveableGameValueChanger
    {
        public readonly List<GameValueChanger> GameValueChangers;

        public readonly DynamicProperty Property;
        
        public readonly Type CollectionType;

        public CollectionSaveableGameValueChanger(
            FieldRef fieldRef, 
            string className, 
            string fieldName, 
            string labelText, 
            bool isLiveUpdateable,
            DynamicProperty dynamicProperty,
            List<GameValueChanger> gameValueChangers) : 
            
            base(
                fieldRef, 
                className, 
                fieldName, 
                labelText, 
                isLiveUpdateable)
        {
            GameValueChangers = gameValueChangers;
            Property = dynamicProperty;
            CollectionType = GetItemType(fieldRef.Value);
        }

        public void ClearCollection()
        {
            GameValueChangers.Clear();
        }

        public GameValueChanger AddItem()
        {
            var dynamicSpecificationGenerator = DependencyContainer.GetInstance<DynamicSpecificationGenerator>();
            
            var newInstance = Activator.CreateInstance(CollectionType);

            var test = dynamicSpecificationGenerator.GetGameValueChanger(newInstance, ClassName, FieldName, Property, FieldRef.GetterOnly(newInstance));

            GameValueChangers.Add(test);

            return test;
        }
        
        public void UpdateCollection()
        {
            var test = new List<object>();
            
            foreach (var VARIABLE in GameValueChangers)
            {
                test.Add(VARIABLE.FieldRef.Value);
            }

            FieldRef.Value = test;
        }
        
        
        private Type GetItemType(object someCollection)
        {
            var type = someCollection.GetType();
            var ienum = type.GetInterface(typeof(IEnumerable<>).Name);
            return ienum != null
                ? ienum.GetGenericArguments()[0]
                : null;
        }
    }
}