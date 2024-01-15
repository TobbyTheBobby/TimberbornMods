using Bindito.Core;
using TimberApi.SceneSystem;
using Timberborn.ToolSystem;
using TobbyTools.UsedImplicitlySystem;

namespace ChooChoo.UnlockingSystem
{
    [Configurator(SceneEntrypoint.InGame)]
    public class UnlockingSystemConfigurator : IConfigurator
    {
        public void Configure(IContainerDefinition containerDefinition)
        {
            containerDefinition.MultiBind<IToolLocker>().To<TrackPieceBlockObjectToolLocker>().AsSingleton();
        }
    }
}