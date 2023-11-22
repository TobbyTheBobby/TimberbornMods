using Timberborn.BatchControl;
using Timberborn.CoreUI;
using Timberborn.Localization;
using UnityEngine;
using UnityEngine.UIElements;

namespace ChooChoo
{
  public class TrainBatchControlRowItemFactory
  {
    private readonly VisualElementLoader _visualElementLoader;
    private readonly ILoc _loc;

    public TrainBatchControlRowItemFactory(VisualElementLoader visualElementLoader, ILoc loc)
    {
      _visualElementLoader = visualElementLoader;
      _loc = loc;
    }

    public IBatchControlRowItem Create(GameObject entity)
    {
      // TrainWagonManager component = entity.GetComponent<TrainWagonManager>();
      // if (component == null || !component.enabled)
      //   return null;
      VisualElement visualElement = _visualElementLoader.LoadVisualElement("Game/BatchControl/MechanicalBatchControlRowItem");
      Label label = visualElement.Q<Label>("MechanicalBatchControlRowItem", (string) null);
      return new MechanicalBatchControlRowItem(_loc, visualElement, label, entity);
    }

    public IBatchControlRowItem Create(int mechanicalGraph)
    {
      VisualElement visualElement = _visualElementLoader.LoadVisualElement("Game/BatchControl/MechanicalHeaderBatchControlRowItem");
      Label label = visualElement.Q<Label>("MechanicalHeaderBatchControlRowItem", (string) null);
      return new MechanicalHeaderBatchControlRowItem(_loc, visualElement, label, mechanicalGraph);
    }
  }
}
