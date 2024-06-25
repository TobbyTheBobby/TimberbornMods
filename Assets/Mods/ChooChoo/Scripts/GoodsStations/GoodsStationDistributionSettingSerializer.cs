using System;
using Timberborn.Goods;
using Timberborn.Persistence;

namespace ChooChoo.GoodsStations
{
    public class GoodsStationDistributionSettingSerializer : IObjectSerializer<GoodsStationGoodDistributionSetting>
    {
        private static readonly PropertyKey<string> GoodIdKey = new("GoodId");
        private static readonly PropertyKey<int> MaxCapacityKey = new("MaxCapacity");
        private static readonly PropertyKey<DistributionOption> DistributionOptionKey = new("DistributionOption");
        private static readonly PropertyKey<float> LastImportTimestampKey = new("LastImportTimestamp");

        private readonly IGoodService _goodService;
        private readonly EnumObjectSerializer<DistributionOption> _importOptionSerializer;

        public GoodsStationDistributionSettingSerializer(IGoodService goodService, EnumObjectSerializer<DistributionOption> importOptionSerializer)
        {
            _goodService = goodService;
            _importOptionSerializer = importOptionSerializer;
        }

        public void Serialize(GoodsStationGoodDistributionSetting value, IObjectSaver objectSaver)
        {
            objectSaver.Set(GoodIdKey, value.GoodId);
            objectSaver.Set(MaxCapacityKey, value.MaxCapacity);
            objectSaver.Set(DistributionOptionKey, value.DistributionOption, _importOptionSerializer);
            objectSaver.Set(LastImportTimestampKey, value.LastImportTimestamp);
        }

        public Obsoletable<GoodsStationGoodDistributionSetting> Deserialize(IObjectLoader objectLoader)
        {
            var goodOrNull = _goodService.GetGoodOrNull(objectLoader.Get(GoodIdKey));
            if (goodOrNull == null)
                return new Obsoletable<GoodsStationGoodDistributionSetting>();
            var maxCapacity = objectLoader.Get(MaxCapacityKey);
            var importOption = DistributionOption.Disabled;
            try
            {
                importOption = objectLoader.Get(DistributionOptionKey, _importOptionSerializer);
            }
            catch (Exception)
            {
                // ignored
            }

            var lastImportTimestamp = objectLoader.Has(LastImportTimestampKey) ? objectLoader.Get(LastImportTimestampKey) : 0.0f;
            return GoodsStationGoodDistributionSetting.Create(goodOrNull, maxCapacity, importOption, lastImportTimestamp);
        }
    }
}