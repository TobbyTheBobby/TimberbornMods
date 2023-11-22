using System.Collections.Generic;

namespace DifficultySettingsChanger
{
    public class ValueTypeSaveableGameValueChanger : SaveableGameValueChanger
    {
        public readonly IEnumerable<GameValueChanger> Fields;

        public ValueTypeSaveableGameValueChanger(
            FieldRef fieldRef, 
            string className, 
            string fieldName, 
            string labelText, 
            bool isLiveUpdateable,
            IEnumerable<GameValueChanger> gameValueChangers) : 
            
            base(
                fieldRef, 
                className, 
                fieldName, 
                labelText, 
                isLiveUpdateable)
        {
            Fields = gameValueChangers;
        }
    }
}