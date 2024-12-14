using Bindito.Core;
using ChooChoo.Trains;
using Timberborn.EntityPanelSystem;
using Timberborn.TemplateSystem;

namespace ChooChoo.DestroySystem
{
  [Context("Game")]
  internal class TrainDestroySystemConfigurator : IConfigurator
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
      var builder = new TemplateModule.Builder();
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
        var builder = new EntityPanelModule.Builder();
        builder.AddLeftHeaderFragment(_trainDestroyerFragment);
        return builder.Build();
      }
    }
  }
}
