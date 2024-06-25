using Bindito.Core;
using TimberApi.SceneSystem;
using Timberborn.PrefabSystem;
using Timberborn.TemplateSystem;
using TobbyTools.UsedImplicitlySystem;

namespace TobbyTools.HookSystem
{
  [Configurator(SceneEntrypoint.InGame)]
  public class HookSystemConfigurator : IConfigurator
  {
    public void Configure(IContainerDefinition containerDefinition)
    {
      containerDefinition.Bind<HookSystemTest>().AsSingleton();
      containerDefinition.MultiBind<TemplateModule>().ToProvider(ProvideTemplateModule).AsSingleton();
    }
    
    private static TemplateModule ProvideTemplateModule()
    {
      var builder = new TemplateModule.Builder();
      builder.AddDecorator<Prefab, HookRegister>();
      return builder.Build();
    }
  }
}
