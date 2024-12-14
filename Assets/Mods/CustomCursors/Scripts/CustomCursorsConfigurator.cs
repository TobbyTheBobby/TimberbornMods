using Bindito.Core;

namespace CustomCursors
{
    [Context("MainMenu")]
    [Context("Game")]
    [Context("MapEditor")]
    public class CustomCursorsConfigurator : IConfigurator
    {
        public void Configure(IContainerDefinition containerDefinition)
        {
            containerDefinition.Bind<BaseGameCursorAdder>().AsSingleton();
            containerDefinition.Bind<CustomCursorsSettings>().AsSingleton();
            containerDefinition.Bind<CustomCursorsService>().AsSingleton();
        }
    }
}