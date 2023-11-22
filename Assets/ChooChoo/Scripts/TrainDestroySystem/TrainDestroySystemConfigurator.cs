using Bindito.Core;
using TimberApi.ConfiguratorSystem;
using TimberApi.SceneSystem;
using Timberborn.EntityPanelSystem;
using Timberborn.TemplateSystem;

namespace ChooChoo
{
  [Configurator(SceneEntrypoint.InGame)]
  public class TrainDestroySystemConfigurator : IConfigurator
  {
    public void Configure(IContainerDefinition containerDefinition)
    {
      containerDefinition.Bind<TrainDestroyerFragment>().AsSingleton();
      containerDefinition.Bind<DeleteTrainBoxShower>().AsSingleton();
      containerDefinition.MultiBind<TemplateModule>().ToProvider(ProvideTemplateModule).AsSingleton();
      containerDefinition.MultiBind<EntityPanelModule>().ToProvider<EntityPanelModuleProvider>().AsSingleton();
    }
    
    private static TemplateModule ProvideTemplateModule()
    {
      TemplateModule.Builder builder = new TemplateModule.Builder();
      builder.AddDecorator<Train, Destroyable>();
      return builder.Build();
    }
    
    private class EntityPanelModuleProvider : IProvider<EntityPanelModule>
    {
      private readonly TrainDestroyerFragment _trainDestroyerFragment;

      public EntityPanelModuleProvider(TrainDestroyerFragment trainDestroyerFragment)
      {
        _trainDestroyerFragment = trainDestroyerFragment;
      }

      public EntityPanelModule Get()
      {
        EntityPanelModule.Builder builder = new EntityPanelModule.Builder();
        builder.AddLeftHeaderFragment(_trainDestroyerFragment);
        return builder.Build();
      }
    }
  }
}
