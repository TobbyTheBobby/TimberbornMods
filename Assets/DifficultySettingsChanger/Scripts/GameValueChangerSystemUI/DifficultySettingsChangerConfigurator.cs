using Bindito.Core;
using TimberApi.ConfiguratorSystem;
using TimberApi.SceneSystem;
using Timberborn.CoreUI;

namespace DifficultySettingsChanger.GameValueChangerSystemUI
{
    [Configurator(SceneEntrypoint.InGame)]
    public class GameValueChangerSystemUIConfigurator : IConfigurator
    {
        public void Configure(IContainerDefinition containerDefinition)
        {
            containerDefinition.Bind<DifficultySettingsChangerBox>().AsSingleton();
            containerDefinition.Bind<CollectionEditorBox>().AsSingleton();
            containerDefinition.Bind<ValueTypeEditorBox>().AsSingleton();
            containerDefinition.Bind<GameValueChangerUiPresetFactory>().AsSingleton();
            containerDefinition.Bind<HierarchicalManager>().AsSingleton();
            containerDefinition.Bind<ScrollBarInitializationService>().AsSingleton();
            containerDefinition.MultiBind<IVisualElementInitializer>().To<ScrollBarInitializer>().AsSingleton();
        }
    }
}