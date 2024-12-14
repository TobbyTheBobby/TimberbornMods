using System.Collections.Generic;
using System.Linq;
using Timberborn.AssetSystem;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MorePaths.CustomPaths
{
    public class CustomPathsAssetsProvider : IAssetProvider
    {
        private readonly CustomPathsRepository _customPathsRepository;

        public CustomPathsAssetsProvider(CustomPathsRepository customPathsRepository)
        {
            _customPathsRepository = customPathsRepository;
        }

        public bool TryLoad<T>(string path, out OrderedAsset<T> orderedAsset) where T : Object
        {
            if (typeof(T) != typeof(GameObject))
            {
                orderedAsset = new OrderedAsset<T>();
                return false;
            }
            
            // Debug.LogWarning(path);
            //
            // foreach (var pair in _customPathsRepository.CustomPaths)
            // {
            //     Debug.LogError(pair.Key);
            // }
            
            if (_customPathsRepository.CustomPaths.TryGetValue(path, out var gameObject))
            {
                orderedAsset = CreateOrderedSpecificationAsset(gameObject).As<T>();
                return true;
            }
            
            orderedAsset = new OrderedAsset<T>();
            return false;
        }

        public IEnumerable<OrderedAsset<T>> LoadAll<T>(string path) where T : Object
        {
            if (typeof(T) == typeof(GameObject))
                return _customPathsRepository.CustomPaths.Select(pair => CreateOrderedSpecificationAsset(pair.Value).As<T>());

            return new List<OrderedAsset<T>>();
        }

        public void Reset()
        {
            _customPathsRepository.CustomPaths.Clear();
        }

        public bool IsBuiltIn => false;

        private static OrderedAsset<T> CreateOrderedSpecificationAsset<T>(T customPath)
        {
            return new OrderedAsset<T>(-1, customPath);
        }
    }
}