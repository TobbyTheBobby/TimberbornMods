// using Timberborn.BatchControl;
// using Timberborn.BuildingsUI;
// using Timberborn.CoreUI;
// using Timberborn.MechanicalSystemUI;
// using Timberborn.StatusSystemUI;
// using UnityEngine;
//
// namespace ChooChoo
// {
//   internal class MechanicalBatchControlRowFactory
//   {
//     private readonly VisualElementLoader _visualElementLoader;
//     private readonly CurrentPopulationBatchControlRowItemFactory _currentPopulationBatchControlRowItemFactory;
//     private readonly BuildingBatchControlRowItemFactory _buildingBatchControlRowItemFactory;
//     private readonly StatusBatchControlRowItemFactory _statusBatchControlRowItemFactory;
//     private readonly TrainBatchControlRowItemFactory _trainBatchControlRowItemFactory;
//     private readonly BatteryBatchControlRowItemFactory _batteryBatchControlRowItemFactory;
//
//     public MechanicalBatchControlRowFactory(
//       VisualElementLoader visualElementLoader,
//       CurrentPopulationBatchControlRowItemFactory currentPopulationBatchControlRowItemFactory,
//       BuildingBatchControlRowItemFactory buildingBatchControlRowItemFactory,
//       StatusBatchControlRowItemFactory statusBatchControlRowItemFactory,
//       TrainBatchControlRowItemFactory trainBatchControlRowItemFactory
//       )
//     {
//       _visualElementLoader = visualElementLoader;
//       _currentPopulationBatchControlRowItemFactory = currentPopulationBatchControlRowItemFactory;
//       _buildingBatchControlRowItemFactory = buildingBatchControlRowItemFactory;
//       _statusBatchControlRowItemFactory = statusBatchControlRowItemFactory;
//       _trainBatchControlRowItemFactory = trainBatchControlRowItemFactory;
//     }
//
//     public BatchControlRow Create(GameObject entity) => new(_visualElementLoader.LoadVisualElement("Master/BatchControl/BatchControlRow"), entity, new[]
//     {
//       _currentPopulationBatchControlRowItemFactory.Create("population-counter__icon--adult"),
//       // _buildingBatchControlRowItemFactory.Create(entity),
//       _trainBatchControlRowItemFactory.Create(entity),
//       _statusBatchControlRowItemFactory.Create(entity)
//     });
//
//     public BatchControlRow Create(int mechanicalGraph) => new BatchControlRow(_visualElementLoader.LoadVisualElement("Master/BatchControl/BatchControlHeaderRow"), new []
//     {
//       _trainBatchControlRowItemFactory.Create(mechanicalGraph)
//     });
//   }
// }
