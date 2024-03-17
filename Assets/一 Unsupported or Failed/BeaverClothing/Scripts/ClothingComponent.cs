using System;
using System.Collections.Generic;
using System.Linq;
using Bindito.Core;
using Timberborn.BaseComponentSystem;
using Timberborn.Beavers;
using Timberborn.Bots;
using Timberborn.WorkSystem;
using UnityEngine;

namespace BeaverClothing
{
    public class ClothingComponent : BaseComponent
    {
        private const bool ShouldLog = true;
        
        private BeaverClothingService _beaverClothingService;

        private IEnumerable<ClothingSpecification> _clothingSpecifications;

        private readonly Dictionary<ClothingSpecification, GameObject> _clothingObjects = new();

        [Inject]
        public void InjectDependencies(BeaverClothingService beaverClothingService)
        {
            _beaverClothingService = beaverClothingService;
        }

        private void Awake()
        {
            _beaverClothingService.InitiateClothing(GameObjectFast);
        }

        private void Start()
        {
            foreach (var specification in _beaverClothingService.ClothingSpecifications)
            {
                if (ShouldLog) Plugin.Log.LogInfo(specification.CharacterType);
                if (ShouldLog) Plugin.Log.LogInfo(specification.PrefabPath);
            }
            // return;
            if (TryGetComponentFast(out Child _))
            {
                _clothingSpecifications = _beaverClothingService.ClothingSpecifications.Where(specification => specification.CharacterType == "BeaverChild");
            }

            if (TryGetComponentFast(out Worker worker))
            {
                worker.GotUnemployed += OnGotUnemployed;
                worker.GotEmployed += OnGotEmployed;
                
                if (TryGetComponentFast(out Beaver _))
                {
                    _clothingSpecifications = _beaverClothingService.ClothingSpecifications.Where(specification => specification.CharacterType == "BeaverAdult");
                }
                if (TryGetComponentFast(out Bot _))
                {
                    _clothingSpecifications = _beaverClothingService.ClothingSpecifications.Where(specification => specification.CharacterType == "Bot");
                }
            }
            IndexClothing();
            UpdateClothing();
        }

        private void IndexClothing()
        {
            foreach (var clothingSpecification in _clothingSpecifications)
            {
                var test = clothingSpecification.PrefabPath.Split("/").Last();
                if (ShouldLog) Plugin.Log.LogError("Clothing name: " + test);
                var bodyPart = BeaverClothingService.FindBodyPart(TransformFast, "#" + test);
                if (ShouldLog) Plugin.Log.LogError("Body part: " + bodyPart);
                if (bodyPart != null)
                {
                    _clothingObjects.Add(clothingSpecification, bodyPart.gameObject);
                }
            }
        }
        
        private void UpdateClothing()
        {
            foreach (var clothingSpecification in _clothingSpecifications)
            {
                var clothingObject = _clothingObjects[clothingSpecification];
                
                if (TryGetComponentFast(out Worker worker) && worker.Employed)
                {
                    var workplace = worker.Workplace.name.Split(".")[0];

                    var workplaceClothingSpecification = _beaverClothingService.WorkplaceClothingSpecifications.FirstOrDefault(spec =>
                        spec.Workplace == workplace);

                    if (workplaceClothingSpecification != null && !WearSomethingAtAll(workplaceClothingSpecification)) 
                        continue;
                    
                    if (clothingSpecification.WorkPlaces.Count == 0)
                    {
                        var shouldWear = WearClothingPieceBased(clothingSpecification);
                        clothingObject.gameObject.SetActive(shouldWear);
                        if (shouldWear)
                            return;
                    }
                    
                    clothingObject.gameObject.SetActive(clothingSpecification.WorkPlaces.Contains(workplace));
                }
                else
                {
                    var shouldWear = WearClothingPieceBased(clothingSpecification);
                    clothingObject.gameObject.SetActive(shouldWear);
                    if (shouldWear)
                        return;
                }
            }
        }
        
        private bool WearSomethingAtAll(WorkplaceClothingSpecification workplaceClothingSpecification) => BeaverClothingService.Random.Next(100) < workplaceClothingSpecification.WearChance;
        
        private bool WearClothingPieceBased(ClothingSpecification clothingSpecification) => BeaverClothingService.Random.Next(100) < clothingSpecification.WearChance;

        private void OnGotUnemployed(object sender, EventArgs e) => UpdateClothing();

        private void OnGotEmployed(object sender, EventArgs e) => UpdateClothing();
    }
}
