using Bindito.Core;
using TimberApi.SceneSystem;
using Timberborn.TemplateSystem;
using Timberborn.WalkingSystem;
using TobbyTools.UsedImplicitlySystem;

namespace ChooChoo.BeaverNavigationUtilities
{
    [Configurator(SceneEntrypoint.InGame)]
    public class BeaverNavigationUtilitiesConfigurator : IConfigurator
    {
        public void Configure(IContainerDefinition containerDefinition)
        {
            containerDefinition.Bind<PathCornerBlockObjectRepository>().AsSingleton();
            // containerDefinition.Bind<ConvertedPathRepository>().AsSingleton();
            containerDefinition.Bind<PathConverter>().AsSingleton();
            containerDefinition.Bind<PathCorrector>().AsSingleton();
            containerDefinition.MultiBind<TemplateModule>().ToProvider<TemplateModuleProvider>().AsSingleton();
        }

        private class TemplateModuleProvider : IProvider<TemplateModule>
        {
            public TemplateModule Get()
            {
                var builder = new TemplateModule.Builder();
                builder.AddDecorator<Walker, PathFollowerUtilities>();
                return builder.Build();
            }
        }
    }
}