using Bindito.Core;
using TimberApi.ConfiguratorSystem;
using TimberApi.SceneSystem;
using Timberborn.PathSystem;
using Timberborn.TemplateSystem;

namespace MorePaths
{
    [Configurator(SceneEntrypoint.InGame)]
    public class CustomPathsConfigurator : IConfigurator
    {
        public void Configure(IContainerDefinition containerDefinition)
        {
            containerDefinition.Bind<DrivewayFactory>().AsSingleton();
            containerDefinition.Bind<DrivewayService>().AsSingleton();
            
            containerDefinition.Bind<CustomPathFactory>().AsSingleton();

            containerDefinition.MultiBind<TemplateModule>().ToProvider(ProvideTemplateModule).AsSingleton();
        }

        private TemplateModule ProvideTemplateModule()
        {
            TemplateModule.Builder builder = new TemplateModule.Builder();
            builder.AddDecorator<DrivewayModel, CustomDrivewayModel>();
            return builder.Build();
        }
    }
}
