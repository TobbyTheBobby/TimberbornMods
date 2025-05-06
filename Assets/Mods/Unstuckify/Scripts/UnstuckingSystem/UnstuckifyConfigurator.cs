﻿using Bindito.Core;
using Timberborn.Beavers;
using Timberborn.Bots;
using Timberborn.EntityPanelSystem;
using Timberborn.TemplateSystem;

namespace Unstuckify.UnstuckingSystem
{
    [Context("Game")]
    public class UnstuckifyConfigurator : IConfigurator
    {
        public void Configure(IContainerDefinition containerDefinition)
        {
            containerDefinition.Bind<UnstuckifyService>().AsSingleton();
            containerDefinition.Bind<UnstuckifyFragment>().AsSingleton();
            containerDefinition.MultiBind<TemplateModule>().ToProvider(ProvideTemplateModule).AsSingleton();
            containerDefinition.MultiBind<EntityPanelModule>().ToProvider<EntityPanelModuleProvider>().AsSingleton();
        }

        private static TemplateModule ProvideTemplateModule()
        {
            var builder = new TemplateModule.Builder();
            builder.AddDecorator<BeaverSpec, UnstuckifyComponent>();
            builder.AddDecorator<BotSpec, UnstuckifyComponent>();
            return builder.Build();
        }

        private class EntityPanelModuleProvider : IProvider<EntityPanelModule>
        {
            private readonly UnstuckifyFragment _unstuckifyFragment;

            public EntityPanelModuleProvider(UnstuckifyFragment unstuckifyFragment)
            {
                _unstuckifyFragment = unstuckifyFragment;
            }

            public EntityPanelModule Get()
            {
                var builder = new EntityPanelModule.Builder();
                builder.AddMiddleFragment(_unstuckifyFragment);
                return builder.Build();
            }
        }
    }
}