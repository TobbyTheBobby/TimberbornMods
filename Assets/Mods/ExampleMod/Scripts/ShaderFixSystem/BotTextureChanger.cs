using Bindito.Core;
using Timberborn.AssetSystem;
using Timberborn.BaseComponentSystem;
using Timberborn.GameFactionSystem;
using UnityEngine;

namespace ExampleMod.ShaderFixSystem
{
    public class BotTextureChanger : BaseComponent
    {
        private FactionService _factionService;
        private IAssetLoader _assetLoader;
        
        [Inject]
        public void InjectDependencies(FactionService factionService, IAssetLoader assetLoader)
        {
            _factionService = factionService;
            _assetLoader = assetLoader;
        }
        
        private void Awake()
        {
            if (_factionService.Current.Id == "Folktails")
            {
                GetComponentInChildren<MeshRenderer>().material.SetTexture("_BaseMap", _assetLoader.Load<Texture2D>("Sprites/Paths/Brickpath/GroundTexture"));
            }
        }
    }
}