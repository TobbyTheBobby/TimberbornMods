using System;
using Newtonsoft.Json;

namespace DifficultySettingsChanger
{
    public class GameValueChanger
    {
        public readonly FieldRef FieldRef;
        
        public readonly string ClassName;

        public readonly string FieldName;

        public readonly string LabelText;

        public readonly bool IsLiveUpdateable;

        public readonly string SerializedInitialValue;

        public string SerializedValue => Serialize(FieldRef.Value);

        public GameValueChanger(
            FieldRef fieldRef,
            string className,
            string fieldName,
            string labelText,
            bool isLiveUpdateable)
        {
            FieldRef = fieldRef;
            ClassName = className;
            FieldName = fieldName;
            LabelText = labelText;
            IsLiveUpdateable = isLiveUpdateable;
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