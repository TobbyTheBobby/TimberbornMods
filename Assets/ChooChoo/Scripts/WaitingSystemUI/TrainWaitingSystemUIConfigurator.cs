// using Bindito.Core;
// using ChooChoo.Trains;
// using TimberApi.SceneSystem;
// using Timberborn.TemplateSystem;
// using TobbyTools.UsedImplicitlySystem;
//
// namespace ChooChoo.WaitingSystemUI
// {
//     [Configurator(SceneEntrypoint.InGame)]
//     public class TrainWaitingSystemUIConfigurator : IConfigurator
//     {
//         public void Configure(IContainerDefinition containerDefinition)
//         {
//             containerDefinition.MultiBind<TemplateModule>().ToProvider(ProvideTemplateModule).AsSingleton();
//         }
//
//         private static TemplateModule ProvideTemplateModule()
//         {
//             var builder = new TemplateModule.Builder();
//             builder.AddDecorator<Train, NoWaitingStationStatus>();
//             return builder.Build();
//         }
//     }
// }