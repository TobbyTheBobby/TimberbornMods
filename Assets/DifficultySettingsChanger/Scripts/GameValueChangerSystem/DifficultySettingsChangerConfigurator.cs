using System;
using Bindito.Core;
using TimberApi.SceneSystem;
using TobbyTools.UsedImplicitlySystem;

namespace DifficultySettingsChanger.GameValueChangerSystem
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