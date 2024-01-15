using Bindito.Core;
using ChooChoo.TrainsUI;
using ChooChoo.Wagons;
using TimberApi.SceneSystem;
using Timberborn.EntityPanelSystem;
using Timberborn.TemplateSystem;
using TobbyTools.UsedImplicitlySystem;

namespace ChooChoo.WagonUI
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
      var builder = new TemplateModule.Builder();
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
        var builder = new EntityPanelModule.Builder();
        builder.AddBottomFragment(_wagonTypeSelectorFragment);
        return builder.Build();
      }
    }
  }
}
