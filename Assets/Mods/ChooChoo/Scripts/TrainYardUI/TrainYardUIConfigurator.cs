using Bindito.Core;
using Timberborn.EntityPanelSystem;

namespace ChooChoo.TrainYardUI
{
    [Context("Game")]
    public class TrainYardUIConfigurator : IConfigurator
    {
        public void Configure(IContainerDefinition containerDefinition)
        {
            containerDefinition.Bind<TrainYardFragmentPreset>().AsTransient();
            
            containerDefinition.Bind<TrainYardFragment>().AsSingleton();
            containerDefinition.MultiBind<EntityPanelModule>().ToProvider<EntityPanelModuleProvider>().AsSingleton();
        }

        private class EntityPanelModuleProvider : IProvider<EntityPanelModule>
        {
            private readonly TrainYardFragment _trainYardFragment;
            
            public EntityPanelModuleProvider(TrainYardFragment trainYardFragment)
            {
                _trainYardFragment = trainYardFragment;
            }

            public EntityPanelModule Get()
            {
                var builder = new EntityPanelModule.Builder();
                builder.AddBottomFragment(_trainYardFragment);
                return builder.Build();
            }
        }
    }
}