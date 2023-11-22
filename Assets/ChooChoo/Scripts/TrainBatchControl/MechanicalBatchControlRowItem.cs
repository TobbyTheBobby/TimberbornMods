using Timberborn.BatchControl;
using Timberborn.Localization;
using UnityEngine;
using UnityEngine.UIElements;

namespace ChooChoo
{
  internal class MechanicalBatchControlRowItem : IBatchControlRowItem, IUpdateableBatchControlRowItem
  {
    private readonly ILoc _loc;
    private readonly Label _label;
    private readonly GameObject _train;

    public VisualElement Root { get; }

    public MechanicalBatchControlRowItem(
      ILoc loc,
      VisualElement root,
      Label label,
      GameObject train)
    {
      Root = root;
      _loc = loc;
      _label = label;
      _train = train;
    }

    public void UpdateRowItem()
    {
      _label.text = MechanicalNodeTextFormatter.FormatGeneratorText(_loc, _train);
    }
  }
}
