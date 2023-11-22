using Bindito.Core;
using TimberApi.ConfiguratorSystem;
using TimberApi.SceneSystem;
using Timberborn.EntityPanelSystem;

namespace ChooChoo
{
  [Configurator(SceneEntrypoint.InGame)]
  public class PassengerSystemUIConfigurator : IConfigurator
  {
    public void Configure(IContainerDefinition containerDefinition)
    {
      containerDefinition.Bind<PassengerDistrictObstacleFragment>().AsSingleton();
      containerDefinition.Bind<PassengerStationFragment>().AsSingleton();
      containerDefinition.Bind<PassengerWagonFragment>().AsSingleton();
      containerDefinition.Bind<PassengerViewFactory>().AsSingleton();
      containerDefinition.Bind<PathLinkPointFragment>().AsSingleton();
      containerDefinition.Bind<ConnectPathLinkPointButton>().AsSingleton();
      containerDefinition.MultiBind<EntityPanelModule>().ToProvider<EntityPanelModuleProvider>().AsSingleton();
    }

    private class EntityPanelModuleProvider : IProvider<EntityPanelModule>
    {
      private readonly PassengerDistrictObstacleFragment _passengerDistrictObstacleFragment;
      private readonly PassengerStationFragment _passengerStationFragment;
      private readonly PassengerWagonFragment _passengerWagonFragment;
      private readonly PathLinkPointFragment _pathLinkPointFragment;

      public EntityPanelModuleProvider(PassengerDistrictObstacleFragment passengerDistrictObstacleFragment, PassengerStationFragment passengerStationFragment, PassengerWagonFragment passengerWagonFragment, PathLinkPointFragment pathLinkPointFragment)
      {
        _passengerDistrictObstacleFragment = passengerDistrictObstacleFragment;
        _passengerStationFragment = passengerStationFragment;
        _passengerWagonFragment = passengerWagonFragment;
        _pathLinkPointFragment = pathLinkPointFragment;
      }

      public EntityPanelModule Get()
      {
        EntityPanelModule.Builder builder = new EntityPanelModule.Builder();
        builder.AddMiddleFragment(_passengerDistrictObstacleFragment);
        builder.AddMiddleFragment(_passengerStationFragment);
        builder.AddBottomFragment(_passengerWagonFragment);
        // builder.AddMiddleFragment(_pathLinkPointFragment);
        return builder.Build();
      }
    }
  }
}
