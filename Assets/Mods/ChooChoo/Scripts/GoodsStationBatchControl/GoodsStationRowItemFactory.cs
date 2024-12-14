using ChooChoo.GoodsStations;
using Timberborn.AssetSystem;
using Timberborn.BaseComponentSystem;
using Timberborn.BatchControl;
using Timberborn.CharactersUI;
using Timberborn.CoreUI;
using Timberborn.EntityPanelSystem;
using Timberborn.EntitySystem;
using Timberborn.SelectionSystem;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace ChooChoo.GoodsStationBatchControl
{
    public class GoodsStationRowItemFactory
    {
        private readonly EntitySelectionService _entitySelectionService;
        private readonly VisualElementLoader _visualElementLoader;
        private readonly EntityBadgeService _entityBadgeService;
        private readonly InputBoxShower _inputBoxShower;
        private readonly IAssetLoader _assetLoader;

        public GoodsStationRowItemFactory(EntitySelectionService entitySelectionService, VisualElementLoader visualElementLoader, EntityBadgeService entityBadgeService, InputBoxShower inputBoxShower, IAssetLoader assetLoader)
        {
            _entitySelectionService = entitySelectionService;
            _visualElementLoader = visualElementLoader;
            _entityBadgeService = entityBadgeService;
            _inputBoxShower = inputBoxShower;
            _assetLoader = assetLoader;
        }

        public IBatchControlRowItem Create(GoodsStation goodsStation)
        {
            var visualElement = _visualElementLoader.LoadVisualElement("Game/BatchControl/DistrictCenterRowItem");
            visualElement.Q<Image>("Image").sprite = _assetLoader.Load<Sprite>(goodsStation.GetComponentFast<LabeledEntitySpec>().ImagePath);
            visualElement.Q<Button>("Select").RegisterCallback((EventCallback<ClickEvent>)(_ => _entitySelectionService.SelectAndFocusOn(goodsStation)));
            
            var originalLabel = visualElement.Q<Label>("Text");
            originalLabel.ToggleDisplayStyle(false);
            
            var characterVisualElement = _visualElementLoader.LoadVisualElement("Game/BatchControl/CharacterBatchControlRowItem");
            var entityName = characterVisualElement.Q<Button>("EntityName");
            Debug.Log($"Width: {entityName.style.width}");
            entityName.style.width = new Length(100, LengthUnit.Percent);
            entityName.RegisterCallback<ClickEvent>(_ => OnEntityNameClicked(goodsStation));
            originalLabel.parent.Insert(originalLabel.parent.IndexOf(originalLabel), entityName);
            
            return new GoodsStationRowItem(visualElement, goodsStation, entityName.Q<Label>("EntityNameText"), _entityBadgeService);
        }
        
        private void OnEntityNameClicked(GoodsStation goodsStation)
        {
            _inputBoxShower.Create().SetLocalizedMessage(CharacterBatchControlRowItemFactory.ChangeNameLocKey).SetConfirmButton(value => SetEntityName(value, goodsStation)).Show();
        }
        
        private void SetEntityName(string newName, BaseComponent goodsStation)
        {
            if (!(bool) (Object) goodsStation || string.IsNullOrWhiteSpace(newName))
                return;
            _entityBadgeService.SetEntityName(goodsStation, newName.Trim());
        }
    }
}