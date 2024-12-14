using Bindito.Core;
using ChooChoo.GoodsStations;
using Timberborn.EntityPanelSystem;
using Timberborn.TemplateSystem;

namespace ChooChoo.GoodsStationUI
{
    [Context("Game")]
    public class GoodsStationUIConfigurator : IConfigurator
    {
        public void Configure(IContainerDefinition containerDefinition)
        {
            containerDefinition.Bind<GoodsStationFragment>().AsSingleton();

            containerDefinition.Bind<GoodsStationSendingInventoryFragment>().AsSingleton();
            containerDefinition.Bind<GoodsStationReceivingInventoryFragment>().AsSingleton();

            containerDefinition.Bind<GoodsStationInventoryDebugFragment>().AsSingleton();

            containerDefinition.MultiBind<EntityPanelModule>().ToProvider<EntityPanelModuleProvider>().AsSingleton();
            containerDefinition.MultiBind<TemplateModule>().ToProvider(ProvideTemplateModule).AsSingleton();
        }

        private static TemplateModule ProvideTemplateModule()
        {
            var builder = new TemplateModule.Builder();
            builder.AddDecorator<GoodsStation, GoodsStationEntityBadge>();
            return builder.Build();
        }

        private class EntityPanelModuleProvider : IProvider<EntityPanelModule>
        {
            private readonly GoodsStationFragment _goodsStationFragment;
            private readonly GoodsStationInventoryDebugFragment _goodsStationInventoryDebugFragment;
            private readonly GoodsStationSendingInventoryFragment _goodsStationSendingInventoryFragment;
            private readonly GoodsStationReceivingInventoryFragment _goodsStationReceivingInventoryFragment;

            public EntityPanelModuleProvider(
                GoodsStationFragment goodsStationFragment,
                GoodsStationInventoryDebugFragment goodsStationInventoryDebugFragment,
                GoodsStationSendingInventoryFragment goodsStationSendingInventoryFragment,
                GoodsStationReceivingInventoryFragment goodsStationReceivingInventoryFragment
            )
            {
                _goodsStationFragment = goodsStationFragment;
                _goodsStationInventoryDebugFragment = goodsStationInventoryDebugFragment;
                _goodsStationSendingInventoryFragment = goodsStationSendingInventoryFragment;
                _goodsStationReceivingInventoryFragment = goodsStationReceivingInventoryFragment;
            }

            public EntityPanelModule Get()
            {
                var builder = new EntityPanelModule.Builder();
                builder.AddSideFragment(_goodsStationFragment);
                builder.AddBottomFragment(_goodsStationInventoryDebugFragment);
                builder.AddBottomFragment(_goodsStationSendingInventoryFragment);
                builder.AddBottomFragment(_goodsStationReceivingInventoryFragment);
                return builder.Build();
            }
        }
    }
}