// using Bindito.Core;
// using TimberApi.ConfiguratorSystem;
// using TimberApi.SceneSystem;
// using Timberborn.PrefabSystem;
//
// namespace GlobalMarket
// {
//   [Configurator(SceneEntrypoint.InGame)]
//   public class AirBalloonConfigurator : IConfigurator
//   {
//     public void Configure(IContainerDefinition containerDefinition)
//     {
//       containerDefinition.MultiBind<IObjectCollection>().To<AirBalloonObjectCollector>().AsSingleton();
//       containerDefinition.Bind<PilotCharacterFactory>().AsSingleton();
//     }
//   }
// }
