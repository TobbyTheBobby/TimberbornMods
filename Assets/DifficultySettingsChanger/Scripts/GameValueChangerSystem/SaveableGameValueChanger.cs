namespace DifficultySettingsChanger
{
    public class SaveableGameValueChanger : GameValueChanger
    {
        public SaveableGameValueChanger(
            FieldRef fieldRef, 
            string className,
            string fieldName,
            string labelText, 
            bool isLiveUpdateable) : 
            
            base(fieldRef, 
                className,
                fieldName,
                labelText, 
                isLiveUpdateable)
        {
        }
    }
}