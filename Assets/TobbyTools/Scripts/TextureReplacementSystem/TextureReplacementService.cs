using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using TimberApi.Common.SingletonSystem;
using Timberborn.BaseComponentSystem;
using Timberborn.Common;
using Timberborn.Persistence;
using Timberborn.PrefabSystem;
using Timberborn.SingletonSystem;
using UnityEngine;

namespace TobbyTools.TextureReplacementTool
{
    public class TextureReplacementService : ITimberApiLoadableSingleton, ILoadableSingleton
    {
        private static readonly int BaseMap = Shader.PropertyToID("_BaseMap");
        private static readonly int MainTex = Shader.PropertyToID("_MainTex");
        
        private readonly ReplaceTextureSpecificationDeserializer _replaceTextureSpecificationDeserializer;
        private readonly ObjectCollectionService _objectCollectionService;
        private readonly ISpecificationService _specificationService;
        private readonly ImageRepository _imageRepository;

        private ImmutableArray<ReplaceTextureSpecification> _replaceTextureSpecifications;

        private readonly Dictionary<string, Texture2D> _cachedTextures = new();

        TextureReplacementService(
            ReplaceTextureSpecificationDeserializer replaceTextureSpecificationDeserializer,
            ObjectCollectionService objectCollectionService,
            ISpecificationService specificationService,
            ImageRepository imageRepository)
        {
            _replaceTextureSpecificationDeserializer = replaceTextureSpecificationDeserializer;
            _objectCollectionService = objectCollectionService;
            _specificationService = specificationService;
            _imageRepository = imageRepository;
        }

        void ITimberApiLoadableSingleton.Load()
        {
            _replaceTextureSpecifications = _specificationService
                .GetSpecifications(_replaceTextureSpecificationDeserializer)
                .OrderBy(specification => specification.BuildingName == null)
                .ThenBy(specification => specification.BuildingName).ToImmutableArray();
        
            foreach (var specification in _replaceTextureSpecifications)
            {
                Plugin.Log.LogError(specification.MaterialName + "   " + specification.BuildingName);
            }
        }

        void ILoadableSingleton.Load()
        {
            foreach (var specification in _replaceTextureSpecifications)
            {
                if (specification.BuildingName != null)
                {
                    ReplaceSpecificTexture(specification);
                }
                else
                {
                    ReplaceSharedTexture(specification);
                }
            }
        }

        private void ReplaceSharedTexture(ReplaceTextureSpecification specification)
        {
            var meshRenderers = _objectCollectionService.GetAllMonoBehaviours<BaseComponent>().SelectMany(gameObject => gameObject.GetComponentsInChildren<MeshRenderer>());

            OverwriteTextures(meshRenderers, specification);
        }

        private void ReplaceSpecificTexture(ReplaceTextureSpecification specification)
        {
            var prefabs = _objectCollectionService.GetAllMonoBehaviours<Prefab>();
            
            foreach (var prefab in prefabs)
            {
                if (prefab.PrefabName != specification.BuildingName) 
                    continue;
                
                var meshRenderers = prefab.GetComponentsInChildren<MeshRenderer>();
                OverwriteTextures(meshRenderers, specification);
            }
        }

        private void OverwriteTextures(IEnumerable<MeshRenderer> meshRenderers, ReplaceTextureSpecification specification)
        {
            foreach (var meshRenderer in meshRenderers)
            {
                foreach (var material in meshRenderer.materials)
                {
                    // Plugin.Log.LogError("meshRenderer: " + meshRenderer.name + " | Material: " + material.name);

                    if (material.name.Contains(specification.MaterialName))
                    {
                        // Plugin.Log.LogWarning("Material: " + materialName + " | Replacing with: " + specification.ReplacementTextureName);

                        ReplaceTexture(material, specification.ReplacementTextureName);
                    }
                }
            }
        }

        private void ReplaceTexture(Material material, string textureName)
        {
            var texture2D = _cachedTextures.GetOrAdd(textureName, () => LoadTexture(textureName));
            
            if (material.HasTexture(BaseMap))
                material.SetTexture(BaseMap, texture2D);
            
            if (material.HasTexture(MainTex))
                material.SetTexture(MainTex, texture2D);
        }
        
        private Texture2D LoadTexture(string replacementTextureName)
        {
            Texture2D texture2D = new Texture2D(1, 1);
            byte[] spriteBytes = { };
            if (replacementTextureName.Contains(".png") || replacementTextureName.Contains(".jpg"))
            {
                spriteBytes = File.ReadAllBytes(_imageRepository.Images[replacementTextureName]);
            }
            else if (!replacementTextureName.Contains(".png"))
            {
                var png = replacementTextureName + ".png";
                if (File.Exists(png))
                    spriteBytes = File.ReadAllBytes(_imageRepository.Images[png]);
            } 
            else if (!replacementTextureName.Contains(".jpg"))
            {
                var png = replacementTextureName + ".jpg";
                if (File.Exists(png))
                    spriteBytes = File.ReadAllBytes(_imageRepository.Images[png]);
            }

            if (spriteBytes.IsEmpty())
                throw new Exception("File '"+ replacementTextureName +"' does not exist, or has unsupported file extension.");

            texture2D.LoadImage(spriteBytes);
            return texture2D;
        }
    }
}
