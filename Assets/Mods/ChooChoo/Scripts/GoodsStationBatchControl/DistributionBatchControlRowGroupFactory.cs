using ChooChoo.GoodsStations;
using Timberborn.BatchControl;
using Timberborn.CoreUI;
using Timberborn.EntitySystem;

namespace ChooChoo.GoodsStationBatchControl
{
    public class DistributionBatchControlRowGroupFactory
    {
        private readonly DistrictDistributionControlRowItemFactory _districtDistributionControlRowItemFactory;
        private readonly DistributionSettingsRowItemFactory _distributionSettingsRowItemFactory;
        private readonly BatchControlRowGroupFactory _batchControlRowGroupFactory;
        private readonly GoodsStationRowItemFactory _goodsStationRowItemFactory;
        private readonly VisualElementLoader _visualElementLoader;

        public DistributionBatchControlRowGroupFactory(
            DistrictDistributionControlRowItemFactory districtDistributionControlRowItemFactory,
            DistributionSettingsRowItemFactory distributionSettingsRowItemFactory,
            BatchControlRowGroupFactory batchControlRowGroupFactory,
            GoodsStationRowItemFactory goodsStationRowItemFactory,
            VisualElementLoader visualElementLoader)
        {
            _districtDistributionControlRowItemFactory = districtDistributionControlRowItemFactory;
            _distributionSettingsRowItemFactory = distributionSettingsRowItemFactory;
            _batchControlRowGroupFactory = batchControlRowGroupFactory;
            _goodsStationRowItemFactory = goodsStationRowItemFactory;
            _visualElementLoader = visualElementLoader;
        }

        public BatchControlRowGroup Create(GoodsStation goodsStation)
        {
            var root = _visualElementLoader.LoadVisualElement("Game/BatchControl/BatchControlRow");
            var componentFast1 = goodsStation.GetComponentFast<GoodsStationDistributionSettings>();
            var batchControlRowItem1 = _goodsStationRowItemFactory.Create(goodsStation);
            var batchControlRowItem2 = _districtDistributionControlRowItemFactory.Create(componentFast1);
            var componentFast2 = goodsStation.GetComponentFast<EntityComponent>();
            IBatchControlRowItem[] batchControlRowItemArray =
            {
                batchControlRowItem1,
                batchControlRowItem2
            };
            var unsorted = _batchControlRowGroupFactory.CreateUnsorted(new BatchControlRow(root, componentFast2, batchControlRowItemArray));
            unsorted.AddRow(_distributionSettingsRowItemFactory.Create(componentFast1));
            return unsorted;
        }
    }
}