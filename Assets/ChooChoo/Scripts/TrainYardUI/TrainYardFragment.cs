using System.Collections.Generic;
using System.Linq;
using ChooChoo.Trains;
using ChooChoo.TrainYards;
using TimberApi.UiBuilderSystem;
using Timberborn.AssetSystem;
using Timberborn.BaseComponentSystem;
using Timberborn.CoreUI;
using Timberborn.EntityPanelSystem;
using Timberborn.GoodsUI;
using Timberborn.InventorySystem;
using Timberborn.InventorySystemUI;
using Timberborn.Localization;
using UnityEngine;
using UnityEngine.UIElements;
using Button = UnityEngine.UIElements.Button;

namespace ChooChoo.TrainYardUI
{
    internal class TrainYardFragment : IEntityPanelFragment
    {
        private const string CreateTrainLocKey = "Tobbert.TrainYard.CreateTrain";
        private readonly InformationalRowsFactory _informationalRowsFactory;
        private readonly VisualElementLoader _visualElementLoader;
        private readonly IResourceAssetLoader _resourceAssetLoader;
        private readonly GoodDescriber _goodDescriber;
        private readonly UIBuilder _uiBuilder;
        private readonly ILoc _loc;
        private TrainYard _trainYard;
        private VisualElement _root;
        private Inventory _inventory;
        private Label _costLabel;
        private Button _createButton;
        private Train _train;

        private ScrollView _inventoryContent;
        private readonly List<InformationalRow> _rows = new();

        public TrainYardFragment(
            InformationalRowsFactory informationalRowsFactory,
            VisualElementLoader visualElementLoader,
            IResourceAssetLoader resourceAssetLoader,
            GoodDescriber goodDescriber,
            UIBuilder uiBuilder,
            ILoc loc)
        {
            _informationalRowsFactory = informationalRowsFactory;
            _visualElementLoader = visualElementLoader;
            _resourceAssetLoader = resourceAssetLoader;
            _goodDescriber = goodDescriber;
            _uiBuilder = uiBuilder;
            _loc = loc;
        }

        public VisualElement InitializeFragment()
        {
            _train = _resourceAssetLoader.Load<GameObject>("tobbert.choochoo/tobbert_choochoo/Train").GetComponent<Train>();


            _root = new VisualElement();
            var firstFragment = _uiBuilder.CreateFragmentBuilder()
                .AddComponent(builder => builder
                    .SetFlexDirection(FlexDirection.Column)
                    .SetWidth(new Length(100, LengthUnit.Percent))
                    .SetJustifyContent(Justify.Center)
                    .AddPreset(builder => builder
                        .Labels()
                        .GameText(
                            name: "CostLabel",
                            builder: builder => builder
                                .SetWidth(new Length(100, LengthUnit.Percent))
                                .SetJustifyContent(Justify.Center)
                        )
                    )
                    .AddPreset(factory => factory.Buttons().Button(CreateTrainLocKey, name: "CreateButton")))
                .BuildAndInitialize();

            _root.Add(firstFragment);

            _costLabel = firstFragment.Q<Label>("CostLabel");
            _createButton = firstFragment.Q<Button>("CreateButton");
            _createButton.Q<Button>("CreateButton").clicked += () => _trainYard.InitializeTrain();

            var secondFragment = _uiBuilder.CreateFragmentBuilder().BuildAndInitialize();
            _root.Add(secondFragment);

            var manufactoryInventoryFragment = _visualElementLoader.LoadVisualElement("Game/EntityPanel/ManufactoryInventoryFragment");
            _inventoryContent = manufactoryInventoryFragment.Q<ScrollView>("Content");
            secondFragment.Add(_inventoryContent);

            _root.ToggleDisplayStyle(false);
            return _root;
        }

        public void ShowFragment(BaseComponent entity)
        {
            _trainYard = entity.GetComponentFast<TrainYard>();
            if (!_trainYard)
                ClearFragment();
            else
            {
                _inventory = _trainYard.Inventory;
                var costDescription = $"{_loc.T("Tobbert.TrainYard.CostOfTrain")}\n";
                foreach (var goodAmountSpecification in _train.TrainCost)
                    costDescription += $"{_goodDescriber.Describe(goodAmountSpecification.GoodId)}: {goodAmountSpecification.Amount}\n";
                _costLabel.text = costDescription;
                AddRows();
            }
        }

        public void ClearFragment()
        {
            _inventoryContent.Clear();
            _rows.Clear();
            _trainYard = null;
            _inventory = null;
            _root.ToggleDisplayStyle(false);
        }

        public void UpdateFragment()
        {
            if (_inventory && _inventory.enabled)
            {
                _root.ToggleDisplayStyle(true);
                foreach (var row in _rows)
                {
                    if (ShouldShow(row.GoodId))
                    {
                        row.ShowUpdated();
                    }
                    else
                        row.Hide();
                }

                var flag = true;
                foreach (var goodAmountSpecification in _train.TrainCost)
                    if (_trainYard.Inventory.AmountInStock(goodAmountSpecification.GoodId) < goodAmountSpecification.Amount)
                        flag = false;
                _createButton.SetEnabled(flag);
            }
            else
                _root.ToggleDisplayStyle(false);
        }

        private void AddRows()
        {
            var goods = _inventory.AllowedGoods.OrderBy(good => _goodDescriber.Describe(good.StorableGood.GoodId)).ToList();

            foreach (var storableGoodAmount in goods)
            {
                var storableGood = storableGoodAmount.StorableGood;
                _rows.Add(_informationalRowsFactory.CreateInputRowWithLimit(storableGood, _inventory, _inventoryContent));
            }

            // List<StorableGoodAmount> list = _inventory.AllowedGoods.OrderBy(good => _goodDescriber.Describe(good.StorableGood.GoodId)).ToList<StorableGoodAmount>();
        }

        private bool ShouldShow(string goodId)
        {
            return _inventory.LimitedAmount(goodId) > 0;
        }
    }
}