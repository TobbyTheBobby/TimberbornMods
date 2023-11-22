using Bindito.Core;
using TimberApi.ConfiguratorSystem;
using TimberApi.SceneSystem;
using Timberborn.Planting;
using Timberborn.TemplateSystem;

namespace PlantingSeeds
{
    [Configurator(SceneEntrypoint.InGame)]
    public class PlantableSeedConfigurator : IConfigurator
    {
        public void Configure(IContainerDefinition containerDefinition)
        {
            containerDefinition.Bind<SeedReceiverInventoryInitializer>().AsSingleton();
            containerDefinition.Bind<PlantableSeedSpecificationDeserializer>().AsSingleton();
            containerDefinition.Bind<PlantableSeedSpecificationApplier>().AsSingleton();
            containerDefinition.MultiBind<TemplateModule>().ToProvider<TemplateModuleProvider>().AsSingleton();
        }

        private class TemplateModuleProvider : IProvider<TemplateModule>
        {
            private readonly SeedReceiverInventoryInitializer _seedReceiverInventoryInitializer;
            
            TemplateModuleProvider(SeedReceiverInventoryInitializer seedReceiverInventoryInitializer)
            {
                _seedReceiverInventoryInitializer = seedReceiverInventoryInitializer;
            }
            
            public TemplateModule Get()
            {
                TemplateModule.Builder builder = new TemplateModule.Builder();
                builder.AddDecorator<Planter, PlantSeedBehavior>();
                builder.AddDedicatedDecorator(_seedReceiverInventoryInitializer);
                return builder.Build();
            }
        }
    }
}