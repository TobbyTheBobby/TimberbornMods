using Bindito.Core;
using Timberborn.Emptying;
using Timberborn.Hauling;
using Timberborn.TemplateSystem;
using Timberborn.Workshops;

namespace ChooChoo.TrainYards
{
    [Context("Game")]
    public class TrainYardConfigurator : IConfigurator
    {
        public void Configure(IContainerDefinition containerDefinition)
        {
            containerDefinition.Bind<TrainYardInventoryInitializer>().AsSingleton();
            containerDefinition.Bind<TrainYardService>().AsSingleton();
            // containerDefinition.MultiBind<IPreviewsValidator>().To<TrainYardPreviewsValidator>().AsSingleton();
            containerDefinition.MultiBind<TemplateModule>().ToProvider<TemplateModuleProvider>().AsSingleton();
        }

        private class TemplateModuleProvider : IProvider<TemplateModule>
        {
            private readonly TrainYardInventoryInitializer _trainYardInventoryInitializer;

            public TemplateModuleProvider(
                TrainYardInventoryInitializer trainYardInventoryInitializer)
            {
                _trainYardInventoryInitializer = trainYardInventoryInitializer;
            }

            public TemplateModule Get()
            {
                var builder = new TemplateModule.Builder();
                builder.AddDecorator<TrainYard, TrainYardDescriber>();
                builder.AddDedicatedDecorator(_trainYardInventoryInitializer);
                builder.AddDecorator<TrainYard, Emptiable>();
                builder.AddDecorator<TrainYard, HaulCandidate>();
                // builder.AddDecorator<TrainYard, FillGoodsStationHaulBehaviorProvider>();
                InitializeBehaviors(builder);
                return builder.Build();
            }

            private static void InitializeBehaviors(TemplateModule.Builder builder)
            {
                // builder.AddDecorator<TrainYard, FillGoodsStationBehavior>();
                builder.AddDecorator<TrainYard, FillInputWorkplaceBehavior>();
                builder.AddDecorator<TrainYard, EmptyInventoriesWorkplaceBehavior>();
                builder.AddDecorator<TrainYard, RemoveUnwantedStockWorkplaceBehavior>();
            }
        }
    }
}