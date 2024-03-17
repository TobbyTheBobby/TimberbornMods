using System.Security.Permissions;
using Bindito.Core;
using TimberApi.SceneSystem;
using Timberborn.Emptying;
using Timberborn.EntityPanelSystem;
using Timberborn.Hauling;
using Timberborn.TemplateSystem;
using TobbyTools.UsedImplicitlySystem;

#pragma warning disable CS0618
[assembly: SecurityPermission(action: SecurityAction.RequestMinimum, SkipVerification = true)]
#pragma warning restore CS0618

namespace TobbyTools.SpecificGoodsStockpileSystem
{
    [Configurator(SceneEntrypoint.InGame)]
    internal class SpecificGoodsStockpileSystemConfigurator : IConfigurator
    {
        public void Configure(IContainerDefinition containerDefinition)
        {
            containerDefinition.Bind<SpecificGoodsStockpileInventoryInitializer>().AsSingleton();
            containerDefinition.Bind<SpecificGoodsStockpileFragmentInventory>().AsSingleton();
            containerDefinition.Bind<SpecificGoodsStockpileInventoryFragment>().AsSingleton();
            containerDefinition.MultiBind<EntityPanelModule>().ToProvider<EntityPanelModuleProvider>().AsSingleton();
            containerDefinition.MultiBind<TemplateModule>().ToProvider<TemplateModuleProvider>().AsSingleton();
        }

        public class TemplateModuleProvider : IProvider<TemplateModule>
        {
            private readonly SpecificGoodsStockpileInventoryInitializer _specificGoodsStockpileInventoryInitializer;

            public TemplateModuleProvider(SpecificGoodsStockpileInventoryInitializer stockpileInventoryInitializer)
            {
                _specificGoodsStockpileInventoryInitializer = stockpileInventoryInitializer;
            }

            public TemplateModule Get()
            {
                var builder = new TemplateModule.Builder();
                builder.AddDedicatedDecorator(_specificGoodsStockpileInventoryInitializer);
                builder.AddDecorator<SpecificGoodsStockpile, Emptiable>();
                builder.AddDecorator<SpecificGoodsStockpile, HaulCandidate>();
                InitializeBehaviors(builder);
                return builder.Build();
            }

            private static void InitializeBehaviors(TemplateModule.Builder builder)
            {
                builder.AddDecorator<SpecificGoodsStockpile, EmptyInventoriesWorkplaceBehavior>();
                builder.AddDecorator<SpecificGoodsStockpile, RemoveUnwantedStockWorkplaceBehavior>();
            }
        }

        private class EntityPanelModuleProvider : IProvider<EntityPanelModule>
        {
            private readonly SpecificGoodsStockpileInventoryFragment _specificGoodsStockpileInventoryFragment;

            public EntityPanelModuleProvider(SpecificGoodsStockpileInventoryFragment specificGoodsStockpileInventoryFragment)
            {
                _specificGoodsStockpileInventoryFragment = specificGoodsStockpileInventoryFragment;
            }

            public EntityPanelModule Get()
            {
                var builder = new EntityPanelModule.Builder();
                builder.AddBottomFragment(_specificGoodsStockpileInventoryFragment);
                return builder.Build();
            }
        }
    }
}