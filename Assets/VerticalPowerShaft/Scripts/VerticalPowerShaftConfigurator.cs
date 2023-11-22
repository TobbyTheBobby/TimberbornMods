using Bindito.Core;

namespace VerticalPowerShaft
{
    public class VerticalPowerShaftConfigurator : IConfigurator
    {
        public void Configure(IContainerDefinition containerDefinition)
        {
            // containerDefinition.Bind<VerticalPowerShaftService>().AsSingleton();
            // containerDefinition.MultiBind<TemplateModule>().ToProvider(ProvideTemplateModule).AsSingleton();
        }

        // private TemplateModule ProvideTemplateModule()
        // {
        //     TemplateModule.Builder builder = new TemplateModule.Builder();
        //     builder.AddDecorator<Beaver, ProfessionClothingComponent>();
        //     return builder.Build();
        // }
    }
}