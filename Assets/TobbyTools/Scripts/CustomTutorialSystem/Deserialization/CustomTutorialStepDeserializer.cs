using System;
using Timberborn.Persistence;

namespace TobbyTools.CustomTutorialSystem
{
    public class CustomTutorialStepDeserializer : IObjectSerializer<CustomTutorialStep>
    {
        private readonly CustomTutorialStepSettingsDeserializer _customTutorialStepSettingsDeserializer;

        CustomTutorialStepDeserializer(CustomTutorialStepSettingsDeserializer customTutorialStepSettingsDeserializer)
        {
            _customTutorialStepSettingsDeserializer = customTutorialStepSettingsDeserializer;
        }
        
        public void Serialize(CustomTutorialStep value, IObjectSaver objectSaver)
        {
            throw new NotSupportedException();
        }

        public Obsoletable<CustomTutorialStep> Deserialize(IObjectLoader objectLoader)
        {
            CustomTutorialStepSettings customTutorialStepSettings;
            try
            {
                customTutorialStepSettings = objectLoader.Get(new PropertyKey<CustomTutorialStepSettings>("StepSettings"), _customTutorialStepSettingsDeserializer);
            }
            catch (Exception)
            {
                customTutorialStepSettings = new CustomTutorialStepSettings();
            }
            return (Obsoletable<CustomTutorialStep>) new CustomTutorialStep(objectLoader.Get(new PropertyKey<string>("StepType")), customTutorialStepSettings);
        }
    }
}
