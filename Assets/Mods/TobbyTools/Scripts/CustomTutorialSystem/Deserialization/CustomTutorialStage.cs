namespace TobbyTools.CustomTutorialSystem
{
    public class CustomTutorialStage
    {
        public CustomTutorialStage(string locKey, CustomTutorialStep[] steps)
        {
            LocKey = locKey;
            Steps = steps;
        }
        
        public string LocKey { get; private set; }
        public CustomTutorialStep[] Steps { get; private set; }
    }
}