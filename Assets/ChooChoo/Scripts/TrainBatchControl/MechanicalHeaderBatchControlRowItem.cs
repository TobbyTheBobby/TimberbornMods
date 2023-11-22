using Timberborn.BatchControl;
using Timberborn.Localization;
using UnityEngine.UIElements;

namespace ChooChoo
{
  internal class MechanicalHeaderBatchControlRowItem : 
    IBatchControlRowItem,
    IUpdateableBatchControlRowItem
  {
    private static readonly string TrainLengthLocKey = "Tobbert.Train.Length";
    private readonly ILoc _loc;
    private readonly Label _label;
    private readonly int _trainLength;

    public VisualElement Root { get; }

    public MechanicalHeaderBatchControlRowItem(
      ILoc loc,
      VisualElement root,
      Label label,
      int trainLength)
    {
      Root = root;
      _loc = loc;
      _label = label;
      _trainLength = trainLength;
    }

    public void UpdateRowItem()
    {
      int trainLength = _trainLength;
      string text = _loc.T(TrainLengthLocKey);
      _label.text = text + trainLength;
    }
  }
}
