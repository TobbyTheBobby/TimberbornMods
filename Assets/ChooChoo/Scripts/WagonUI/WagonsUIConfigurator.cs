using Bindito.Core;
using ChooChoo;
using TimberApi.ConfiguratorSystem;
using TimberApi.SceneSystem;
using Timberborn.EntityPanelSystem;
using Timberborn.TemplateSystem;

namespace GlobalMarket
{
  [Configurator(SceneEntrypoint.InGame)]
  public class WagonsUIConfigurator : IConfigurator
  {
    public void Configure(IContainerDefinition containerDefinition)
    {
      containerDefinition.Bind<WagonTypeSelectorFragment>().AsSingleton();
      containerDefinition.MultiBind<TemplateModule>().ToProvider(ProvideTemplateModule).AsSingleton();
      containerDefinition.MultiBind<EntityPanelModule>().ToProvider<EntityPanelModuleProvider>().AsSingleton();
    }
    
    private static TemplateModule ProvideTemplateModule()
    {
      TemplateModule.Builder builder = new TemplateModule.Builder();
      builder.AddDecorator<TrainWagon, TrainEntityBadge>();
      builder.AddDecorator<TrainWagon, WagonTypeDropdownProvider>();
      builder.AddDecorator<WagonManager, TrainWagonsGoodsManager>();
      builder.AddDecorator<WagonManager, WagonMovementController>();
      return builder.Build();
    }
    
    private class EntityPanelModuleProvider : IProvider<EntityPanelModule>
    {
      private readonly WagonTypeSelectorFragment _wagonTypeSelectorFragment;

      public EntityPanelModuleProvider(WagonTypeSelectorFragment wagonTypeSelectorFragment)
      {
        _wagonTypeSelectorFragment = wagonTypeSelectorFragment;
      }

      public EntityPanelModule Get()
      {
        EntityPanelModule.Builder builder = new EntityPanelModule.Builder();
        builder.AddBottomFragment(_wagonTypeSelectorFragment);
        return builder.Build();
      }
    }
  }
}
