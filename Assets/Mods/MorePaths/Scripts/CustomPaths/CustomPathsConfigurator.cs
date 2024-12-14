using Bindito.Core;
using MorePaths.Core;

namespace MorePaths.CustomPaths
{
    [Context("Game")]
    public class CustomPathsConfigurator : IConfigurator
    {
        public void Configure(IContainerDefinition containerDefinition)
        {
            // containerDefinition.Bind<CustomPathToolButtonController>().AsSingleton();
            containerDefinition.Bind<CustomPathFactory>().AsSingleton();
            containerDefinition.Bind<CustomPathGenerator>().AsSingleton();
            containerDefinition.Bind<BaseGamePathToolButtonHider>().AsSingleton();
        }
    }
}
