using System.Collections.Generic;
using Timberborn.BatchControl;
using Timberborn.CoreUI;
using Timberborn.EntitySystem;

namespace ChooChoo
{
  public class DistributionBatchControlTab : BatchControlTab
  {
    public static readonly int TabIndex = 9;
    private readonly GoodsStationRegistry _goodsStationRegistry;
    private readonly DistributionBatchControlRowGroupFactory _distributionBatchControlRowGroupFactory;
    private BatchControlTab _batchControlTabImplementation;

    public DistributionBatchControlTab(
      VisualElementLoader visualElementLoader,
      BatchControlDistrict batchControlDistrict,
      GoodsStationRegistry goodsStationRegistry,
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
      foreach (GoodsStation finishedGoodsStation in _goodsStationRegistry.FinishedGoodsStations)
        yield return _distributionBatchControlRowGroupFactory.Create(finishedGoodsStation);
    }
  }
}
