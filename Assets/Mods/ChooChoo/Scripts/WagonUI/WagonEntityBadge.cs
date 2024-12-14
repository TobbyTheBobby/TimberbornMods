using Bindito.Core;
using ChooChoo.Wagons;
using Timberborn.AssetSystem;
using Timberborn.BaseComponentSystem;
using Timberborn.Characters;
using Timberborn.EntityPanelSystem;
using Timberborn.Localization;
using Timberborn.SelectionSystem;
using UnityEngine;

namespace ChooChoo.WagonUI
{
    public class WagonEntityBadge : BaseComponent, IEntityBadge
    {
        private readonly string _trainDisplayNameLocKey = "Tobbert.Train.Name";
        private readonly string _ageLocKey = "Beaver.Age";

        private EntitySelectionService _selectionManager;
        private IAssetLoader _assetLoader;
        private ILoc _loc;

        private TrainWagon _trainWagon;
        private Character _character;
        private Sprite _sprite;

        [Inject]
        public void InjectDependencies(
            EntitySelectionService selectionManager,
            IAssetLoader resourceAssetLoader,
            ILoc loc)
        {
            _selectionManager = selectionManager;
            _assetLoader = resourceAssetLoader;
            _loc = loc;
        }

        public int EntityBadgePriority => 1;

        public void Awake()
        {
            _character = GetComponentFast<Character>();
            _sprite = _assetLoader.Load<Sprite>("tobbert.choochoo/tobbert_choochoo/ToolGroupIcon");
        }

        public string GetEntityName()
        {
            return $"<b>{_character.FirstName}</b>";
        }

        public string GetEntitySubtitle()
        {
            return Age();
        }

        public ClickableSubtitle GetEntityClickableSubtitle()
        {
            var train = _trainWagon.Train;
            return ClickableSubtitle.Create(() => _selectionManager.SelectAndFocusOn(train), _loc.T(_trainDisplayNameLocKey));
        }

        public Sprite GetEntityAvatar()
        {
            return _sprite;
        }

        private string Age()
        {
            return _loc.T(_ageLocKey, _character.Age);
        }
    }
}