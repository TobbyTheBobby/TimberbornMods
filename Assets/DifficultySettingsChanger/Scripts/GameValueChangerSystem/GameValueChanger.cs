using System;
using Newtonsoft.Json;

namespace DifficultySettingsChanger
{
    public class GameValueChanger
    {
        public readonly FieldRef FieldRef;
        public readonly Type ParentType;
        public readonly string ObjectName;
        public readonly string FieldName;
        public readonly bool IsLiveUpdateable;
        public readonly DynamicProperty DynamicProperty;
        public readonly string SerializedInitialValue;

        public string SerializedValue => Serialize(FieldRef.Value);
        public string ClassFieldNameCombined => $"{ObjectName}.{DynamicProperty.StyledName}";
        public string LabelText => $"{DynamicProperty.StyledName}";
        
        public GameValueChanger(
            FieldRef fieldRef,
            Type parentType,
            string objectName,
            string fieldName,
            bool isLiveUpdateable,
            DynamicProperty dynamicProperty)
        {
            FieldRef = fieldRef;
            ParentType = parentType;
            ObjectName = objectName;
            FieldName = fieldName;
            IsLiveUpdateable = isLiveUpdateable;
            DynamicProperty = dynamicProperty;
            SerializedInitialValue = Serialize(fieldRef.Value);
        }

        private string Serialize(object value)
        {
            try
            {
                return JsonConvert.SerializeObject(value);
            }
            catch (Exception)
            {
                return "";
            }
        }
    }
}