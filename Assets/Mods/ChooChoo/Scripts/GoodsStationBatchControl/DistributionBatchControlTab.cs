using System.Collections.Generic;
using ChooChoo.GoodsStations;
using Timberborn.BatchControl;
using Timberborn.CoreUI;
using Timberborn.EntitySystem;
using TobbyTools.BuildingRegistrySystem;

namespace ChooChoo.GoodsStationBatchControl
{
    public class DistributionBatchControlTab : BatchControlTab
    {
        public static readonly int TabIndex = 9;

        private readonly BuildingRegistry<GoodsStation> _goodsStationRegistry;
        private readonly DistributionBatchControlRowGroupFactory _distributionBatchControlRowGroupFactory;

        public DistributionBatchControlTab(
            VisualElementLoader visualElementLoader,
            BatchControlDistrict batchControlDistrict,
            BuildingRegistry<GoodsStation> goodsStationRegistry,
            DistributionBatchControlRowGroupFactory distributionBatchControlRowGroupFactory)
            : base(visualElementLoader, batchControlDistrict)
        {
            _goodsStationRegistry = goodsStationRegistry;
            _distributionBatchControlRowGroupFactory = distributionBatchControlRowGroupFactory;
        }

        public override string TabNameLocKey => "Tobbert.BatchControl.GoodsStation";

        public override string TabImage => "Distribution";

        public override string BindingKey => "Tobbert.ChooChoo.KeyBinding.GoodsStationDistributionControlTab";

        public override IEnumerable<BatchControlRowGroup> GetRowGroups(IEnumerable<EntityComponent> entities)
        {
            foreach (var finishedGoodsStation in _goodsStationRegistry.Finished)
                yield return _distributionBatchControlRowGroupFactory.Create(finishedGoodsStation);
        }
    }
}