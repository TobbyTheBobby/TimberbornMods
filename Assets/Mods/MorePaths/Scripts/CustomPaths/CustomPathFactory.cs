using MorePaths.PathSpecificationSystem;
using MorePaths.Settings;
using Timberborn.AssetSystem;
using Timberborn.BaseComponentSystem;
using Timberborn.GameFactionSystem;
using Timberborn.PathSystem;
using Timberborn.TemplateSystem;
using UnityEngine;

namespace MorePaths.CustomPaths
{
    public class CustomPathFactory
    {
        private static FactionService _factionService;
        private static IAssetLoader _assetLoader;
        private readonly TemplateInstantiator _templateInstantiator;
        private readonly MorePathsSettings _morePathsSettings;
        private readonly BaseInstantiator _baseInstantiator;

        private GameObject customPathPrefab;

        private CustomPathFactory(
            FactionService factionService,
            IAssetLoader assetLoader,
            TemplateInstantiator templateInstantiator,
            MorePathsSettings morePathsSettings,
            BaseInstantiator baseInstantiator)
        {
            _factionService = factionService;
            _assetLoader = assetLoader;
            _templateInstantiator = templateInstantiator;
            _morePathsSettings = morePathsSettings;
            _baseInstantiator = baseInstantiator;
        }

        public static GameObject PathCorner => _assetLoader.Load<GameObject>("Tobbert/Prefabs/PathCorner");

        public static Material ActivePathMaterial => _assetLoader.Load<Material>(_factionService.Current.PathMaterial);

        public CustomPath CreateCustomPath(DynamicPathModel dynamicPathModel, PathSpecification pathSpecification)
        {
            // var stopwatch = Stopwatch.StartNew();

            Debug.Log($"Creating path: {pathSpecification.Name}");
            
            var gameObject = _baseInstantiator.InstantiateInactive(dynamicPathModel.GameObjectFast, new GameObject().transform);
            // gameObject.name = pathSpecification.Name;
            // var template =  _templateInstantiator.Instantiate(gameObject, new GameObject().transform);
            // var customPath = template.GetComponent<CustomPath>();
            var customPath = _baseInstantiator.AddComponent<CustomPath>(gameObject);
            _baseInstantiator.AddComponent<DynamicPathCorner>(gameObject);
            customPath.name = pathSpecification.Name;
            customPath.SetSpecification(pathSpecification);
            
            // stopwatch.Stop();
            // Plugin.Log.LogInfo($"Created {customPaths.Count} paths in {stopwatch.ElapsedMilliseconds}ms.");
            return customPath;
        }
    }
}