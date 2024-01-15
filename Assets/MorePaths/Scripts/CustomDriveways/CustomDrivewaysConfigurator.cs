using Bindito.Core;
using TimberApi.SceneSystem;
using Timberborn.PathSystem;
using Timberborn.TemplateSystem;
using TobbyTools.UsedImplicitlySystem;

namespace MorePaths.CustomDriveways
{
    [Configurator(SceneEntrypoint.InGame)]
    public class CustomDrivewaysConfigurator : IConfigurator
    {
        public void Configure(IContainerDefinition containerDefinition)
        {
            containerDefinition.Bind<DrivewayFactory>().AsSingleton();
            containerDefinition.Bind<DrivewayService>().AsSingleton();
            containerDefinition.MultiBind<TemplateModule>().ToProvider(ProvideTemplateModule).AsSingleton();
        }

        private TemplateModule ProvideTemplateModule()
        {
            var builder = new TemplateModule.Builder();
            builder.AddDecorator<DrivewayModel, CustomDrivewayModel>();
            return builder.Build();
        }
    }
}
