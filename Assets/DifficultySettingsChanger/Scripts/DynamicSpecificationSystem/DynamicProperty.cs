namespace DifficultySettingsChanger
{
    public class DynamicProperty
    {
        public DynamicProperty(string name, object value = null)
        {
            OriginalName = name;
            StyledName = CreateStyledName(name);
            Value = value;
        }
        
        public readonly string OriginalName;
        
        public readonly string StyledName;
        
        public readonly object Value;

        private string CreateStyledName(string originalName)
        {
            if (originalName.Length == 1)
                return char.ToUpper(originalName[0]).ToString();

            var styledName = originalName;
            
            styledName = styledName.Replace("<", "");
            styledName = styledName.Replace(">k__BackingField", "");
            
            if (styledName[0] == "_".ToCharArray()[0])
                return char.ToUpper(styledName.Replace("_", "")[0]) + styledName.Substring(2);
            
            return styledName;
        }
    }
}