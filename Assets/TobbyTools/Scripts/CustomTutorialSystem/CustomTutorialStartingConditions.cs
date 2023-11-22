namespace TobbyTools.CustomTutorialSystem
{
    public class CustomTutorialStartingConditions
    {
        public CustomTutorialStartingConditions(bool runAtStart, string faction)
        {
            RunAtStart = runAtStart;
            Faction = faction;
        }
            
        public bool RunAtStart { get; private set; }
        public string Faction { get; private set; }
    }
}