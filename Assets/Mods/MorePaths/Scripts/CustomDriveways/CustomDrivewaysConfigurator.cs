using Bindito.Core;
using Timberborn.PathSystem;
using Timberborn.TemplateSystem;

namespace MorePaths.CustomDriveways
{
    [Context("Game")]
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
