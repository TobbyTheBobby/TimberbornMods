using Bindito.Core;
using ChooChoo.Trains;
using Timberborn.EntityPanelSystem;
using Timberborn.TemplateSystem;

namespace ChooChoo.TrainsUI
{
  [Context("Game")]
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
      var builder = new TemplateModule.Builder();
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
        var builder = new EntityPanelModule.Builder();
        builder.AddMiddleFragment(_trainTypeSelectorFragment);
        return builder.Build();
      }
    }
  }
}
