using Bindito.Core;

namespace TobbyTools.ShaderFixSystem
{
    [Context("Game")]
    public class ShaderFixConfigurator : IConfigurator
    {
        public void Configure(IContainerDefinition containerDefinition)
        {
            containerDefinition.Bind<ShaderRepository>().AsSingleton();
            containerDefinition.Bind<ShaderFix>().AsSingleton();
        }
    }
}
