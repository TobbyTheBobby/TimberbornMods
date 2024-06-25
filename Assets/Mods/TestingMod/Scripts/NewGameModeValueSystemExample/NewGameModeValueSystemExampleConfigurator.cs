using Bindito.Core;
using TimberApi.SceneSystem;
using TobbyTools.NewGameModeValueSystem;
using TobbyTools.UsedImplicitlySystem;

namespace TestingMod.NewGameModeValueSystemExample
{
    [Configurator(SceneEntrypoint.All)]
    public class NewGameModeValueSystemExampleConfiguratorAll : IConfigurator
    {
        public void Configure(IContainerDefinition containerDefinition)
        {
            containerDefinition.BindNewGameValue<SeasonDurationNewGameValue>();
        }
    }
    
    [Configurator(SceneEntrypoint.InGame)]
    public class NewGameModeValueSystemExampleConfiguratorInGame : IConfigurator
    {
        public void Configure(IContainerDefinition containerDefinition)
        {
            containerDefinition.Bind<SeasonService>().AsSingleton();
        }
    }
}