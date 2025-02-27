﻿using Bindito.Core;
using ChooChoo.Debugging;
using ChooChoo.TrackSystem;
using Timberborn.EntityPanelSystem;
using Timberborn.TemplateSystem;

namespace ChooChoo.TrackSystemUI
{
    [Context("Game")]
    public class TrackSystemUIConfigurator : IConfigurator
    {
        public void Configure(IContainerDefinition containerDefinition)
        {
            containerDefinition.Bind<TrackSectionDividerFragmentPreset>().AsTransient();
            
            containerDefinition.Bind<TrackSectionDividerFragment>().AsSingleton();
            containerDefinition.Bind<TrackPieceDebugger>().AsSingleton();
            containerDefinition.MultiBind<EntityPanelModule>().ToProvider<EntityPanelModuleProvider>().AsSingleton();
            containerDefinition.MultiBind<TemplateModule>().ToProvider(ProvideTemplateModule).AsSingleton();
        }

        private static TemplateModule ProvideTemplateModule()
        {
            var builder = new TemplateModule.Builder();
            builder.AddDecorator<TrackPiece, TileTrackConnectionMarkerDrawer>();
            builder.AddDecorator<OneWayTrack, TileTrackConnectionMarkerDrawer>();
            return builder.Build();
        }

        private class EntityPanelModuleProvider : IProvider<EntityPanelModule>
        {
            private readonly TrackSectionDividerFragment _trackSectionDividerFragment;
            
            public EntityPanelModuleProvider(TrackSectionDividerFragment trackSectionDividerFragment)
            {
                _trackSectionDividerFragment = trackSectionDividerFragment;
            }

            public EntityPanelModule Get()
            {
                var builder = new EntityPanelModule.Builder();
                builder.AddMiddleFragment(_trackSectionDividerFragment);
                return builder.Build();
            }
        }
    }
}