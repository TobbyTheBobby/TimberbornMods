using System.Collections.Generic;
using System.Linq;
using Timberborn.AssetSystem;
using Timberborn.GameFactionSystem;
using UnityEngine;

namespace MorePaths
{
    public class CustomPathFactory
    {
        private static IResourceAssetLoader _resourceAssetLoader;
        private static FactionService _factionService;
        
        private readonly MorePathsCore _morePathsCore;

        CustomPathFactory(IResourceAssetLoader resourceAssetLoader, FactionService factionService, MorePathsCore morePathsCore)
        {
            _resourceAssetLoader = resourceAssetLoader;
            _factionService = factionService;
            _morePathsCore = morePathsCore;
        }
        
        public static GameObject PathCorner => _resourceAssetLoader.Load<GameObject>("tobbert.morepaths/tobbert_morepaths/PathCorner");

        public static Material ActivePathMaterial => _resourceAssetLoader.Load<Material>(_factionService.Current.PathMaterial);
        
        public List<CustomPath> CreatePathsFromSpecification()
        {
            // Stopwatch stopwatch = Stopwatch.StartNew();

            PreventInstantiatePatch.RunInstantiate = false;

            var originalPathGameObject = Resources.Load<GameObject>("Buildings/Paths/Path/Path." + _factionService.Current.Id);
            originalPathGameObject.AddComponent<DynamicPathCorner>();
            originalPathGameObject.AddComponent<CustomPath>();
            
            var test  = _morePathsCore.PathsSpecifications.Select(specification =>
            {
                var customPath = Object.Instantiate(originalPathGameObject, new GameObject().transform).GetComponent<CustomPath>();
                customPath.SetSpecification(_morePathsCore, specification);
                return customPath;
            }).ToList();
            PreventInstantiatePatch.RunInstantiate = true;

            // _morePathsCore.CustomPaths = _morePathsCore.PathsSpecifications.Select(specification => new CustomPath(_prefabOptimizationChain, _materialRepository, _morePathsCore, _optimizedPrefabInstantiator.InstantiateInactive(originalPathGameObject, new GameObject().transform, out bool _), Object.Instantiate(PathCorner), specification)).ToList();

            // stopwatch.Stop();
            // Plugin.Log.LogInfo("Total: " + stopwatch.ElapsedMilliseconds);
            return test;
        }
    }
}
