using System;

namespace DifficultySettingsChanger
{
    public class FieldRef
    {
        private readonly Func<object> _getter;
        
        private readonly Action<object> _setter;

        public bool FieldWasChanged;

        public FieldRef(Func<object> getter, Action<object> setter)
        {
            _getter = getter;
            _setter = setter;
        }

        public static FieldRef GetterOnly(object item)
        {
            return new FieldRef(() => item, _ => { });
        }

        public object Value
        {
            get => _getter();
            set
            {
                FieldWasChanged = true;
                Plugin.Log.LogError($"Changing field from {_getter()} to {value}");
                _setter(value);
            }
        }
    }
}