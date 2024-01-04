using System.Collections.Generic;
using Timberborn.AssetSystem;
using Timberborn.PrefabSystem;
using UnityEngine;

namespace ChooChoo
{
    public class TrainObjectCollector : IObjectCollection
    {
        private readonly IResourceAssetLoader _resourceAssetLoader;
        
        TrainObjectCollector(IResourceAssetLoader resourceAssetLoader)
        {
            _resourceAssetLoader = resourceAssetLoader;
        }
        
        public IEnumerable<GameObject> GetObjects()
        {
            List<GameObject> list = new List<GameObject>
            {
                _resourceAssetLoader.Load<GameObject>("tobbert.choochoo/tobbert_choochoo/Train"),
                _resourceAssetLoader.Load<GameObject>("tobbert.choochoo/tobbert_choochoo/Wagon")
            };
            return list;
        }
    }
}