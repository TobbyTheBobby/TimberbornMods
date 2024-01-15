using Bindito.Core;
using ChooChoo.NavigationSystem;
using TimberApi.SceneSystem;
using Timberborn.TemplateSystem;
using TobbyTools.UsedImplicitlySystem;

namespace ChooChoo.NavigationSystemUI
{
    [Configurator(SceneEntrypoint.InGame)]
    public class TrainNavigationSystemUIConfigurator : IConfigurator
    {
        public void Configure(IContainerDefinition containerDefinition)
        {
            containerDefinition.MultiBind<TemplateModule>().ToProvider(ProvideTemplateModule).AsSingleton();
        }

        private static TemplateModule ProvideTemplateModule()
        {
            var builder = new TemplateModule.Builder();
            builder.AddDecorator<TrainDestination, UnconnectedTrainDestinationStatus>();
            return builder.Build();
        }
    }
}