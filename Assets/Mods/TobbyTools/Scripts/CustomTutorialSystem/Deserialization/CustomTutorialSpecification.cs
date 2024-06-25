namespace TobbyTools.CustomTutorialSystem
{
    public class CustomTutorialSpecification
    {
        public CustomTutorialSpecification(CustomTutorialStartingConditions customTutorialStartingConditions, CustomTutorialStage[] tutorialStages)
        {
            CustomTutorialStartingConditions = customTutorialStartingConditions;
            TutorialStages = tutorialStages;
        }

        public CustomTutorialStartingConditions CustomTutorialStartingConditions { get; private set; }
        public CustomTutorialStage[] TutorialStages { get; private set; }
    }
}