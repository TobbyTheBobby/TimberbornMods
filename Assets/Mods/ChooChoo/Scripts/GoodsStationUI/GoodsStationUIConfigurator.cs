using Bindito.Core;
using TimberApi.SceneSystem;
using Timberborn.EntityPanelSystem;
using TobbyTools.UsedImplicitlySystem;

namespace ChooChoo.GoodsStationUI
{
    [Configurator(SceneEntrypoint.InGame)]
    public class GoodsStationUIConfigurator : IConfigurator
    {
        public void Configure(IContainerDefinition containerDefinition)
        {
            containerDefinition.Bind<GoodsStationFragment>().AsSingleton();

            containerDefinition.Bind<GoodsStationSendingInventoryFragment>().AsSingleton();
            containerDefinition.Bind<GoodsStationReceivingInventoryFragment>().AsSingleton();

            containerDefinition.Bind<GoodsStationInventoryDebugFragment>().AsSingleton();

            containerDefinition.MultiBind<EntityPanelModule>().ToProvider<EntityPanelModuleProvider>().AsSingleton();
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