using Bindito.Core;
using ChooChoo.MovementSystem;
using ChooChoo.NavigationSystem;
using TimberApi.SceneSystem;
using Timberborn.Coordinates;
using Timberborn.Persistence;
using Timberborn.TemplateSystem;
using TobbyTools.UsedImplicitlySystem;

namespace ChooChoo.TrackSystem
{
    [Configurator(SceneEntrypoint.InGame)]
    public class TrackSystemConfigurator : IConfigurator
    {
        public void Configure(IContainerDefinition containerDefinition)
        {
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