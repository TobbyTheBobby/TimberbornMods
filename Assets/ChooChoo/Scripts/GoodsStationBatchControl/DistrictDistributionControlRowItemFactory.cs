using ChooChoo.GoodsStations;
using Timberborn.AssetSystem;
using Timberborn.BatchControl;
using Timberborn.CoreUI;
using Timberborn.SingletonSystem;
using Timberborn.TooltipSystem;
using UnityEngine;
using UnityEngine.UIElements;

namespace ChooChoo.GoodsStationBatchControl
{
    public class DistrictDistributionControlRowItemFactory : ILoadableSingleton
    {
        private static readonly string ResetLocKey = "Distribution.Reset";
        private static readonly string DistributionDisabledAllLocKey = "Tobbert.BatchControl.DistributionDisabledAll";
        private static readonly string DistributionSendingAllLocKey = "Tobbert.BatchControl.DistributionSendingAll";
        private static readonly string DistributionReceivingAllLocKey = "Tobbert.BatchControl.DistributionReceivingAll";
        private readonly ITooltipRegistrar _tooltipRegistrar;
        private readonly VisualElementLoader _visualElementLoader;
        private readonly IResourceAssetLoader _resourceAssetLoader;

        private Texture2D _sendingIcon;
        private Texture2D _receivingIcon;

        public DistrictDistributionControlRowItemFactory(ITooltipRegistrar tooltipRegistrar, VisualElementLoader visualElementLoader,
            IResourceAssetLoader resourceAssetLoader)
        {
            _tooltipRegistrar = tooltipRegistrar;
            _visualElementLoader = visualElementLoader;
            _resourceAssetLoader = resourceAssetLoader;
        }

        public IBatchControlRowItem Create(GoodsStationDistributionSettings goodsStationDistributionSettings)
        {
            var visualElement = _visualElementLoader.LoadVisualElement("Game/BatchControl/DistrictDistributionControlRowItem");
            var button1 = visualElement.Q<Button>("Reset");
            button1.RegisterCallback((EventCallback<ClickEvent>)(_ => goodsStationDistributionSettings.ResetToDefault()));
            _tooltipRegistrar.RegisterLocalizable(button1, ResetLocKey);
            var button2 = visualElement.Q<Button>("ExportAll");
            button2.ToggleDisplayStyle(false);
            var button3 = visualElement.Q<Button>("ExportNone");
            button3.ToggleDisplayStyle(false);
            var button4 = visualElement.Q<Button>("ImportDisabledAll");
            button4.RegisterCallback((EventCallback<ClickEvent>)(_ =>
                goodsStationDistributionSettings.SetDistrictImportOption(DistributionOption.Disabled)));
            _tooltipRegistrar.RegisterLocalizable(button4, DistributionDisabledAllLocKey);
            var button5 = visualElement.Q<Button>("ImportAutoAll");
            button5.RegisterCallback((EventCallback<ClickEvent>)(_ =>
                goodsStationDistributionSettings.SetDistrictImportOption(DistributionOption.Sending)));
            _tooltipRegistrar.RegisterLocalizable(button5, DistributionSendingAllLocKey);
            button5.Q<VisualElement>("Icon").style.backgroundImage = _sendingIcon;
            var button6 = visualElement.Q<Button>("ImportForcedAll");
            button6.RegisterCallback((EventCallback<ClickEvent>)(_ =>
                goodsStationDistributionSettings.SetDistrictImportOption(DistributionOption.Receiving)));
            _tooltipRegistrar.RegisterLocalizable(button6, DistributionReceivingAllLocKey);
            button6.Q<VisualElement>("Icon").style.backgroundImage = _receivingIcon;
            return new EmptyBatchControlRowItem(visualElement);
        }

        public void Load()
        {
            _sendingIcon = _resourceAssetLoader.Load<Texture2D>("tobbert.choochoo/tobbert_choochoo/empty-icon");
            _receivingIcon = _resourceAssetLoader.Load<Texture2D>("tobbert.choochoo/tobbert_choochoo/obtain-icon");
        }
    }
}