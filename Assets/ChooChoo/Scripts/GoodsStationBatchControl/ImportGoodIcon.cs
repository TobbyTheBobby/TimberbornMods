using Timberborn.CoreUI;
using UnityEngine.UIElements;

namespace ChooChoo
{
  public class ImportGoodIcon
  {
    private readonly string _goodId;
    private readonly VisualElement _sendingIcon;
    private readonly VisualElement _receivingIcon;
    private readonly VisualElement _nonImportableIcon;

    public GoodsStationDistributionSettings GoodsStationDistributionSettings { get; private set; }

    public ImportGoodIcon(string goodId, VisualElement sendingIcon, VisualElement receivingIcon, VisualElement nonImportableIcon)
    {
      _goodId = goodId;
      _sendingIcon = sendingIcon;
      _receivingIcon = receivingIcon;
      _nonImportableIcon = nonImportableIcon;
    }

    public void SetGoodsStationDistributionSettings(GoodsStationDistributionSettings goodsStationDistributionSettings)
    {
      GoodsStationDistributionSettings = goodsStationDistributionSettings;
    }

    public void Update()
    {
      var goodImportOption = GoodsStationDistributionSettings.GetGoodDistributionSetting(_goodId).DistributionOption;
      // Plugin.Log.LogInfo("ImportGoodIcon: goodImportOption = " + goodImportOption);
      _sendingIcon.ToggleDisplayStyle(goodImportOption == DistributionOption.Sending);
      _receivingIcon.ToggleDisplayStyle(goodImportOption == DistributionOption.Receiving);
      _nonImportableIcon.ToggleDisplayStyle(goodImportOption == DistributionOption.Disabled);
    }

    public void Clear()
    {
      GoodsStationDistributionSettings = null;
    }
  }
}
