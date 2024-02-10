using Bindito.Core;
using TimberApi.SceneSystem;
using Timberborn.CoreUI;
using TobbyTools.UsedImplicitlySystem;

namespace DifficultySettingsChanger.GameValueChangerSystemUI
{
    [Configurator(SceneEntrypoint.InGame)]
    public class GameValueChangerSystemUIConfigurator : IConfigurator
    {
        public void Configure(IContainerDefinition containerDefinition)
        {
            containerDefinition.Bind<DifficultySettingsChangerBox>().AsSingleton();
            containerDefinition.Bind<GameValueChangerUiPresetFactory>().AsSingleton();
            containerDefinition.Bind<ScrollBarInitializationService>().AsSingleton();
            containerDefinition.MultiBind<IVisualElementInitializer>().To<ScrollBarInitializer>().AsSingleton();
        }
    }
}