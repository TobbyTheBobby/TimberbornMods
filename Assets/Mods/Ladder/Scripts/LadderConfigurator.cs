using Bindito.Core;
using Timberborn.TemplateSystem;

namespace Ladder
{
    [Context("Game")]
    public class LadderConfigurator : IConfigurator
    {
        public void Configure(IContainerDefinition containerDefinition)
        {
            containerDefinition.Bind<LadderService>().AsSingleton();
            containerDefinition.MultiBind<TemplateModule>().ToProvider(ProvideTemplateModule).AsSingleton();
        }

        private static TemplateModule ProvideTemplateModule()
        {
            var builder = new TemplateModule.Builder();
            builder.AddDecorator<Ladder, MultiDrivewayModelFixer>();
            return builder.Build();
        }
    }
}