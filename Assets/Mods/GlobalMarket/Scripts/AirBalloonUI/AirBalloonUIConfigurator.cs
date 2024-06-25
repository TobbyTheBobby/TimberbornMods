// using Bindito.Core;
// using TimberApi.ConfiguratorSystem;
// using TimberApi.SceneSystem;
// using Timberborn.BeaversUI;
// using Timberborn.TemplateSystem;
//
// namespace GlobalMarket
// {
//   [Configurator(SceneEntrypoint.InGame)]
//   public class AirBalloonUIConfigurator : IConfigurator
//   {
//     public void Configure(IContainerDefinition containerDefinition)
//     {
//       containerDefinition.MultiBind<TemplateModule>().ToProvider(ProvideTemplateModule).AsSingleton();
//     }
//     
//     private static TemplateModule ProvideTemplateModule()
//     {
//       TemplateModule.Builder builder = new TemplateModule.Builder();
//       builder.AddDecorator<GlobalMarketServant, AirBalloonEntityBadge>();
//       // builder.AddDecorator<Beaver, BeaverSelectionSound>();
//       return builder.Build();
//     }
//
//     // private class EntityPanelModuleProvider : IProvider<EntityPanelModule>
//     // {
//     //   private readonly AdulthoodFragment _adulthoodFragment;
//     //   // private readonly BeaverBuildingsFragment _beaverBuildingsFragment;
//     //
//     //   public EntityPanelModuleProvider(
//     //     AdulthoodFragment adulthoodFragment,
//     //     // BeaverBuildingsFragment beaverBuildingsFragment
//     //     )
//     //   {
//     //     this._adulthoodFragment = adulthoodFragment;
//     //     // this._beaverBuildingsFragment = beaverBuildingsFragment;
//     //   }
//     //
//     //   public EntityPanelModule Get()
//     //   {
//     //     EntityPanelModule.Builder builder = new EntityPanelModule.Builder();
//     //     builder.AddTopFragment((IEntityPanelFragment) _adulthoodFragment);
//     //     // builder.AddTopFragment((IEntityPanelFragment) this._beaverBuildingsFragment);
//     //     return builder.Build();
//     //   }
//     // }
//   }
// }
