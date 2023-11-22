using System.Collections.Generic;
using Timberborn.AssetSystem;
using Timberborn.PrefabSystem;
using UnityEngine;

namespace GlobalMarket
{
    public class AirBalloonObjectCollector : IObjectCollection
    {
        private readonly IResourceAssetLoader _resourceAssetLoader;
        
        AirBalloonObjectCollector(IResourceAssetLoader resourceAssetLoader)
        {
            _resourceAssetLoader = resourceAssetLoader;
        }
        
        public IEnumerable<Object> GetObjects()
        {
            List<Object> list = new List<Object>
            {
                _resourceAssetLoader.Load<GameObject>("tobbert.globalmarket/tobbert_globalmarket/AirBalloon.Folktails"),
                _resourceAssetLoader.Load<GameObject>("tobbert.globalmarket/tobbert_globalmarket/AirBalloon.IronTeeth")
            };
            return list;
        }
    }
}