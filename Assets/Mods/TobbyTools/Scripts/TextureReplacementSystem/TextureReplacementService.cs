// using System.Collections.Generic;
// using System.Collections.Immutable;
// using System.Linq;
// using TimberApi.SingletonSystem;
// using Timberborn.Common;
// using Timberborn.Persistence;
// using Timberborn.PrefabSystem;
// using Timberborn.SingletonSystem;
// using Timberborn.Timbermesh;
// using TobbyTools.InaccessibilityUtilitySystem;
// using UnityEngine;
//
// namespace TobbyTools.TextureReplacementSystem
// {
//     public class TextureReplacementService : ITimberApiLoadableSingleton, ILoadableSingleton
//     {
//         private static readonly int BaseMap = Shader.PropertyToID("_BaseMap");
//         private static readonly int MainTex = Shader.PropertyToID("_MainTex");
//         
//         private readonly ReplaceTextureSpecificationDeserializer _replaceTextureSpecificationDeserializer;
//         private readonly ObjectCollectionService _objectCollectionService;
//         private readonly ISpecificationService _specificationService;
//         private readonly ImageRepositoryService _imageRepositoryService;
//         private readonly IMaterialRepository _materialRepository;
//
//         private ImmutableArray<ReplaceTextureSpecification> _replaceTextureSpecifications;
//
//         private readonly Dictionary<string, Texture2D> _cachedTextures = new();
//
//         TextureReplacementService(
//             ReplaceTextureSpecificationDeserializer replaceTextureSpecificationDeserializer,
//             ObjectCollectionService objectCollectionService,
//             ISpecificationService specificationService,
//             ImageRepositoryService imageRepositoryService,
//             IMaterialRepository materialRepository)
//         {
//             _replaceTextureSpecificationDeserializer = replaceTextureSpecificationDeserializer;
//             _objectCollectionService = objectCollectionService;
//             _specificationService = specificationService;
//             _imageRepositoryService = imageRepositoryService;
//             _materialRepository = materialRepository;
//         }
//
//         void ITimberApiLoadableSingleton.Load()
//         {
//             _replaceTextureSpecifications = _specificationService
//                 .GetSpecifications(_replaceTextureSpecificationDeserializer)
//                 .OrderBy(specification => specification.BuildingName == null)
//                 .ThenBy(specification => specification.BuildingName).ToImmutableArray();
//         
//             // foreach (var specification in _replaceTextureSpecifications)
//             // {
//             //     Plugin.Log.LogError(specification.MaterialName + "   " + specification.BuildingName);
//             // }
//             //
//             // Plugin.Log.LogWarning("       ");
//             //
//             // var test = (MaterialRepository)_materialRepository;
//             //
//             // foreach (var material in (List<Material>)InaccessibilityUtilities.GetInaccessibleField(test, "_materials"))
//             // {
//             //     Plugin.Log.LogError(material.name);
//             // }
//         }
//
//         void ILoadableSingleton.Load()
//         {
//             foreach (var specification in _replaceTextureSpecifications)
//             {
//                 if (specification.BuildingName != null)
//                 {
//                     ReplaceSpecificTexture(specification);
//                 }
//                 else
//                 {
//                     ReplaceSharedTexture(specification);
//                 }
//             }
//         }
//
//         private void ReplaceSharedTexture(ReplaceTextureSpecification specification)
//         {
//             foreach (var material in (List<Material>)InaccessibilityUtilities.GetInaccessibleField(_materialRepository, "_materials"))
//             {
//                 if (material.name.Contains(specification.MaterialName))
//                 {
//                     // Plugin.Log.LogWarning("Material: " + material.name + " | Replacing with: " + specification.ReplacementTextureName);
//
//                     ReplaceTexture(material, specification.ReplacementTextureName);
//                 }
//             }
//         }
//
//         private void ReplaceSpecificTexture(ReplaceTextureSpecification specification)
//         {
//             var prefabs = _objectCollectionService.GetAllMonoBehaviours<Prefab>();
//             
//             foreach (var prefab in prefabs)
//             {
//                 if (prefab.PrefabName != specification.BuildingName) 
//                     continue;
//                 
//                 var meshRenderers = prefab.GetComponentsInChildren<MeshRenderer>(true);
//                 OverwriteTextures(meshRenderers, specification);
//             }
//         }
//
//         private void OverwriteTextures(IEnumerable<MeshRenderer> meshRenderers, ReplaceTextureSpecification specification)
//         {
//             foreach (var meshRenderer in meshRenderers)
//             {
//                 foreach (var material in meshRenderer.materials)
//                 {
//                     // Plugin.Log.LogError("meshRenderer: " + meshRenderer.name + " | Material: " + material.name);
//
//                     if (material.name.Contains(specification.MaterialName))
//                     {
//                         // Plugin.Log.LogWarning("Material: " + material.name + " | Replacing with: " + specification.ReplacementTextureName);
//
//                         ReplaceTexture(material, specification.ReplacementTextureName);
//                     }
//                 }
//             }
//         }
//
//         private void ReplaceTexture(Material material, string textureName)
//         {
//             var texture2D = _cachedTextures.GetOrAdd(textureName, () => _imageRepositoryService.GetByName(textureName));
//             
//             if (material.HasTexture(BaseMap))
//                 material.SetTexture(BaseMap, texture2D);
//             
//             if (material.HasTexture(MainTex))
//                 material.SetTexture(MainTex, texture2D);
//         }
//     }
// }
