using Bindito.Core;
using ChooChoo.MovementSystem;
using ChooChoo.NavigationSystem;
using Timberborn.Coordinates;
using Timberborn.Persistence;
using Timberborn.TemplateSystem;

namespace ChooChoo.TrackSystem
{
    [Context("Game")]
    public class TrackSystemConfigurator : IConfigurator
    {
        public void Configure(IContainerDefinition containerDefinition)
        {
            containerDefinition.Bind<TracksRecalculator>().AsSingleton();
            containerDefinition.Bind<TrackArrayProvider>().AsSingleton();
            containerDefinition.Bind<TrackRouteWeightCache>().AsSingleton();
            // containerDefinition.Bind<TrackSectionService>().AsSingleton();
            containerDefinition.Bind<EnumObjectSerializer<Direction2D>>().AsSingleton();
            containerDefinition.Bind<TrackRouteObjectDeserializer>().AsSingleton();
            containerDefinition.Bind<TrackConnectionObjectDeserializer>().AsSingleton();
            containerDefinition.Bind<TrackPieceSpecificationDeserializer>().AsSingleton();
            containerDefinition.MultiBind<TemplateModule>().ToProvider(ProvideTemplateModule).AsSingleton();
        }
    
        private static TemplateModule ProvideTemplateModule()
        {
            var builder = new TemplateModule.Builder();
            builder.AddDecorator<TrackObserver, TrackSectionOccupier>();
            return builder.Build();
        }
    }
}