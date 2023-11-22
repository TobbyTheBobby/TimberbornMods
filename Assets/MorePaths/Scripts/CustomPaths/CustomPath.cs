using System;
using System.Collections.Generic;
using System.Linq;
using Bindito.Core;
using Timberborn.BaseComponentSystem;
using Timberborn.BlockSystem;
using Timberborn.Buildings;
using Timberborn.Common;
using Timberborn.PrefabSystem;
using UnityEngine;

namespace MorePaths
{
    public class CustomPath : BaseComponent
    {
        private MorePathsCore _morePathsCore;
        
        private PathSpecification _pathSpecification;
        private BuildingModel _buildingModel;
        private GameObject _pathCorner;

        private Texture2D _groundTexture2D;
        private Texture2D _railingTexture2D;
        private Texture2D _spriteTexture2D;

        private Material _groundMaterial;

        private bool IsDefaultPath => GetComponentFast<Prefab>().PrefabName is "Path.Folktails" or "Path.IronTeeth";

        [Inject]
        public void InjectDependencies(MorePathsCore morePathsCore)
        {
            var pathSpecification = morePathsCore.PathsSpecifications.First(spec => GetComponentFast<Prefab>().PrefabName.Equals(spec.Name) || (spec.Name == "DefaultPath" && IsDefaultPath));
            
            SetSpecification(morePathsCore, pathSpecification);
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

        public void SetSpecification(MorePathsCore morePathsCore, PathSpecification pathSpecification)
        {
            _morePathsCore = morePathsCore;
            
            _pathSpecification = pathSpecification;
            _pathCorner = Instantiate(CustomPathFactory.PathCorner);
            _pathCorner.SetActive(false);

            _groundTexture2D = _pathSpecification.GroundTexture2D(_morePathsCore);
            _railingTexture2D = _pathSpecification.RailingTexture2D(_morePathsCore);
            _spriteTexture2D = _pathSpecification.SpriteTexture2D(_morePathsCore, 112, 112);
            OverwriteVariables();
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
            _morePathsCore.ChangePrivateField(GameObjectFast.GetComponent<Prefab>(), "_prefabName", specificationName);
        }

        private void RemoveBackwardCompatiblePrefabNames()
        {
            _morePathsCore.ChangePrivateField(GameObjectFast.GetComponent<Prefab>(), "_backwardCompatiblePrefabNames", new string[] { });
        }

        private void SetToolGroup(string toolGroup)
        {
            _morePathsCore.ChangePrivateField(GameObjectFast.GetComponent<PlaceableBlockObject>(), "_toolGroupId", toolGroup);
        }
        
        private void SetToolOrder(int toolOrder)
        {
            _morePathsCore.ChangePrivateField(GameObjectFast.GetComponent<PlaceableBlockObject>(), "_toolOrder", toolOrder);
        }

        private void SetLocalisationAndSprite()
        {
            var labeledPrefab = GameObjectFast.GetComponent<LabeledPrefab>();
            _morePathsCore.ChangePrivateField(labeledPrefab, "_displayNameLocKey", _pathSpecification.DisplayNameLocKey);
            _morePathsCore.ChangePrivateField(labeledPrefab, "_descriptionLocKey", _pathSpecification.DescriptionLocKey);
            _morePathsCore.ChangePrivateField(labeledPrefab, "_flavorDescriptionLocKey", _pathSpecification.FlavorDescriptionLocKey);
            
            if (_pathSpecification.PathIcon == null) return;

            var sprite2D = Sprite.Create(_spriteTexture2D, labeledPrefab.Image.rect, labeledPrefab.Image.pivot, labeledPrefab.Image.pixelsPerUnit);
            _morePathsCore.ChangePrivateField(labeledPrefab, "_image", sprite2D);
        }

        private void AddPathCorners()
        {
            var material = new Material(_groundMaterial);
            material.SetTexture("_FadeTex", null);
            material.SetTexture("_NoiseTex", null);
            material.SetTexture("_DetailMask", null);
            material.renderQueue -= 1;

            _pathCorner.GetComponentInChildren<MeshRenderer>().material = material;

            if (!GameObjectFast.TryGetComponent(out DynamicPathCorner dynamicPathCorner)) return;
            
            dynamicPathCorner.CreatePathCorners(_pathCorner);
        }
    }
}
