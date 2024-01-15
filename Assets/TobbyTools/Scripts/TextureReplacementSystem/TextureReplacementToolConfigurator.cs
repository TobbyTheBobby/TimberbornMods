using Bindito.Core;
using TimberApi.SceneSystem;
using TobbyTools.UsedImplicitlySystem;

namespace TobbyTools.TextureReplacementSystem
{
    [Configurator(SceneEntrypoint.InGame)]
    public class TextureReplacementToolConfigurator : IConfigurator
    {
        public void Configure(IContainerDefinition containerDefinition)
        {
            containerDefinition.Bind<ReplaceTextureSpecificationDeserializer>().AsSingleton();
            containerDefinition.Bind<TextureReplacementService>().AsSingleton();
        }
    }
}