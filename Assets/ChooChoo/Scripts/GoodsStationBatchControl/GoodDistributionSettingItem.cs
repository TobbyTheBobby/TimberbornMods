using Timberborn.InputSystem;
using Timberborn.SliderToggleSystem;
using UnityEngine.UIElements;
using ProgressBar = Timberborn.CoreUI.ProgressBar;

namespace ChooChoo
{
  public class GoodDistributionSettingItem : IInputProcessor
  {
    private readonly InputService _inputService;
    private readonly GoodsStationGoodDistributionSetting _setting;
    private readonly ImportGoodIcon _importGoodIcon;
    private readonly SliderToggle _importToggle;
    private readonly ProgressBar _fillRateProgressBar;
    private readonly IntegerField _minimum;
    private readonly Button _increaseMaxCapacity;
    private readonly Button _decreaseMaxCapacity;

    public VisualElement Root { get; }

    public GoodDistributionSettingItem(
      InputService inputService,
      VisualElement root,
      GoodsStationGoodDistributionSetting setting,
      ImportGoodIcon importGoodIcon,
      SliderToggle importToggle,
      IntegerField minimum,
      Button increaseMaxCapacity,
      Button decreaseMaxCapacity)
    {
      _inputService = inputService;
      Root = root;
      _setting = setting;
      _importGoodIcon = importGoodIcon;
      _importToggle = importToggle;
      _minimum = minimum;
      _increaseMaxCapacity = increaseMaxCapacity;
      _decreaseMaxCapacity = decreaseMaxCapacity;
    }
    
    public void Initialize()
    {
      _inputService.AddInputProcessor(this);
    }

    public void Update()
    {
      if (!IsFocused)
        _minimum.SetValueWithoutNotify(_setting.MaxCapacity);
      _minimum.SetEnabled(_setting.DistributionOption is DistributionOption.Sending or DistributionOption.Receiving);
      _decreaseMaxCapacity.SetEnabled(_setting.DistributionOption is DistributionOption.Sending or DistributionOption.Receiving && _setting.MaxCapacity > 0);
      // _increaseMaxCapacity.SetEnabled(_setting.DistributionOption == DistributionOption.Forced && _setting.MaxCapacity < GoodsStation.Capacity);
      _increaseMaxCapacity.SetEnabled(_setting.DistributionOption is DistributionOption.Sending or DistributionOption.Receiving);
      _importGoodIcon.Update();
      _importToggle.Update();
    }
    
    public bool ProcessInput()
    {
      return IsFocused;
    }

    public void Clear()
    {
      _inputService.RemoveInputProcessor(this);
      _importGoodIcon.Clear();
    }
    
    private bool IsFocused => _minimum.focusController?.focusedElement == _minimum;
  }
}
