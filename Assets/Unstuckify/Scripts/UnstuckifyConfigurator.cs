using Bindito.Core;
using TimberApi.ConfiguratorSystem;
using TimberApi.SceneSystem;
using Timberborn.Beavers;
using Timberborn.Bots;
using Timberborn.EntityPanelSystem;
using Timberborn.TemplateSystem;

namespace Unstuckify
{
  [Configurator(SceneEntrypoint.InGame)]
  public class UnstuckifyConfigurator : IConfigurator
  {
    public void Configure(IContainerDefinition containerDefinition)
    {
      containerDefinition.Bind<UnstuckifyService>().AsSingleton();
      containerDefinition.Bind<UnstuckifyFragment>().AsSingleton();
      containerDefinition.MultiBind<TemplateModule>().ToProvider(ProvideTemplateModule).AsSingleton();
      containerDefinition.MultiBind<EntityPanelModule>().ToProvider<EntityPanelModuleProvider>().AsSingleton();
    }
    
    private static TemplateModule ProvideTemplateModule()
    {
      TemplateModule.Builder builder = new TemplateModule.Builder();
      builder.AddDecorator<Beaver, UnstuckifyComponent>();
      builder.AddDecorator<Bot, UnstuckifyComponent>();
      return builder.Build();
    }

    private class EntityPanelModuleProvider : IProvider<EntityPanelModule>
    {
      private readonly UnstuckifyFragment _unstuckifyFragment;

      public EntityPanelModuleProvider(UnstuckifyFragment unstuckifyFragment)
      {
        _unstuckifyFragment = unstuckifyFragment;
      }

      public EntityPanelModule Get()
      {
        EntityPanelModule.Builder builder = new EntityPanelModule.Builder();
        builder.AddMiddleFragment(_unstuckifyFragment);
        return builder.Build();
      }
    }
  }
}
