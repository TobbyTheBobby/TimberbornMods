using ChooChoo.GoodsStations;
using Timberborn.CoreUI;
using Timberborn.Localization;
using Timberborn.SliderToggleSystem;
using UnityEngine.UIElements;

namespace ChooChoo.GoodsStationBatchControl
{
    public class ImportToggleFactory
    {
        private static readonly string DistributionDisabledIconClass = "import-icon--disabled";
        private static readonly string DistributionSendingIconClass = "stockpile-priority-toggle__icon--empty";
        private static readonly string DistributionReceivingIconClass = "stockpile-priority-toggle__icon--obtain";
        private static readonly string DistributionDisabledBackgroundClass = "import-background--disabled";
        private static readonly string DistributionActiveBackgroundClass = null;
        private static readonly string DistributionDisabledLocKey = "Tobbert.BatchControl.DistributionDisabled";
        private static readonly string DistributionDisabledDescriptionLocKey = "Tobbert.BatchControl.DistributionDisabled.Description";
        private static readonly string DistributionSendingLocKey = "Tobbert.BatchControl.DistributionSending";
        private static readonly string DistributionSendingDescriptionLocKey = "Tobbert.BatchControl.DistributionSending.Description";
        private static readonly string DistributionReceivingLocKey = "Tobbert.BatchControl.DistributionReceiving";
        private static readonly string DistributionReceivingDescriptionLocKey = "Tobbert.BatchControl.DistributionReceiving.Description";

        private readonly VisualElementLoader _visualElementLoader;
        private readonly SliderToggleFactory _sliderToggleFactory;
        private readonly ILoc _loc;

        public ImportToggleFactory(
            VisualElementLoader visualElementLoader,
            SliderToggleFactory sliderToggleFactory,
            ILoc loc)
        {
            _visualElementLoader = visualElementLoader;
            _sliderToggleFactory = sliderToggleFactory;
            _loc = loc;
        }

        public SliderToggle Create(VisualElement parent, GoodsStationGoodDistributionSetting setting)
        {
            var sliderToggleItem1 = SliderToggleItem.Create(
                GetDistributionDisabledTooltip,
                DistributionDisabledIconClass,
                DistributionDisabledBackgroundClass,
                () =>
                {
                    setting.SetImportOption(DistributionOption.Disabled);
                    setting.SetMaxCapacity(0);
                },
                () => setting.DistributionOption == DistributionOption.Disabled);

            var sliderToggleItem2 = SliderToggleItem.Create(
                GetDistributionSendingTooltip,
                DistributionSendingIconClass,
                DistributionActiveBackgroundClass,
                () =>
                {
                    setting.SetImportOption(DistributionOption.Sending);
                    setting.SetMaxCapacity(50);
                },
                () => setting.DistributionOption == DistributionOption.Sending);

            var sliderToggleItem3 = SliderToggleItem.Create(
                GetDistributionReceivingTooltip,
                DistributionReceivingIconClass,
                DistributionActiveBackgroundClass,
                () =>
                {
                    setting.SetImportOption(DistributionOption.Receiving);
                    setting.SetMaxCapacity(50);
                },
                () => setting.DistributionOption == DistributionOption.Receiving);
            return _sliderToggleFactory.Create(parent, sliderToggleItem1, sliderToggleItem2, sliderToggleItem3);
        }

        private VisualElement GetDistributionDisabledTooltip()
        {
            return GetTooltip(DistributionDisabledLocKey, DistributionDisabledDescriptionLocKey);
        }

        private VisualElement GetDistributionSendingTooltip()
        {
            return GetTooltip(DistributionSendingLocKey, DistributionSendingDescriptionLocKey);
        }

        private VisualElement GetDistributionReceivingTooltip()
        {
            return GetTooltip(DistributionReceivingLocKey, DistributionReceivingDescriptionLocKey);
        }

        private VisualElement GetTooltip(string title, string description)
        {
            var e = _visualElementLoader.LoadVisualElement("Game/ImportToggleTooltip");
            e.Q<Label>("Title").text = _loc.T(title);
            e.Q<Label>("Description").text = _loc.T(description);
            return e;
        }
    }
}