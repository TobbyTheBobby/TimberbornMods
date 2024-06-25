using Bindito.Core;
using TimberApi.SceneSystem;
using Timberborn.PrefabSystem;
using TobbyTools.UsedImplicitlySystem;

namespace MorePaths.CustomPaths
{
    [Configurator(SceneEntrypoint.InGame)]
    public class CustomPathsConfigurator : IConfigurator
    {
        public void Configure(IContainerDefinition containerDefinition)
        {
            containerDefinition.Bind<CustomPathFactory>().AsSingleton();
            containerDefinition.Bind<CustomPathToolButtonController>().AsSingleton();
            containerDefinition.MultiBind<IObjectCollection>().To<CustomPathsObjectCollection>().AsSingleton();
        }
    }
}
