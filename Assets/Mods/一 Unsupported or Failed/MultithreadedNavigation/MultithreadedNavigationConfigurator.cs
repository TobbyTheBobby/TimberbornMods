using Bindito.Core;

namespace MultithreadedNavigation
{
    public class MultithreadedNavigationConfigurator : IConfigurator
    {
        public void Configure(IContainerDefinition containerDefinition)
        {
            containerDefinition.Bind<MultithreadedNavigationService>().AsSingleton();
            containerDefinition.Bind<RunMethodsOnMainThread>().AsSingleton();
        }
    }
}
