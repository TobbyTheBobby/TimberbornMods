using Bindito.Core;
using Timberborn.ToolSystem;

namespace ChooChoo.UnlockingSystem
{
    [Context("Game")]
    public class UnlockingSystemConfigurator : IConfigurator
    {
        public void Configure(IContainerDefinition containerDefinition)
        {
            containerDefinition.MultiBind<IToolLocker>().To<TrackPieceBlockObjectToolLocker>().AsSingleton();
        }
    }
}