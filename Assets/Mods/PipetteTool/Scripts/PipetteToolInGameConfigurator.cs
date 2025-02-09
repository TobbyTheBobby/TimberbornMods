using Bindito.Core;
using Timberborn.BottomBarSystem;

namespace PipetteTool
{
    [Context("Game")]
    public class PipetteToolInGameConfigurator : Configurator
    {
        protected override void Configure()
        {
            Bind<IPipetteTool>().To<PipetteToolInGame>().AsSingleton();
            Bind<PipetteToolButton>().AsSingleton();
            // MultiBind<IToolFactory>().To<PipetteToolFactory>().AsSingleton();
            // Bind<CustomSelectableObjectRaycaster>().AsSingleton();
            MultiBind<BottomBarModule>().ToProvider<BottomBarModuleProvider>().AsSingleton();
        }

        public class BottomBarModuleProvider : IProvider<BottomBarModule>
        {
            private readonly PipetteToolButton _pipetteToolButton;
        
            public BottomBarModuleProvider(PipetteToolButton pipetteToolButton)
            {
                _pipetteToolButton = pipetteToolButton;
            }
        
            public BottomBarModule Get()
            {
                var builder = new BottomBarModule.Builder();
                builder.AddLeftSectionElement(_pipetteToolButton, 1234);
                return builder.Build();
            }
        }
    }

    [Context("MapEditor")]
    public class PipetteToolMapEditorConfigurator : Configurator
    {
        protected override void Configure()
        {
            Bind<IPipetteTool>().To<PipetteTool>().AsSingleton();
            Bind<PipetteToolButton>().AsSingleton();
            // MultiBind<IToolFactory>().To<PipetteToolFactory>().AsSingleton();
            // Bind<CustomSelectableObjectRaycaster>().AsSingleton();
            MultiBind<BottomBarModule>().ToProvider<BottomBarModuleProvider>().AsSingleton();
        }
        
        public class BottomBarModuleProvider : IProvider<BottomBarModule>
        {
            private readonly PipetteToolButton _pipetteToolButton;
        
            public BottomBarModuleProvider(PipetteToolButton pipetteToolButton)
            {
                _pipetteToolButton = pipetteToolButton;
            }
        
            public BottomBarModule Get()
            {
                var builder = new BottomBarModule.Builder();
                builder.AddLeftSectionElement(_pipetteToolButton, 215);
                return builder.Build();
            }
        }
    }
}