using Bindito.Core;
using TimberApi.SceneSystem;
using Timberborn.TemplateSystem;
using TobbyTools.UsedImplicitlySystem;

namespace ChooChoo.Wagons
{
  [Configurator(SceneEntrypoint.InGame)]
  public class WagonsConfigurator : IConfigurator
  {
    public void Configure(IContainerDefinition containerDefinition)
    {
      containerDefinition.Bind<ObjectFollowerFactory>().AsSingleton();
      containerDefinition.Bind<WagonInitializer>().AsSingleton();
      containerDefinition.MultiBind<TemplateModule>().ToProvider(ProvideTemplateModule).AsSingleton();
    }

    private static TemplateModule ProvideTemplateModule()
    {
      var builder = new TemplateModule.Builder();
      builder.AddDecorator<TrainWagon, TrainWagonGoodsManager>();
      return builder.Build();
    }
  }
}
