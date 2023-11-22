namespace DynamicSpecifications
{
    public class DynamicProperty
    {
        public DynamicProperty(string name, object value = null)
        {
            OriginalName = name;
            if (name.Length == 1)
                StyledName = char.ToUpper(name[0]).ToString();
            else
            {
                if (name[0] == "_".ToCharArray()[0])
                    StyledName = char.ToUpper(name.Replace("_", "")[0]) + name.Substring(2);
                else
                    StyledName = name;
            }
                
            Value = value;
        }
        
        public readonly string OriginalName;
        
        public readonly string StyledName;
        
        public readonly object Value;
    }
}