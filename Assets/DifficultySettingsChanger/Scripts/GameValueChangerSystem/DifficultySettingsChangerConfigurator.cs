using System;
using Bindito.Core;
using TimberApi.ConfiguratorSystem;
using TimberApi.SceneSystem;

namespace DifficultySettingsChanger
{
    [Configurator(SceneEntrypoint.InGame)]
    // [RequiredModDependencies("tobbert.choochoo")]
    public class GameValueChangerSystemConfigurator : IConfigurator
    {
        public void Configure(IContainerDefinition containerDefinition)
        {
            try
            {
                // containerDefinition.Bind<TrackRouteObjectSerializer>().AsSingleton();
            }
            catch (Exception e)
            {
                Plugin.Log.LogError(e + "");
            }

            // containerDefinition.MultiBind<IGameValueChangerGenerator>().To<DifficultySettingsGenerator>().AsSingleton();
            containerDefinition.MultiBind<IGameValueChangerGenerator>().To<WindSettingsGenerator>().AsSingleton();
            
            containerDefinition.Bind<GameValueSpecificationRepository>().AsSingleton();
            containerDefinition.Bind<GameValuesSpecificationDeserializer>().AsSingleton();
            containerDefinition.Bind<GameValueSpecificationDeserializer>().AsSingleton();
            containerDefinition.Bind<GameValueChangerRepository>().AsSingleton();
            containerDefinition.Bind<GameValueChangerService>().AsSingleton();
        }
    }
}