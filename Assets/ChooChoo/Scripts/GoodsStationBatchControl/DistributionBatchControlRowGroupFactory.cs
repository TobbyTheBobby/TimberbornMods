using Timberborn.BatchControl;
using Timberborn.CoreUI;
using Timberborn.EntitySystem;
using UnityEngine.UIElements;

namespace ChooChoo
{
  public class DistributionBatchControlRowGroupFactory
  {
    private readonly BatchControlRowGroupFactory _batchControlRowGroupFactory;
    private readonly GoodsStationRowItemFactory _goodsStationRowItemFactory;
    private readonly DistributionSettingsRowItemFactory _distributionSettingsRowItemFactory;
    private readonly DistrictDistributionControlRowItemFactory _districtDistributionControlRowItemFactory;
    private readonly VisualElementLoader _visualElementLoader;

    public DistributionBatchControlRowGroupFactory(
      BatchControlRowGroupFactory batchControlRowGroupFactory,
      GoodsStationRowItemFactory goodsStationRowItemFactory,
      DistributionSettingsRowItemFactory distributionSettingsRowItemFactory,
      DistrictDistributionControlRowItemFactory districtDistributionControlRowItemFactory,
      VisualElementLoader visualElementLoader)
    {
      _batchControlRowGroupFactory = batchControlRowGroupFactory;
      _goodsStationRowItemFactory = goodsStationRowItemFactory;
      _distributionSettingsRowItemFactory = distributionSettingsRowItemFactory;
      _districtDistributionControlRowItemFactory = districtDistributionControlRowItemFactory;
      _visualElementLoader = visualElementLoader;
    }

    public BatchControlRowGroup Create(GoodsStation goodsStation)
    {
      VisualElement root = _visualElementLoader.LoadVisualElement("Game/BatchControl/BatchControlRow");
      GoodsStationDistributionSettings componentFast1 = goodsStation.GetComponentFast<GoodsStationDistributionSettings>();
      IBatchControlRowItem batchControlRowItem1 = _goodsStationRowItemFactory.Create(goodsStation);
      IBatchControlRowItem batchControlRowItem2 = _districtDistributionControlRowItemFactory.Create(componentFast1);
      EntityComponent componentFast2 = goodsStation.GetComponentFast<EntityComponent>();
      IBatchControlRowItem[] batchControlRowItemArray = {
        batchControlRowItem1,
        batchControlRowItem2
      };
      BatchControlRowGroup unsorted = _batchControlRowGroupFactory.CreateUnsorted(new BatchControlRow(root, componentFast2, batchControlRowItemArray));
      unsorted.AddRow(_distributionSettingsRowItemFactory.Create(componentFast1));
      return unsorted;
    }
  }
}
