using Timberborn.BatchControl;
using Timberborn.CoreUI;
using Timberborn.PrefabSystem;
using Timberborn.SelectionSystem;
using UnityEngine.UIElements;

namespace ChooChoo
{
  public class GoodsStationRowItemFactory
  {
    private readonly EntitySelectionService _entitySelectionService;
    private readonly VisualElementLoader _visualElementLoader;

    public GoodsStationRowItemFactory(EntitySelectionService entitySelectionService, VisualElementLoader visualElementLoader)
    {
      _entitySelectionService = entitySelectionService;
      _visualElementLoader = visualElementLoader;
    }

    public IBatchControlRowItem Create(GoodsStation goodsStation)
    {
      VisualElement visualElement = _visualElementLoader.LoadVisualElement("Game/BatchControl/DistrictCenterRowItem");
      LabeledPrefab componentFast = goodsStation.GetComponentFast<LabeledPrefab>();
      visualElement.Q<Image>("Image").sprite = componentFast.Image;
      visualElement.Q<Button>("Select").RegisterCallback((EventCallback<ClickEvent>) (_ => _entitySelectionService.SelectAndFocusOn(goodsStation)));
      return new GoodsStationRowItem(visualElement, goodsStation, visualElement.Q<Label>("Text"));
    }
  }
}
