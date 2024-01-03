using Bindito.Core;
using TimberApi.ConfiguratorSystem;
using TimberApi.SceneSystem;
using Timberborn.ToolSystem;

namespace ChooChoo
{
  [Configurator(SceneEntrypoint.InGame)]
  public class UnlockingSystemConfigurator : IConfigurator
  {
    public void Configure(IContainerDefinition containerDefinition)
    {
      containerDefinition.Bind<UnlockedTrainService>().AsSingleton();
      containerDefinition.MultiBind<IToolLocker>().To<TrackPieceBlockObjectToolLocker>().AsSingleton();
    }
  }
}
