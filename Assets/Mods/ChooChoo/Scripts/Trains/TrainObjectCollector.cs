using System.Collections.Generic;
using Timberborn.AssetSystem;
using Timberborn.PrefabSystem;
using UnityEngine;

namespace ChooChoo.Trains
{
    public class TrainObjectCollector : IObjectCollection
    {
        private readonly IResourceAssetLoader _resourceAssetLoader;

        private TrainObjectCollector(IResourceAssetLoader resourceAssetLoader)
        {
            _resourceAssetLoader = resourceAssetLoader;
        }
        
        public IEnumerable<GameObject> GetObjects()
        {
            var list = new List<GameObject>
            {
                _resourceAssetLoader.Load<GameObject>("tobbert.choochoo/tobbert_choochoo/Train"),
                _resourceAssetLoader.Load<GameObject>("tobbert.choochoo/tobbert_choochoo/Wagon")
            };
            return list;
        }
    }
}