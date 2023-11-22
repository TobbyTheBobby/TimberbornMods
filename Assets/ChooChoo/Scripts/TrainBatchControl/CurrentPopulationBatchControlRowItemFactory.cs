using Timberborn.BatchControl;
using Timberborn.CoreUI;
using UnityEngine.UIElements;

namespace ChooChoo
{
  internal class CurrentPopulationBatchControlRowItemFactory
  {
    private readonly VisualElementLoader _visualElementLoader;

    public CurrentPopulationBatchControlRowItemFactory(VisualElementLoader visualElementLoader) => _visualElementLoader = visualElementLoader;

    public IBatchControlRowItem Create(string iconClass)
    {
      VisualElement visualElement = _visualElementLoader.LoadVisualElement("Game/BatchControl/PopulationBatchControlRowItem");
      visualElement.Q<VisualElement>("PopulationIcon").AddToClassList(iconClass);
      return new CurrentPopulationBatchControlRowItem(visualElement, visualElement.Q<Label>("CurrentPopulation"));
    }
  }
}
