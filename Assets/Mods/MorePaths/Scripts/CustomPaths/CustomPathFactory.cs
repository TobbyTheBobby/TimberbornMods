using System.Collections.Generic;
using System.Linq;
using MorePaths.Core;
using MorePaths.CustomPaths;
using MorePaths.Patches;
using Timberborn.AssetSystem;
using Timberborn.GameFactionSystem;
using TobbyTools.ImageRepository;
using UnityEngine;

namespace MorePaths
{
    public class CustomPathFactory
    {
        private static ImageRepositoryService _imageRepositoryService;
        private static IResourceAssetLoader _resourceAssetLoader;
        private static FactionService _factionService;
        
        private readonly MorePathsCore _morePathsCore;

        CustomPathFactory(ImageRepositoryService imageRepositoryService, IResourceAssetLoader resourceAssetLoader, FactionService factionService, MorePathsCore morePathsCore)
        {
            _imageRepositoryService = imageRepositoryService;
            _resourceAssetLoader = resourceAssetLoader;
            _factionService = factionService;
            _morePathsCore = morePathsCore;
        }
        
        public static GameObject PathCorner => _resourceAssetLoader.Load<GameObject>("tobbert.morepaths/tobbert_morepaths/PathCorner");

        public static Material ActivePathMaterial => _resourceAssetLoader.Load<Material>(_factionService.Current.PathMaterial);
        
        public List<CustomPath> CreatePathsFromSpecification()
        {
            // var stopwatch = Stopwatch.StartNew();

            PreventInstantiatePatch.RunInstantiate = false;

            var originalPathGameObject = Resources.Load<GameObject>("Buildings/Paths/Path/Path." + _factionService.Current.Id);
            originalPathGameObject.AddComponent<DynamicPathCorner>();
            originalPathGameObject.AddComponent<CustomPath>();
            
            var customPaths  = _morePathsCore.PathsSpecifications.Select(specification =>
            {
                var customPath = Object.Instantiate(originalPathGameObject, new GameObject().transform).GetComponent<CustomPath>();
                customPath.SetSpecification(_imageRepositoryService, specification);
                return customPath;
            }).ToList();
            PreventInstantiatePatch.RunInstantiate = true;

            // stopwatch.Stop();
            // Plugin.Log.LogInfo($"Created {customPaths.Count} paths in {stopwatch.ElapsedMilliseconds}ms.");
            return customPaths;
        }
    }
}
