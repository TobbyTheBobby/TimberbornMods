using Timberborn.AssetSystem;
using Timberborn.Bots;
using Timberborn.GameFactionSystem;
using Timberborn.SingletonSystem;
using Timberborn.Timbermesh;
using UnityEngine;

namespace ExampleMod.ShaderFixSystem
{
    public class ShaderFix : ILoadableSingleton, IUnloadableSingleton
    {
        private static readonly int BaseMap = Shader.PropertyToID("_BaseMap");
        
        private readonly IMaterialRepository _materialRepository;
        private readonly IAssetLoader _assetLoader;
        private readonly FactionService _factionService;

        private Material _botFolktails;
        private Material _backup;

        public ShaderFix(IMaterialRepository materialRepository, IAssetLoader assetLoader, FactionService factionService, BotFactory botFactory)
        {
            _materialRepository = materialRepository;
            _assetLoader = assetLoader;
            _factionService = factionService;
        }

        public void Load()
        {
            if (_factionService.Current.Id == "Folktails")
            {
                _botFolktails = _materialRepository.GetMaterial("Bot.Folktails");
                
                _backup = new Material(_botFolktails);

                var texture = _assetLoader.Load<Texture2D>("Sprites/Paths/Brickpath/GroundTexture");

                _botFolktails.SetTexture(BaseMap, texture);
            }
        }

        public void Unload()
        {
            if (_botFolktails == null || _backup == null)
                return;
            _botFolktails.SetTexture(BaseMap, _backup.GetTexture(BaseMap));
        }
    }
}