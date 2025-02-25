using Bindito.Core;
using Timberborn.TemplateSystem;

namespace Ladder
{
    [Context("Game")]
    public class LadderConfigurator : Configurator
    {
        protected override void Configure()
        {
            Bind<LadderService>().AsSingleton();
            MultiBind<TemplateModule>().ToProvider(ProvideTemplateModule).AsSingleton();
        }

        private static TemplateModule ProvideTemplateModule()
        {
            var builder = new TemplateModule.Builder();
            builder.AddDecorator<Ladder, SecondDrivewayModel>();
            builder.AddDecorator<Ladder, ThirdDrivewayModel>();
            return builder.Build();
        }
    }
}