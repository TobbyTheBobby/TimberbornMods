// using Bindito.Core;
// using TimberApi.ConfiguratorSystem;
// using TimberApi.SceneSystem;
//
// namespace TobbyTools.TextureReplacementTool
// {
//     [Configurator(SceneEntrypoint.InGame)]
//     public class TextureReplacementToolConfigurator : IConfigurator
//     {
//         public void Configure(IContainerDefinition containerDefinition)
//         {
//             containerDefinition.Bind<ImageRepository>().AsSingleton();
//             containerDefinition.Bind<ReplaceTextureSpecificationDeserializer>().AsSingleton();
//             containerDefinition.Bind<TextureReplacementService>().AsSingleton();
//         }
//     }
// }