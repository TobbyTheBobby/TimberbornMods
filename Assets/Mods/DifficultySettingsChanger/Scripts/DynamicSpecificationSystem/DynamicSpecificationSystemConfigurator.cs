using Bindito.Core;
using TimberApi.SceneSystem;
using TobbyTools.UsedImplicitlySystem;

namespace DifficultySettingsChanger
{
    [Configurator(SceneEntrypoint.InGame)]
    public class DynamicSpecificationSystemConfigurator : IConfigurator
    {
        public void Configure(IContainerDefinition containerDefinition)
        {
            containerDefinition.Bind<ActiveComponentRetriever>().AsSingleton();
            containerDefinition.Bind<DynamicSpecificationDeserializer>().AsSingleton();
            containerDefinition.MultiBind<IGameValueChangerGenerator>().To<DynamicSpecificationGenerator>().AsSingleton();
            containerDefinition.Bind<DynamicSpecificationGenerator>().AsSingleton();
            containerDefinition.Bind<DynamicSpecificationApplier>().AsSingleton();
            containerDefinition.Bind<DynamicPropertyObtainer>().AsSingleton();
        }
    }
}