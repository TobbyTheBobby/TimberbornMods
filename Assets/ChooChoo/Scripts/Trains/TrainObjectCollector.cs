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
        
        public IEnumerable<Object> GetObjects()
        {
            List<Object> list = new List<Object>
            {
                _resourceAssetLoader.Load<GameObject>("tobbert.choochoo/tobbert_choochoo/Train"),
                _resourceAssetLoader.Load<GameObject>("tobbert.choochoo/tobbert_choochoo/Wagon")
            };
            return list;
        }
    }
}