using Bindito.Core;
using TimberApi.Tools.ToolSystem;

namespace PipetteTool
{
    [Context("Game")]
    public class PipetteToolInGameConfigurator : IConfigurator
    {
        public void Configure(IContainerDefinition containerDefinition)
        {
            containerDefinition.Bind<IPipetteTool>().To<PipetteToolInGame>().AsSingleton();
            // containerDefinition.Bind<PipetteToolButton>().AsSingleton();
            containerDefinition.MultiBind<IToolFactory>().To<PipetteToolFactory>().AsSingleton();
            // containerDefinition.Bind<CustomSelectableObjectRaycaster>().AsSingleton();
            // containerDefinition.MultiBind<BottomBarModule>().ToProvider<BottomBarModuleProvider>().AsSingleton();
        }

        // public class BottomBarModuleProvider : IProvider<BottomBarModule>
        // {
        //     private readonly PipetteToolButton _pipetteToolButton;
        //
        //     public BottomBarModuleProvider(PipetteToolButton pipetteToolButton)
        //     {
        //         _pipetteToolButton = pipetteToolButton;
        //     }
        //
        //     public BottomBarModule Get()
        //     {
        //         var builder = new BottomBarModule.Builder();
        //         builder.AddLeftSectionElement(_pipetteToolButton, 1234);
        //         return builder.Build();
        //     }
        // }
    }

    [Context("MapEditor")]
    public class PipetteToolMapEditorConfigurator : IConfigurator
    {
        public void Configure(IContainerDefinition containerDefinition)
        {
            containerDefinition.Bind<IPipetteTool>().To<PipetteTool>().AsSingleton();
            // containerDefinition.Bind<PipetteToolButton>().AsSingleton();
            containerDefinition.MultiBind<IToolFactory>().To<PipetteToolFactory>().AsSingleton();
            // containerDefinition.Bind<CustomSelectableObjectRaycaster>().AsSingleton();
            // containerDefinition.MultiBind<BottomBarModule>().ToProvider<BottomBarModuleProvider>().AsSingleton();
        }
        
        // public class BottomBarModuleProvider : IProvider<BottomBarModule>
        // {
        //     private readonly PipetteToolButton _pipetteToolButton;
        //
        //     public BottomBarModuleProvider(PipetteToolButton pipetteToolButton)
        //     {
        //         _pipetteToolButton = pipetteToolButton;
        //     }
        //
        //     public BottomBarModule Get()
        //     {
        //         var builder = new BottomBarModule.Builder();
        //         builder.AddLeftSectionElement(_pipetteToolButton, 215);
        //         return builder.Build();
        //     }
        // }
    }
}