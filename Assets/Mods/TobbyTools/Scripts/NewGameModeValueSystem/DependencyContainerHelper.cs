// using Bindito.Core;
// using JetBrains.Annotations;
//
// namespace TobbyTools.NewGameModeValueSystem
// {
//     public static class DependencyContainerHelper
//     {
//         public static void BindNewGameValue<[MeansImplicitUse(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)] T2>(this IContainerDefinition containerDefinition) where T2 : class, INewGameModeValue
//         {
//             containerDefinition.Bind<T2>().AsSingleton();
//             containerDefinition.MultiBind<INewGameModeValue>().To<T2>().AsSingleton();
//         }
//     }
// }