namespace DifficultySettingsChanger
{
    public class GameValueSpecification
    {
        public GameValueSpecification(
            string className,
            string fieldName,
            object value)
        {
            ClassName = className;
            FieldName = fieldName;
            Value = value;
        }

        public string ClassName { get; }
        public string FieldName { get; }
        public object Value { get; }
    }
}