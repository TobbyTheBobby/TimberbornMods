using System;
using System.Collections.Generic;

namespace DifficultySettingsChanger
{
    public class ValueTypeSaveableGameValueChanger : SaveableGameValueChanger
    {
        public readonly IEnumerable<GameValueChanger> Fields;

        public ValueTypeSaveableGameValueChanger(
            FieldRef fieldRef, 
            Type parentType,
            string className, 
            string fieldName, 
            bool isLiveUpdateable,
            DynamicProperty dynamicProperty,
            IEnumerable<GameValueChanger> gameValueChangers) : 
            
            base(
                fieldRef, 
                parentType,
                className, 
                fieldName, 
                isLiveUpdateable,
                dynamicProperty)
        {
            Fields = gameValueChangers;
        }
    }
}