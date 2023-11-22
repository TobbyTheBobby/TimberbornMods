// using Bindito.Core;
// using TimberApi.ConfiguratorSystem;
// using TimberApi.SceneSystem;
// using Timberborn.EmptyingUI;
// using Timberborn.EntityPanelSystem;
//
// namespace GlobalMarket
// {
//   [Configurator(SceneEntrypoint.InGame)]
//   public class GlobalMarketUIConfigurator : IConfigurator
//   {
//     public void Configure(IContainerDefinition containerDefinition)
//     {
//       containerDefinition.Bind<AirBalloonFragment>().AsSingleton();
//       containerDefinition.MultiBind<EntityPanelModule>().ToProvider<EntityPanelModuleProvider>().AsSingleton();
//     }
//
//     private class EntityPanelModuleProvider : IProvider<EntityPanelModule>
//     {
//       private readonly AirBalloonFragment _airBalloonFragment;
//
//       public EntityPanelModuleProvider(AirBalloonFragment airBalloonFragment) => _airBalloonFragment = airBalloonFragment;
//
//       public EntityPanelModule Get()
//       {
//         EntityPanelModule.Builder builder = new EntityPanelModule.Builder();
//         builder.AddBottomFragment(_airBalloonFragment);
//         return builder.Build();
//       }
//     }
//   }
// }
