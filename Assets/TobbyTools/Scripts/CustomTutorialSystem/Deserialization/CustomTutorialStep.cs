namespace TobbyTools.CustomTutorialSystem
{
    public class CustomTutorialStep
    {
        public CustomTutorialStep(string stepType, CustomTutorialStepSettings customTutorialStepSettings)
        {
            StepType = stepType;
            CustomTutorialStepSettings = customTutorialStepSettings;
        }
        
        public string StepType { get; private set; }
        public CustomTutorialStepSettings CustomTutorialStepSettings { get; private set; }
    }
}