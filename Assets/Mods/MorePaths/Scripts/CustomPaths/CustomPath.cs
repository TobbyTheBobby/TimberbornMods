using System.Collections.Generic;
using System.Linq;
using Bindito.Core;
using MorePaths.PathSpecificationSystem;
using Timberborn.AssetSystem;
using Timberborn.BaseComponentSystem;
using Timberborn.BlockSystem;
using Timberborn.Buildings;
using Timberborn.Common;
using Timberborn.EntitySystem;
using Timberborn.PrefabSystem;
using UnityEngine;

namespace MorePaths.CustomPaths
{
    public class CustomPath : BaseComponent
    {
        private IAssetLoader _assetLoader;

        private BuildingModel _buildingModel;
        private GameObject _pathCorner;
        
        private Texture2D _groundTexture2D;
        private Texture2D _railingTexture2D;
        private Material _groundMaterial;
        
        private bool IsDefaultPath => GetComponentFast<Prefab>().PrefabName is "Path.Folktails" or "Path.IronTeeth";
        
        public PathSpecification PathSpecification { get; private set; }
        
        [Inject]
        public void InjectDependencies(IAssetLoader assetLoader, PathSpecificationRepository pathSpecificationRepository)
        {
            _assetLoader = assetLoader;
            
            var pathSpecification = pathSpecificationRepository.GetAll().First(spec => GetComponentFast<Prefab>().PrefabName.Equals(spec.Name) || (spec.Name == "DefaultPath" && IsDefaultPath));
            
            SetSpecification(pathSpecification);
        }
        
        private void Start()
        {
            _buildingModel = GameObjectFast.GetComponent<BuildingModel>();
            OverwriteTextures();
        }

        public void SetSpecification(PathSpecification pathSpecification)
        {
            PathSpecification = pathSpecification;
            
            _groundTexture2D = LoadImage(PathSpecification.PathTexture);
            _railingTexture2D = LoadImage(PathSpecification.RailingTexture);
            OverwriteVariables();
        }

        private Texture2D LoadImage(string search)
        {
            return string.IsNullOrEmpty(search) ? new Texture2D(0, 0) : _assetLoader.Load<Texture2D>(search);
        }
        
        private void OverwriteVariables()
        {
            SetGroundMaterial();
            AddPathCorners();
            SetToolGroup(PathSpecification.ToolGroup);
            SetObjectName(PathSpecification.Name);
            SetPrefabName(PathSpecification.Name);
            RemoveBackwardCompatiblePrefabNames();
            SetToolOrder(PathSpecification.ToolOrder);
            if (PathSpecification.Name == "DefaultPath")
                return;
            SetLabeledEntitySpec();
        }
        
        private void OverwriteTextures()
        {
            if (PathSpecification.Name == "DefaultPath")
                return;
        
            var directChildren = _buildingModel.FinishedModel.GetDirectChildren().ToArray();
            
            foreach (var variantString in new List<string> { "Path0000", "Path0010", "Path1010", "Path0011", "Path0111", "Path1111" })
            {
                var originalGroundVariant = directChildren.First(obj => obj.name.Contains("Dirt" + variantString));
                
                originalGroundVariant.GetComponentInChildren<MeshRenderer>().material = _groundMaterial;
        
                if (PathSpecification.RailingTexture != null)
                {
                    var originalRoofVariant = directChildren.First(obj => obj.name.Contains("Roof" + variantString));
                    originalRoofVariant.GetComponentInChildren<MeshRenderer>().material.mainTexture = _railingTexture2D;
                }
            }
        }
        
        private void SetGroundMaterial()
        {
            var newGroundMaterial = new Material(CustomPathFactory.ActivePathMaterial);
            newGroundMaterial.SetFloat("_MainTexScale", PathSpecification.MainTextureScale);
            newGroundMaterial.SetFloat("_NoiseTexScale", PathSpecification.NoiseTexScale);
            newGroundMaterial.SetVector("_MainColor", new Vector4(PathSpecification.MainColorRed, PathSpecification.MainColorGreen, PathSpecification.MainColorBlue, 1f));
            if (PathSpecification.PathTexture != null)
                newGroundMaterial.mainTexture = _groundTexture2D;
            _groundMaterial = newGroundMaterial;
        }
        
        private void SetObjectName(string specificationName)
        {
            GameObjectFast.name = specificationName;
        }
        
        private void SetPrefabName(string specificationName)
        {
            GetComponentFast<Prefab>()._prefabName = specificationName;
        }
        
        private void RemoveBackwardCompatiblePrefabNames()
        {
            GetComponentFast<Prefab>()._backwardCompatiblePrefabNames = new string[] { };
        }
        
        private void SetToolGroup(string toolGroup)
        {
            GetComponentFast<PlaceableBlockObject>()._toolGroupId = toolGroup;
        }
        
        private void SetToolOrder(int toolOrder)
        {
            GetComponentFast<PlaceableBlockObject>()._toolOrder = toolOrder;
        }
        
        private void SetLabeledEntitySpec()
        {
            var labeledEntity = GameObjectFast.GetComponent<LabeledEntitySpec>();
            labeledEntity._displayNameLocKey = PathSpecification.DisplayNameLocKey;
            labeledEntity._descriptionLocKey = PathSpecification.DescriptionLocKey;
            labeledEntity._flavorDescriptionLocKey = PathSpecification.FlavorDescriptionLocKey;
            if (PathSpecification.PathIcon == null) return;
            labeledEntity._imagePath = PathSpecification.PathIcon;
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
        
            if (!GameObjectFast.TryGetComponent(out DynamicPathCorner dynamicPathCorner)) 
                return;
            dynamicPathCorner.CreatePathCorners(pathCorner);
        }
    }
}
