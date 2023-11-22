using System;
using Timberborn.Persistence;

namespace TobbyTools.CustomTutorialSystem
{
    public class CustomTutorialStageDeserializer : IObjectSerializer<CustomTutorialStage>
    {
        private readonly CustomTutorialStepDeserializer _customTutorialStepDeserializer;

        CustomTutorialStageDeserializer(CustomTutorialStepDeserializer customTutorialStepDeserializer)
        {
            _customTutorialStepDeserializer = customTutorialStepDeserializer;
        }
        
        public void Serialize(CustomTutorialStage value, IObjectSaver objectSaver) => throw new NotSupportedException();

        public Obsoletable<CustomTutorialStage> Deserialize(IObjectLoader objectLoader)
        {
            return new CustomTutorialStage(
                objectLoader.Get(new PropertyKey<string>("LocKey")),
                objectLoader.Get(new ListKey<CustomTutorialStep>("Steps"), _customTutorialStepDeserializer).ToArray());
        }
    }
}
