using Bindito.Core;
using TimberApi.SceneSystem;
using Timberborn.Beavers;
using Timberborn.Meshy;
using Timberborn.TemplateSystem;
using TobbyTools.UsedImplicitlySystem;

namespace BeaverClothing
{
    [Configurator(SceneEntrypoint.InGame)]
    public class BeaverClothingConfigurator : IConfigurator
    {
        public void Configure(IContainerDefinition containerDefinition)
        {
            containerDefinition.Bind<WorkplaceClothingSpecificationDeserializer>().AsSingleton();
            containerDefinition.Bind<ClothingSpecificationDeserializer>().AsSingleton();
            containerDefinition.Bind<BeaverClothingService>().AsSingleton();
            containerDefinition.MultiBind<IModelPostprocessor>().To<TestingModelPostProcessor>().AsSingleton();
            containerDefinition.MultiBind<TemplateModule>().ToProvider(ProvideTemplateModule).AsSingleton();
        }

        private TemplateModule ProvideTemplateModule()
        {
            var builder = new TemplateModule.Builder();
            builder.AddDecorator<Beaver, ClothingComponent>();
            return builder.Build();
        }
    }
}