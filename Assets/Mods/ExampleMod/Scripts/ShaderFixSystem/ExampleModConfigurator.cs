using Bindito.Core;
using Timberborn.Bots;
using Timberborn.TemplateSystem;

namespace ExampleMod.ShaderFixSystem
{
    [Context("Game")]
    public class ExampleModConfigurator : IConfigurator
    {
        public void Configure(IContainerDefinition containerDefinition)
        {
            containerDefinition.Bind<ShaderFix>().AsSingleton();
            containerDefinition.MultiBind<TemplateModule>().ToProvider(ProvideTemplateModule).AsSingleton();
        }

        private static TemplateModule ProvideTemplateModule()
        {
            var builder = new TemplateModule.Builder();
            builder.AddDecorator<Bot, BotTextureChanger>();
            return builder.Build();
        }
    }
}
