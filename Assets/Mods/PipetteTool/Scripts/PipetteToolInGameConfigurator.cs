using Bindito.Core;
using TimberApi.ConfiguratorSystem;
using TimberApi.SceneSystem;
using TimberApi.ToolSystem;

namespace PipetteTool
{
    [Configurator(SceneEntrypoint.InGame)]
    public class PipetteToolInGameConfigurator : IConfigurator
    {
        public void Configure(IContainerDefinition containerDefinition)
        {
            containerDefinition.Bind<IPipetteTool>().To<PipetteToolInGame>().AsSingleton();
            containerDefinition.MultiBind<IToolFactory>().To<PipetteToolFactory>().AsSingleton();
            // containerDefinition.Bind<CustomSelectableObjectRaycaster>().AsSingleton();
        }
    }
    
    // [Configurator(SceneEntrypoint.MapEditor)]
    // public class PipetteToolMapEditorConfigurator : IConfigurator
    // {
    //     public void Configure(IContainerDefinition containerDefinition)
    //     {
    //         containerDefinition.Bind<IPipetteTool>().To<PipetteTool>().AsSingleton();
    //         containerDefinition.MultiBind<IToolFactory>().To<PipetteToolFactory>().AsSingleton();
    //         containerDefinition.Bind<CustomSelectableObjectRaycaster>().AsSingleton();
    //     }
    // }
}