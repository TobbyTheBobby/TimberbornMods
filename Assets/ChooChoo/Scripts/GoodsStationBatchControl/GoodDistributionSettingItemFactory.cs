using System.Linq;
using Timberborn.CoreUI;
using Timberborn.InputSystem;
using Timberborn.Localization;
using Timberborn.SliderToggleSystem;
using Timberborn.TooltipSystem;
using UnityEngine.UIElements;
using Button = UnityEngine.UIElements.Button;
using ProgressBar = Timberborn.CoreUI.ProgressBar;
using Slider = UnityEngine.UIElements.Slider;
using Toggle = UnityEngine.UIElements.Toggle;

namespace ChooChoo
{
  public class GoodDistributionSettingItemFactory
  {
    private static readonly string DecreaseMaximumLocKey = "Tobbert.BatchControl.DecreaseMaximum";
    private static readonly string DecreaseMaximumDescriptionLocKey = "Tobbert.BatchControl.DecreaseMaximum.Description";
    private static readonly string IncreaseMaximumLocKey = "Tobbert.BatchControl.IncreaseMaximum";
    private static readonly string IncreaseMaximumDescriptionLocKey = "Tobbert.BatchControl.IncreaseMaximum.Description";
    
    private readonly AlternateClickableFactory _alternateClickableFactory;
    private readonly ImportGoodIconFactory _importGoodIconFactory;
    private readonly ImportToggleFactory _importToggleFactory;
    private readonly ITooltipRegistrar _tooltipRegistrar;
    private readonly VisualElementLoader _visualElementLoader;
    private readonly InputService _inputService;
    private readonly ILoc _loc;

    public GoodDistributionSettingItemFactory(
      AlternateClickableFactory alternateClickableFactory,
      ImportGoodIconFactory importGoodIconFactory,
      ImportToggleFactory importToggleFactory,
      ITooltipRegistrar tooltipRegistrar,
      VisualElementLoader visualElementLoader,
      InputService inputService,
      ILoc loc)
    {
      _alternateClickableFactory = alternateClickableFactory;
      _importGoodIconFactory = importGoodIconFactory;
      _importToggleFactory = importToggleFactory;
      _tooltipRegistrar = tooltipRegistrar;
      _visualElementLoader = visualElementLoader;
      _inputService = inputService;
      _loc = loc;
    }

    public GoodDistributionSettingItem Create(GoodsStationDistributionSettings goodsStationDistributionSettings, GoodsStationGoodDistributionSetting goodsStationGoodDistributionSetting)
    {
      VisualElement visualElement = _visualElementLoader.LoadVisualElement("Game/BatchControl/GoodDistributionSettingItem");
      ImportGoodIcon importGoodIcon = _importGoodIconFactory.CreateImportGoodIcon(visualElement.Q<VisualElement>("ImportGoodIconWrapper"), goodsStationGoodDistributionSetting.GoodId);
      importGoodIcon.SetGoodsStationDistributionSettings(goodsStationDistributionSettings);
      var slider = visualElement.Q<Slider>("ExportThresholdSlider");
      slider.parent.ToggleDisplayStyle(false);
      
      
      VisualElement visualElement1 = _visualElementLoader.LoadVisualElement("Game/BatchControl/PopulationDistributorBatchControlRowItem");
      IntegerField textField = visualElement1.Q<IntegerField>("MinimumValue");
      textField.style.maxWidth = new Length(40, LengthUnit.Percent);
      var plusButton = visualElement1.Q<Button>("PlusButton");
      var minusButton = visualElement1.Q<Button>("MinusButton");
      InitializeMinimumControls(textField, plusButton, minusButton, visualElement1, goodsStationGoodDistributionSetting);
      var buttonsParent = textField.parent;
      var minimum = buttonsParent.Children().First();
      minimum.ToggleDisplayStyle(false);
      var warning = buttonsParent.Children().Last();
      warning.ToggleDisplayStyle(false);
      slider.parent.parent.Add(buttonsParent);
      
      
      SliderToggle importToggle = _importToggleFactory.Create(visualElement.Q<VisualElement>("ImportToggleWrapper"), goodsStationGoodDistributionSetting);
      visualElement.Q<ProgressBar>("FillRateProgressBar").ToggleDisplayStyle(false);
      var goodDistributionSettingItem = new GoodDistributionSettingItem(_inputService, visualElement, goodsStationGoodDistributionSetting, importGoodIcon, importToggle, textField, plusButton, minusButton);
      goodDistributionSettingItem.Initialize();
      return goodDistributionSettingItem;
    }

    private void InitializeMinimumControls(
      IntegerField minimumValue, 
      Button plusButton, 
      Button minusButton, 
      VisualElement root, 
      GoodsStationGoodDistributionSetting goodsStationGoodDistributionSetting)
    {
      TextFields.InitializeIntegerField(minimumValue, goodsStationGoodDistributionSetting.MaxCapacity, afterEditingCallback: newValue => OnIntFieldChange(newValue, goodsStationGoodDistributionSetting));
      _alternateClickableFactory.Create(
        minusButton, () => OnButtonClicked(-1, goodsStationGoodDistributionSetting), 
        () => OnButtonClicked(-10, goodsStationGoodDistributionSetting));   
      _tooltipRegistrar.Register(minusButton, GetMinusButtonTooltip());
      _alternateClickableFactory.Create(
        plusButton, 
        () => OnButtonClicked(1, goodsStationGoodDistributionSetting), 
        () => OnButtonClicked(10, goodsStationGoodDistributionSetting));      
      _tooltipRegistrar.Register(plusButton, GetPlusButtonTooltip());
      Toggle immigrationToggle = root.Q<Toggle>("ImmigrationToggle");
      immigrationToggle.ToggleDisplayStyle(false);
      Toggle emigrationToggle = root.Q<Toggle>("EmigrationToggle");
      emigrationToggle.ToggleDisplayStyle(false);
    }

    private void OnIntFieldChange(int newValue, GoodsStationGoodDistributionSetting goodsStationGoodDistributionSetting)
    {
      if (newValue > GoodsStation.Capacity)
        newValue = GoodsStation.Capacity;
      goodsStationGoodDistributionSetting.SetMaxCapacity(newValue);
    }

    private void OnButtonClicked(int change, GoodsStationGoodDistributionSetting goodsStationGoodDistributionSetting)
    {
      int minimum = goodsStationGoodDistributionSetting.MaxCapacity + change;
      if (minimum > GoodsStation.Capacity)
        minimum = GoodsStation.Capacity;
      goodsStationGoodDistributionSetting.SetMaxCapacity(minimum);
    }
    
    private VisualElement GetMinusButtonTooltip() => GetTooltip(DecreaseMaximumLocKey, DecreaseMaximumDescriptionLocKey);

    private VisualElement GetPlusButtonTooltip() => GetTooltip(IncreaseMaximumLocKey, IncreaseMaximumDescriptionLocKey);

    private VisualElement GetTooltip(string title, string description)
    {
      VisualElement e = _visualElementLoader.LoadVisualElement("Game/ImportToggleTooltip");
      e.Q<Label>("Title").text = _loc.T(title);
      e.Q<Label>("Description").text = _loc.T(description);
      return e;
    }
  }
}
