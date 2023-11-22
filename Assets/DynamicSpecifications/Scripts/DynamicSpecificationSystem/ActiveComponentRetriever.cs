using System.Linq;
using Bindito.Unity;
using Timberborn.AssetSystem;
using UnityEngine;

namespace DynamicSpecifications
{
    public class ActiveComponentRetriever
    {
        private readonly IResourceAssetLoader _resourceAssetLoader;

        ActiveComponentRetriever(IResourceAssetLoader resourceAssetLoader)
        {
            _resourceAssetLoader = resourceAssetLoader;
        }
      
        // public GameObject[] GetAllPrefabs()
        // {
        //     var prefabConfigurators = GetAllPrefabConfigurators();
        //     var allPrefabConfiguratorComponents = prefabConfigurators.SelectMany(configurator => configurator.gameObject.Concat(configurator.GetComponentsInChildren<Component>()));
        //     var prefabs = GetAllGameObjects();
        //     return prefabConfigurators.Concat(prefabs);
        // }
        
        public Component[] GetAllComponents()
        {
            
            var prefabConfigurators = GetAllPrefabConfigurators();
            var allPrefabConfiguratorComponents = prefabConfigurators.SelectMany(configurator => configurator.GetComponents<Component>().Concat(configurator.GetComponentsInChildren<Component>()));
            var prefabs = GetAllGameObjects();
            var allPrefabComponents = prefabs.SelectMany(prefab => prefab.GetComponents<Component>());
            var allComponents = allPrefabComponents.Concat(allPrefabConfiguratorComponents);
            return allComponents.ToArray();
        }

        private PrefabConfigurator[] GetAllPrefabConfigurators()
        {
            return Object.FindObjectsOfType<PrefabConfigurator>(true);
        }

        private GameObject[] GetAllGameObjects()
        {
            return _resourceAssetLoader.LoadAll<GameObject>("");
        }
    }
}