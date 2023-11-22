using Bindito.Core;
using TimberApi.ConfiguratorSystem;
using TimberApi.SceneSystem;
using Timberborn.TutorialSystem;

namespace TobbyTools.CustomTutorialSystem
{
    [Configurator(SceneEntrypoint.InGame)]
    public class CustomTutorialSystemConfigurator : IConfigurator
    {
        public void Configure(IContainerDefinition containerDefinition)
        {
            containerDefinition.Bind<ITutorialConfigurationProvider>().To<CustomTutorialConfigurationProvider>().AsSingleton();
            containerDefinition.Bind<CustomTutorialSpecificationDeserializer>().AsSingleton();
            containerDefinition.Bind<CustomTutorialStartingConditionsDeserializer>().AsSingleton();
            containerDefinition.Bind<CustomTutorialStageDeserializer>().AsSingleton();
            containerDefinition.Bind<CustomTutorialStepDeserializer>().AsSingleton();
            containerDefinition.Bind<CustomTutorialStepSettingsDeserializer>().AsSingleton();
        }
    }
}