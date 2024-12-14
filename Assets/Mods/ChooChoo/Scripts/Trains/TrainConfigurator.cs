using Bindito.Core;
using Timberborn.BehaviorSystem;
using Timberborn.TemplateSystem;

namespace ChooChoo.Trains
{
  [Context("Game")]
  public class TrainsConfigurator : IConfigurator
  {
    public void Configure(IContainerDefinition containerDefinition)
    {
      containerDefinition.Bind<StaticWindService>().AsSingleton();
      // containerDefinition.MultiBind<IObjectCollection>().To<TrainObjectCollector>().AsSingleton();
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
