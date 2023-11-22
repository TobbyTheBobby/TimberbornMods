using Bindito.Core;
using TimberApi.ConfiguratorSystem;
using TimberApi.SceneSystem;
using Timberborn.EntityPanelSystem;
using Timberborn.TemplateSystem;

namespace ChooChoo
{
  [Configurator(SceneEntrypoint.InGame)]
  public class TrainsUIConfigurator : IConfigurator
  {
    public void Configure(IContainerDefinition containerDefinition)
    {
      containerDefinition.Bind<TrainTypeSelectorFragment>().AsSingleton();
      containerDefinition.Bind<TrainTypeDropdownProvider>().AsSingleton();
      containerDefinition.MultiBind<TemplateModule>().ToProvider(ProvideTemplateModule).AsSingleton();
      containerDefinition.MultiBind<EntityPanelModule>().ToProvider<EntityPanelModuleProvider>().AsSingleton();
    }
    
    private static TemplateModule ProvideTemplateModule()
    {
      TemplateModule.Builder builder = new TemplateModule.Builder();
      builder.AddDecorator<Train, TrainEntityBadge>();
      builder.AddDecorator<Train, TrainTypeDropdownProvider>();
      return builder.Build();
    }
    
    private class EntityPanelModuleProvider : IProvider<EntityPanelModule>
    {
      private readonly TrainTypeSelectorFragment _trainTypeSelectorFragment;

      public EntityPanelModuleProvider(TrainTypeSelectorFragment trainTypeSelectorFragment)
      {
        _trainTypeSelectorFragment = trainTypeSelectorFragment;
      }

      public EntityPanelModule Get()
      {
        EntityPanelModule.Builder builder = new EntityPanelModule.Builder();
        builder.AddMiddleFragment(_trainTypeSelectorFragment);
        return builder.Build();
      }
    }
  }
}
