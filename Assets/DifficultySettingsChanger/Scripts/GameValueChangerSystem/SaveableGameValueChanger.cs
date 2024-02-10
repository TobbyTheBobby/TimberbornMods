using System;

namespace DifficultySettingsChanger
{
    public class SaveableGameValueChanger : GameValueChanger
    {
        public SaveableGameValueChanger(
            FieldRef fieldRef, 
            Type parentType,
            string objectName,
            string fieldName,
            bool isLiveUpdateable,
            DynamicProperty dynamicProperty) : 
            
            base(fieldRef, 
                parentType,
                objectName,
                fieldName,
                isLiveUpdateable,
                dynamicProperty)
        {
        }
    }
}