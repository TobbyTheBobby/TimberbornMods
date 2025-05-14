using Bindito.Core;
using Timberborn.BottomBarSystem;

namespace PipetteTool.PipetteToolSystem
{
    [Context("Game")]
    [Context("MapEditor")]
    public class PipetteToolInGameConfigurator : Configurator
    {
        protected override void Configure()
        {
            MultiBind<IPipetteToolMode>().To<PressMode>().AsSingleton();
            MultiBind<IPipetteToolMode>().To<HoldMode>().AsSingleton();
            MultiBind<IPipetteToolMode>().To<DirectModeKeyBind>().AsSingleton();
            MultiBind<IPipetteToolMode>().To<DirectModeMiddleMouseButton>().AsSingleton();
            MultiBind<IPipetteToolMode>().To<InvertedMode>().AsSingleton();
            Bind<PipetteToolModeManager>().AsSingleton();
            Bind<PipetteTool>().AsSingleton();
            Bind<PipetteToolButton>().AsSingleton();
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
}