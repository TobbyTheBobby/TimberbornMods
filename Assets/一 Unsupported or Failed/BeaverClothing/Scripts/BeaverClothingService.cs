using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Timberborn.AssetSystem;
using Timberborn.Beavers;
using Timberborn.Common;
using Timberborn.Persistence;
using Timberborn.SingletonSystem;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = System.Random;

namespace BeaverClothing
{
    public class BeaverClothingService : ILoadableSingleton 
    {
        private readonly WorkplaceClothingSpecificationDeserializer _workplaceClothingSpecificationDeserializer;
        private readonly ClothingSpecificationDeserializer _clothingSpecificationDeserializer;
        private readonly ISpecificationService _specificationService;
        private readonly IResourceAssetLoader _resourceAssetLoader;

        public static readonly Random Random = new();
        
        private Shader _shader;
        private ImmutableArray<ClothingSpecification> _clothingSpecifications;

        public ImmutableArray<ClothingSpecification> ClothingSpecifications => _clothingSpecifications;
        private ImmutableArray<WorkplaceClothingSpecification> _workplaceClothingSpecifications;

        public ImmutableArray<WorkplaceClothingSpecification> WorkplaceClothingSpecifications => _workplaceClothingSpecifications;
        
        public readonly List<Clothing> Clothings = new()
        {
            // new Clothing
            // { 
            //     Name="FrogHat", 
            //     BodyPartName = "DEF-head",
            //     WorkPlaces = new List<string>() { "" }
            // },
            new Clothing
            { 
                Name="ConstructionHat", 
                BodyPartName = "DEF-head",
                WorkPlaces = new List<string> { "" }
            },
            // new Clothing() { 
            //     Name="ConstructionHat", 
            //     BodyPartName = "DEF-head",
            //     WorkPlaces = new List<string>() { "DistrictCenter" }
            // },
            // new Clothing() { 
            //     Name="StrawHat", 
            //     BodyPartName = "DEF-head",
            //     WorkPlaces = new List<string>() { "FarmHouse" }
            // },
            // new Clothing() { 
            //      Name="FlowerCrown", 
            //      BodyPartName = "DEF-head",
            //      WorkPlaces = new List<string>() { "GathererFlag" }
            // },
            // new Clothing() { 
            //     Name="PithHelmet", 
            //     BodyPartName = "DEF-head",
            //     WorkPlaces = new List<string>() { "ScavengerFlag" }
            // },
            // new Clothing() { 
            //     Name="ChefsHat", 
            //     BodyPartName = "DEF-head",
            //     WorkPlaces = new List<string>() { "Grill", "Bakery", "Gristmill" }
            // }
        };

        public BeaverClothingService(WorkplaceClothingSpecificationDeserializer workplaceClothingSpecificationDeserializer, ClothingSpecificationDeserializer clothingSpecificationDeserializer, ISpecificationService specificationService, IResourceAssetLoader resourceAssetLoader)
        {
            _workplaceClothingSpecificationDeserializer = workplaceClothingSpecificationDeserializer;
            _clothingSpecificationDeserializer = clothingSpecificationDeserializer;
            _specificationService = specificationService;
            _resourceAssetLoader = resourceAssetLoader;
        }

        private Shader Shader
        {
            get
            {
                if (_shader == null)
                    _shader = Resources.Load<Material>($"materials/goods/Box").shader;
                return _shader;
            }
        }
        
        public void Load()
        {
            _clothingSpecifications = _specificationService.GetSpecifications(_clothingSpecificationDeserializer).Where(specification => specification.Enabled).ToImmutableArray();
            _workplaceClothingSpecifications = _specificationService.GetSpecifications(_workplaceClothingSpecificationDeserializer).Where(specification => specification.Enabled).ToImmutableArray();
        }

        public void InitiateClothing(GameObject prefab)
        {
            foreach (var specification in _clothingSpecifications)
            {
                InitiateClothingPiece(specification, prefab.transform);
            }
        }

        private void InitiateClothingPiece(ClothingSpecification specification, Transform character)
        {
            LogTransform(character);

            var child = character.gameObject.GetComponent<Child>();

            Transform bodyPart;
            
            if (child)
            {
                bodyPart = character.gameObject.FindChild("#BeaverChild");
            }
            else
            {
                bodyPart = character.gameObject.FindChild("#BeaverAdult");
            }
            
             
            
            if (bodyPart == null)
                return;

            var clothingObject = Object.Instantiate(_resourceAssetLoader.Load<GameObject>(specification.PrefabPath));
            var transform = clothingObject.transform;
            transform.position = new Vector3(specification.PositionX, specification.PositionY, specification.PositionZ);
            transform.rotation = Quaternion.Euler(specification.RotationX, specification.RotationY, specification.RotationZ);
            // Plugin.Log.LogInfo("Adding: " + clothingObject.name);
            clothingObject.name = ("#" + clothingObject.name).Replace("(Clone)", "");
            ShaderFix(transform);
            // ReSharper disable once Unity.InefficientPropertyAccess
            transform.rotation *= bodyPart.rotation;
            // ReSharper disable once Unity.InefficientPropertyAccess
            transform.position += bodyPart.position;
            transform.localScale = new Vector3(specification.ScaleX, specification.ScaleY, specification.ScaleZ);
            transform.Rotate(5, 0, 0);
            
            
            if (bodyPart.Find(clothingObject.name) != null)
                bodyPart.Find(clothingObject.name).parent = null;
            
            transform.SetParent(bodyPart.transform);
            clothingObject.SetActive(false);
        }
        
        private void ShaderFix(Transform child)
        {
            foreach (var child1 in child)
            {
                ShaderFix(child1 as Transform);
            }
            
            if (!child.TryGetComponent(out MeshRenderer meshRenderer)) 
                return;
            foreach (var material in meshRenderer.materials)
            {
                material.shader = Shader;
            }
        }

        public static Transform FindBodyPart(Transform parent, string bodyPartName)
        {
            foreach (Transform child in parent)
            {
                if (child.name == bodyPartName)
                    return child;
                
                var result = FindBodyPart(child, bodyPartName);
                if (result != null)
                    return result;
            }

            return null;
        }

        private void LogTransform(Transform parent)
        {
            // Plugin.Log.LogInfo(parent.name);
            foreach (Transform child in parent)
            {
                LogTransform(child);
            }
        }
    }
}
