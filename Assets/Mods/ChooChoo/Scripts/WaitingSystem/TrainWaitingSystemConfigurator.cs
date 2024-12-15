using Bindito.Core;
using ChooChoo.NavigationSystem;
using Timberborn.TemplateSystem;
using TobbyTools.BuildingRegistrySystem;

namespace ChooChoo.WaitingSystem
{
    [Context("Game")]
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