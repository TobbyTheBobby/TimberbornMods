using System;
using Timberborn.Persistence;

namespace TobbyTools.CustomTutorialSystem
{
    public class CustomTutorialSpecificationDeserializer : IObjectSerializer<CustomTutorialSpecification>
    {
        private readonly CustomTutorialStartingConditionsDeserializer _customTutorialStartingConditionsDeserializer;
        private readonly CustomTutorialStageDeserializer _customTutorialStageDeserializer;
        
        CustomTutorialSpecificationDeserializer(
            CustomTutorialStartingConditionsDeserializer customTutorialStartingConditionsDeserializer,
            CustomTutorialStageDeserializer customTutorialStageDeserializer)
        {
            _customTutorialStartingConditionsDeserializer = customTutorialStartingConditionsDeserializer;
            _customTutorialStageDeserializer = customTutorialStageDeserializer;
        }
        
        public void Serialize(CustomTutorialSpecification value, IObjectSaver objectSaver) => throw new NotSupportedException();

        public Obsoletable<CustomTutorialSpecification> Deserialize(IObjectLoader objectLoader)
        {
            return new CustomTutorialSpecification(
                objectLoader.Get(new PropertyKey<CustomTutorialStartingConditions>("StartingConditions"), _customTutorialStartingConditionsDeserializer),
                objectLoader.Get(new ListKey<CustomTutorialStage>("TutorialStages"), _customTutorialStageDeserializer).ToArray());
        }
    }
}
