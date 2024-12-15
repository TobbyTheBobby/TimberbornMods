// using System.Collections.Generic;
// using System.Linq;
// using Timberborn.AssetSystem;
// using Timberborn.ModdingAssets;
// using Timberborn.SingletonSystem;
// using UnityEngine;
//
// namespace TobbyTools.ShaderFixSystem
// {
//     public class ShaderFix : ILoadableSingleton
//     {
//         private readonly ModAssetBundleProvider _modAssetBundleProvider;
//         private readonly ShaderRepository _shaderRepository;
//         
//         public ShaderFix(IEnumerable<IAssetProvider> assetProviders, ShaderRepository shaderRepository)
//         {
//             _modAssetBundleProvider = (ModAssetBundleProvider)assetProviders.First(provider => provider is ModAssetBundleProvider);
//             _shaderRepository = shaderRepository;
//         }
//
//         public void Load()
//         {
//             var materials =  _modAssetBundleProvider.LoadAll<Material>("").Select(asset => asset.Asset);
//
//             foreach (var material in materials)
//             {
//                 var shader = _shaderRepository.Shaders.First(shader => shader.Value.renderQueue == material.shader.renderQueue).Value;
//
//                 material.shader = shader;
//             }
//         }
//     }
// }