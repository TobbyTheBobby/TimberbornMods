using Bindito.Core;
using TimberApi.ConfiguratorSystem;
using TimberApi.SceneSystem;

namespace CategoryButton
{
    [Configurator(SceneEntrypoint.InGame | SceneEntrypoint.MapEditor)]
    public class ToolBarCategoriesConfigurator : IConfigurator
    {
        public void Configure(IContainerDefinition containerDefinition)
        {
            containerDefinition.Bind<ImageRepository>().AsSingleton();
            
            containerDefinition.Bind<CategoryButtonFactory>().AsSingleton();
            containerDefinition.Bind<CategoryButtonSpecificationDeserializer>().AsSingleton();
            containerDefinition.Bind<CategoryButtonService>().AsSingleton();
        }
    }
}