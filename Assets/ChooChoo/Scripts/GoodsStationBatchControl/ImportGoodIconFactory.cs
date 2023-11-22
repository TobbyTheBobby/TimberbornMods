using System.Collections.Generic;
using Timberborn.CoreUI;
using Timberborn.Goods;
using Timberborn.GoodsUI;
using Timberborn.Localization;
using Timberborn.TooltipSystem;
using UnityEngine;
using UnityEngine.UIElements;

namespace ChooChoo
{
  public class ImportGoodIconFactory
  {
    private static readonly string DisabledInfoLocKey = "Tobbert.GoodsStation.Distribution.Disabled.Info";
    private static readonly string SendingInfoLocKey = "Tobbert.GoodsStation.Distribution.Sending.Info";
    private static readonly string ReceivingInfoLocKey = "Tobbert.GoodsStation.Distribution.Receiving.Info";
    
    private readonly GoodDescriber _goodDescriber;
    private readonly IGoodService _goodService;
    private readonly GoodsGroupSpecificationService _goodsGroupSpecificationService;
    private readonly ITooltipRegistrar _tooltipRegistrar;
    private readonly VisualElementLoader _visualElementLoader;
    private readonly ILoc _loc;

    public ImportGoodIconFactory(
      GoodDescriber goodDescriber,
      IGoodService goodService,
      GoodsGroupSpecificationService goodsGroupSpecificationService,
      ITooltipRegistrar tooltipRegistrar,
      VisualElementLoader visualElementLoader,
      ILoc loc)
    {
      _goodDescriber = goodDescriber;
      _goodService = goodService;
      _goodsGroupSpecificationService = goodsGroupSpecificationService;
      _tooltipRegistrar = tooltipRegistrar;
      _visualElementLoader = visualElementLoader;
      _loc = loc;
    }

    public IEnumerable<ImportGoodIcon> CreateImportGoods(VisualElement parent)
    {
      List<ImportGoodIcon> importGoods = new List<ImportGoodIcon>();
      foreach (GoodGroupSpecification groupSpecification in _goodsGroupSpecificationService.GoodGroupSpecifications)
        importGoods.AddRange(CreateImportGoodsGroup(parent, groupSpecification));
      return importGoods;
    }

    public ImportGoodIcon CreateImportGoodIcon(VisualElement parent, string goodId)
    {
      VisualElement root = _visualElementLoader.LoadVisualElement("Game/ImportGoodIcon");
      parent.Add(root);
      Image image = root.Q<Image>("Icon");
      DescribedGood describedGood = _goodDescriber.GetDescribedGood(goodId);
      image.sprite = describedGood.Icon;
      VisualElement receivingIcon = root.Q<VisualElement>("ImportableIcon");
      
      
      VisualElement visualElement = _visualElementLoader.LoadVisualElement("Game/ImportGoodIcon");
      VisualElement sendingIcon = visualElement.Q<VisualElement>("ImportableIcon");
      sendingIcon.transform.rotation *= Quaternion.Euler(0, 0, 180);
      receivingIcon.parent.Add(sendingIcon);
      
      VisualElement nonImportableIcon = root.Q<VisualElement>("NonImportableIcon");
      ImportGoodIcon importGoodIcon = new ImportGoodIcon(goodId, sendingIcon, receivingIcon, nonImportableIcon);
      _tooltipRegistrar.Register(image, () => GetTooltip(importGoodIcon, goodId, describedGood.DisplayName));
      return importGoodIcon;
    }

    private IEnumerable<ImportGoodIcon> CreateImportGoodsGroup(VisualElement parent, GoodGroupSpecification groupSpecification)
    {
      VisualElement visualElement = _visualElementLoader.LoadVisualElement("Game/EntityPanel/ImportGoodsGroup");
      visualElement.Q<Image>("Icon").sprite = groupSpecification.Icon;
      parent.Add(visualElement);
      VisualElement iconsParent = visualElement.Q<VisualElement>("Items");
      foreach (string goodId in _goodService.GetGoodsForGroup(groupSpecification.Id))
        yield return CreateImportGoodIcon(iconsParent, goodId);
    }

    private VisualElement GetTooltip(ImportGoodIcon importGoodIcon, string goodId, string goodDisplayName)
    {
      VisualElement e = _visualElementLoader.LoadVisualElement("Game/ImportGoodIconTooltip");
      e.Q<Label>("GoodLabel").text = goodDisplayName;
      GoodsStationDistributionSettings goodsStationDistributionSettings = importGoodIcon.GoodsStationDistributionSettings;
      DistributionOption goodDistributionOption = goodsStationDistributionSettings.GetGoodDistributionSetting(goodId).DistributionOption;
      var disabledInfo = e.Q<Label>("DisabledInfo");
      disabledInfo.ToggleDisplayStyle(goodDistributionOption == DistributionOption.Disabled);
      disabledInfo.text = _loc.T(DisabledInfoLocKey);
      var sendingInfo = e.Q<Label>("ForcedInfo");
      sendingInfo.ToggleDisplayStyle(goodDistributionOption == DistributionOption.Sending);
      sendingInfo.text = _loc.T(SendingInfoLocKey);
      var receivingInfo = e.Q<Label>("ImportableInfo");
      receivingInfo.ToggleDisplayStyle(goodDistributionOption == DistributionOption.Receiving);
      receivingInfo.text = _loc.T(ReceivingInfoLocKey);
      e.Q<VisualElement>("NonImportableInfo").ToggleDisplayStyle(false);
      return e;
    }
  }
}
