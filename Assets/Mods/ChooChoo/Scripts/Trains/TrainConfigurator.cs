using Bindito.Core;
using TimberApi.SceneSystem;
using Timberborn.BehaviorSystem;
using Timberborn.PrefabSystem;
using Timberborn.TemplateSystem;
using TobbyTools.UsedImplicitlySystem;

namespace ChooChoo.Trains
{
  [Configurator(SceneEntrypoint.InGame)]
  public class TrainsConfigurator : IConfigurator
  {
    public void Configure(IContainerDefinition containerDefinition)
    {
      containerDefinition.Bind<StaticWindService>().AsSingleton();
      containerDefinition.MultiBind<IObjectCollection>().To<TrainObjectCollector>().AsSingleton();
      // containerDefinition.MultiBind<TemplateModule>().ToProvider<TemplateModuleProvider>().AsSingleton();
    }

    private class TemplateModuleProvider : IProvider<TemplateModule>
    {
      public TemplateModule Get()
      {
        var builder = new TemplateModule.Builder();
        builder.AddDecorator<WaitExecutor, TrainSmokeController>();
        return builder.Build();
      }
    }
  }
}
