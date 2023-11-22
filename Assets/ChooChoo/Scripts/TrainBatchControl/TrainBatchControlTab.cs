// using System.Collections.Generic;
// using System.Linq;
// using Timberborn.BatchControl;
// using Timberborn.CoreUI;
// using Timberborn.Common;
// using Timberborn.MechanicalSystem;
// using Timberborn.SingletonSystem;
// using UnityEngine;
//
// namespace ChooChoo
// {
//   internal class TrainBatchControlTab : BatchControlTab, ILoadableSingleton
//   {
//     private readonly MechanicalBatchControlRowFactory _mechanicalBatchControlRowFactory;
//     private readonly EventBus _eventBus;
//     private readonly Dictionary<int, List<GameObject>> _trains = new();
//
//     public TrainBatchControlTab(
//       VisualElementLoader visualElementLoader,
//       BatchControlDistrict batchControlDistrict,
//       MechanicalBatchControlRowFactory mechanicalBatchControlRowFactory,
//       EventBus eventBus)
//       : base(visualElementLoader, batchControlDistrict)
//     {
//       _mechanicalBatchControlRowFactory = mechanicalBatchControlRowFactory;
//       _eventBus = eventBus;
//     }
//
//     public override string TabNameLocKey => "Tobbert.BatchControl.Trains";
//
//     public override string TabImage => "Mechanical";
//
//     public override bool IgnoreDistrictSelection => true;
//
//     [OnEvent]
//     public void OnMechanicalGraphCreated(
//       MechanicalGraphCreatedEvent mechanicalGraphCreatedEvent)
//     {
//       HideAndMarkForRefresh();
//     }
//
//     [OnEvent]
//     public void OnMechanicalGraphRemoved(
//       MechanicalGraphRemovedEvent mechanicalGraphRemovedEvent)
//     {
//       HideAndMarkForRefresh();
//     }
//
//     public void Load() => _eventBus.Register(this);
//
//     protected override IEnumerable<BatchControlRowGroup> GetRows(IEnumerable<GameObject> gameObjects)
//     {
//       GatherTrains(gameObjects.Where(gameObject =>
//       {
//         TrainWagonManager component = gameObject.GetComponent<TrainWagonManager>();
//         return component != null && component.enabled;
//       }).Select(gameObject => gameObject.GetComponent<TrainWagonManager>()));
//       return GetRows();
//     }
//
//     private void HideAndMarkForRefresh()
//     {
//       HideContent();
//       IsDirty = true;
//     }
//     
//     private void GatherTrains(IEnumerable<TrainWagonManager> trains)
//     {
//       foreach (TrainWagonManager trainWagonManager in trains)
//         _trains.GetOrAdd(trainWagonManager.NumberOfWagons).Add(trainWagonManager.gameObject);
//     }
//
//     private IEnumerable<BatchControlRowGroup> GetRows()
//     {
//       foreach (int key in _trains.Keys)
//       {
//         BatchControlRowGroup batchControlRowGroup = BatchControlRowGroup.Create(_mechanicalBatchControlRowFactory.Create(key));
//         foreach (GameObject entity in _trains[key])
//           batchControlRowGroup.AddRow(_mechanicalBatchControlRowFactory.Create(entity));
//         yield return batchControlRowGroup;
//       }
//       _trains.Clear();
//     }
//   }
// }
