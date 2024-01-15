using Bindito.Core;
using ChooChoo.NavigationSystem;
using TimberApi.SceneSystem;
using Timberborn.TemplateSystem;
using TobbyTools.BuildingRegistrySystem;
using TobbyTools.UsedImplicitlySystem;

namespace ChooChoo.WaitingSystem
{
    [Configurator(SceneEntrypoint.InGame)]
    public class TrainWaitingSystemConfigurator : IConfigurator
    {
        public void Configure(IContainerDefinition containerDefinition)
        {
            containerDefinition.Bind<BuildingRegistry<TrainWaitingLocation>>().AsSingleton();
            containerDefinition.Bind<ClosestTrainWaitingLocationPicker>().AsSingleton();
            containerDefinition.MultiBind<TemplateModule>().ToProvider(ProvideTemplateModule).AsSingleton();
        }

        private static TemplateModule ProvideTemplateModule()
        {
            var builder = new TemplateModule.Builder();
            builder.AddDecorator<TrainWaitingLocation, TrainDestination>();
            return builder.Build();
        }
    }
}