using System;
using System.Collections.Generic;
using System.Linq;
using Bindito.Core;
using MorePaths.Core;
using MorePaths.Specifications;
using Timberborn.BaseComponentSystem;
using Timberborn.BlockSystem;
using Timberborn.Buildings;
using Timberborn.Common;
using Timberborn.PrefabSystem;
using TobbyTools.ImageRepository;
using TobbyTools.InaccessibilityUtilitySystem;
using UnityEngine;

namespace MorePaths.CustomPaths
{
    public class CustomPath : BaseComponent
    {
        private ImageRepositoryService _imageRepositoryService;
        
        private PathSpecification _pathSpecification;
        private BuildingModel _buildingModel;
        private GameObject _pathCorner;

        private Texture2D _groundTexture2D;
        private Texture2D _railingTexture2D;
        private Texture2D _spriteTexture2D;

        private Material _groundMaterial;

        private bool IsDefaultPath => GetComponentFast<Prefab>().PrefabName is "Path.Folktails" or "Path.IronTeeth";

        [Inject]
        public void InjectDependencies(ImageRepositoryService imageRepositoryService, MorePathsCore morePathsCore)
        {
            var pathSpecification = morePathsCore.PathsSpecifications.First(spec => GetComponentFast<Prefab>().PrefabName.Equals(spec.Name) || (spec.Name == "DefaultPath" && IsDefaultPath));
            
            SetSpecification(imageRepositoryService, pathSpecification);
        }

        private void Start()
        {
            _buildingModel = GameObjectFast.GetComponent<BuildingModel>();
            try
            {
                OverwriteTextures();
            }
            catch (InvalidOperationException)
            {
                // Ignored
            }
        }

        public void SetSpecification(ImageRepositoryService imageRepositoryService, PathSpecification pathSpecification)
        {
            _imageRepositoryService = imageRepositoryService;
            
            _pathSpecification = pathSpecification;

            _groundTexture2D = LoadImage(_pathSpecification.PathTexture);
            _railingTexture2D = LoadImage(_pathSpecification.RailingTexture);
            _spriteTexture2D = LoadImage(_pathSpecification.PathIcon);
            OverwriteVariables();
        }

        private Texture2D LoadImage(string search)
        {
            if (string.IsNullOrEmpty(search) || string.IsNullOrEmpty(_pathSpecification.Name))
            {
                return new Texture2D(0, 0);
            }

            return _imageRepositoryService.GetByName(search, _pathSpecification.Name);
        }

        private void OverwriteVariables()
        {
            SetGroundMaterial();
            AddPathCorners();
            SetToolGroup(_pathSpecification.ToolGroup);
            if (_pathSpecification.Name == "DefaultPath")
                return;
            SetObjectName(_pathSpecification.Name);
            SetPrefabName(_pathSpecification.Name);
            RemoveBackwardCompatiblePrefabNames();
            SetToolOrder(_pathSpecification.ToolOrder);
            SetLocalisationAndSprite();
        }

        private void OverwriteTextures()
        {
            if (_pathSpecification.Name == "DefaultPath")
                return;

            var directChildren = _buildingModel.FinishedModel.GetDirectChildren().ToArray();
            
            foreach (var variantString in new List<string> { "Path0000", "Path0010", "Path1010", "Path0011", "Path0111", "Path1111" })
            {
                var originalGroundVariant = directChildren.First(obj => obj.name.Contains("Dirt" + variantString));
                
                originalGroundVariant.GetComponentInChildren<MeshRenderer>().material = _groundMaterial;

                if (_pathSpecification.RailingTexture != null)
                {
                    var originalRoofVariant = directChildren.First(obj => obj.name.Contains("Roof" + variantString));
                    originalRoofVariant.GetComponentInChildren<MeshRenderer>().material.mainTexture = _railingTexture2D;
                }
            
                // if (!_pathSpecification.RailingEnabled)
                // {
                //     _morePathsCore.ChangePrivateField(pathModelVariant, "_roofVariant", new GameObject());
                // }
            }
        }

        private void SetGroundMaterial()
        {
            var newGroundMaterial = new Material(CustomPathFactory.ActivePathMaterial);
            newGroundMaterial.SetFloat("_MainTexScale", _pathSpecification.MainTextureScale);
            newGroundMaterial.SetFloat("_NoiseTexScale", _pathSpecification.NoiseTexScale);
            newGroundMaterial.SetVector("_MainColor", new Vector4(_pathSpecification.MainColorRed, _pathSpecification.MainColorGreen, _pathSpecification.MainColorBlue, 1f));
            if (_pathSpecification.PathTexture != null)
                newGroundMaterial.mainTexture = _groundTexture2D;
            _groundMaterial = newGroundMaterial;
        }
        
        private void SetObjectName(string specificationName)
        {
            GameObjectFast.name = specificationName;
        }

        private void SetPrefabName(string specificationName)
        {
            InaccessibilityUtilities.SetInaccessibleField(GameObjectFast.GetComponent<Prefab>(), "_prefabName", specificationName);
        }

        private void RemoveBackwardCompatiblePrefabNames()
        {
            InaccessibilityUtilities.SetInaccessibleField(GameObjectFast.GetComponent<Prefab>(), "_backwardCompatiblePrefabNames", new string[] { });
        }

        private void SetToolGroup(string toolGroup)
        {
            InaccessibilityUtilities.SetInaccessibleField(GameObjectFast.GetComponent<PlaceableBlockObject>(), "_toolGroupId", toolGroup);
        }
        
        private void SetToolOrder(int toolOrder)
        {
            InaccessibilityUtilities.SetInaccessibleField(GameObjectFast.GetComponent<PlaceableBlockObject>(), "_toolOrder", toolOrder);
        }

        private void SetLocalisationAndSprite()
        {
            var labeledPrefab = GameObjectFast.GetComponent<LabeledPrefab>();
            
            InaccessibilityUtilities.SetInaccessibleField(labeledPrefab, "_displayNameLocKey", _pathSpecification.DisplayNameLocKey);
            InaccessibilityUtilities.SetInaccessibleField(labeledPrefab, "_descriptionLocKey", _pathSpecification.DescriptionLocKey);
            InaccessibilityUtilities.SetInaccessibleField(labeledPrefab, "_flavorDescriptionLocKey", _pathSpecification.FlavorDescriptionLocKey);
            
            if (_pathSpecification.PathIcon == null) return;

            var sprite2D = Sprite.Create(_spriteTexture2D, labeledPrefab.Image.rect, labeledPrefab.Image.pivot, labeledPrefab.Image.pixelsPerUnit);
            InaccessibilityUtilities.SetInaccessibleField(labeledPrefab, "_image", sprite2D);
        }

        private void AddPathCorners()
        {
            var material = new Material(_groundMaterial);
            material.SetTexture("_FadeTex", null);
            material.SetTexture("_NoiseTex", null);
            material.SetTexture("_DetailMask", null);
            material.renderQueue -= 1;

            var pathCorner = Instantiate(CustomPathFactory.PathCorner);
            pathCorner.SetActive(false);
            
            pathCorner.GetComponentInChildren<MeshRenderer>().material = material;

            if (!GameObjectFast.TryGetComponent(out DynamicPathCorner dynamicPathCorner)) return;
            
            dynamicPathCorner.CreatePathCorners(pathCorner);
        }
    }
}
