using System;
using Timberborn.Goods;

namespace ChooChoo.GoodsStations
{
    public class GoodsStationGoodDistributionSetting
    {
        private readonly GoodSpecification _goodSpecification;

        public event EventHandler SettingChanged;

        public int MaxCapacity { get; private set; }

        public DistributionOption DistributionOption { get; private set; }

        public float LastImportTimestamp { get; private set; }

        private GoodsStationGoodDistributionSetting(GoodSpecification goodSpecification)
        {
            _goodSpecification = goodSpecification;
        }

        public static GoodsStationGoodDistributionSetting CreateDefault(GoodSpecification goodSpecification)
        {
            var distributionSetting = new GoodsStationGoodDistributionSetting(goodSpecification);
            distributionSetting.SetDefault();
            return distributionSetting;
        }

        public static GoodsStationGoodDistributionSetting Create(
            GoodSpecification goodSpecification,
            int maxCapacity,
            DistributionOption distributionOption,
            float lastImportTimestamp)
        {
            return new GoodsStationGoodDistributionSetting(goodSpecification)
            {
                MaxCapacity = maxCapacity,
                DistributionOption = distributionOption,
                LastImportTimestamp = lastImportTimestamp
            };
        }

        public string GoodId => _goodSpecification.Id;

        public void SetDefault()
        {
            MaxCapacity = 0;
            DistributionOption = DistributionOption.Disabled;
            var settingChanged = SettingChanged;
            if (settingChanged == null)
                return;
            settingChanged(this, EventArgs.Empty);
        }

        public void SetImportOption(DistributionOption distributionOption)
        {
            DistributionOption = distributionOption;
            var settingChanged = SettingChanged;
            if (settingChanged == null)
                return;
            settingChanged(this, EventArgs.Empty);
        }

        public void SetMaxCapacity(int maxCapacity)
        {
            MaxCapacity = maxCapacity;
            var settingChanged = SettingChanged;
            if (settingChanged == null)
                return;
            settingChanged(this, EventArgs.Empty);
        }
    }
}