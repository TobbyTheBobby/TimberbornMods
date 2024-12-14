// using System;
// using System.Collections.Generic;
// using System.Linq;
// using HarmonyLib;
// using TimberApi.SingletonSystem;
// using Timberborn.GameFactionSystem;
// using Timberborn.Localization;
// using Timberborn.Persistence;
// using Timberborn.TutorialSystem;
// using Timberborn.TutorialSystemInitialization;
// using TobbyTools.InaccessibilityUtilitySystem;
//
// namespace TobbyTools.CustomTutorialSystem
// {
//   internal class CustomTutorialConfigurationProvider : ITutorialConfigurationProvider, IEarlyLoadableSingleton
//   {
//     private readonly CustomTutorialSpecificationDeserializer _customTutorialSpecificationDeserializer;
//     private readonly ISpecificationService _specificationService;
//     private readonly FactionService _factionService;
//     private readonly ILoc _loc;
//     
//     private readonly BuildingTutorialStepFactory _buildingTutorialStepFactory;
//     private readonly MarkTreesTutorialStepFactory _markTreesTutorialStepFactory;
//     private readonly ConnectBuildingsTutorialStepFactory _connectBuildingsTutorialStepFactory;
//     private readonly MarkPlantablesTutorialStepFactory _markPlantablesTutorialStepFactory;
//     private readonly PowerBuildingsTutorialStepFactory _powerBuildingsTutorialStepFactory;
//     private readonly SelectStockpileGoodTutorialStepFactory _selectStockpileGoodTutorialStepFactory;
//     
//     private List<CustomTutorialSpecification> _customTutorialSpecifications;
//
//     public CustomTutorialConfigurationProvider(
//       CustomTutorialSpecificationDeserializer customTutorialSpecificationDeserializer,
//       ISpecificationService specificationService,
//       FactionService factionService,
//       ILoc loc,
//       
//       BuildingTutorialStepFactory buildingTutorialStepFactory,
//       MarkTreesTutorialStepFactory markTreesTutorialStepFactory,
//       ConnectBuildingsTutorialStepFactory connectBuildingsTutorialStepFactory,
//       MarkPlantablesTutorialStepFactory markPlantablesTutorialStepFactory,
//       PowerBuildingsTutorialStepFactory powerBuildingsTutorialStepFactory,
//       SelectStockpileGoodTutorialStepFactory selectStockpileGoodTutorialStepFactory)
//     {
//       _customTutorialSpecificationDeserializer = customTutorialSpecificationDeserializer;
//       _specificationService = specificationService;
//       _factionService = factionService;
//       _loc = loc;
//       
//       _buildingTutorialStepFactory = buildingTutorialStepFactory;
//       _markTreesTutorialStepFactory = markTreesTutorialStepFactory;
//       _connectBuildingsTutorialStepFactory = connectBuildingsTutorialStepFactory;
//       _markPlantablesTutorialStepFactory = markPlantablesTutorialStepFactory;
//       _powerBuildingsTutorialStepFactory = powerBuildingsTutorialStepFactory;
//       _selectStockpileGoodTutorialStepFactory = selectStockpileGoodTutorialStepFactory;
//     }
//     
//     public void EarlyLoad()
//     {
//       _customTutorialSpecifications = _specificationService.GetSpecifications(_customTutorialSpecificationDeserializer).ToList();
//     }
//
//     public TutorialConfiguration Get()
//     {
//       var currentFaction = _factionService.Current.Id;
//       foreach (var customTutorialSpecification in _customTutorialSpecifications)
//       {
//         if (currentFaction == customTutorialSpecification.CustomTutorialStartingConditions.Faction)
//         {
//           return CreateTutorialConfiguration(customTutorialSpecification);
//         }
//       }
//       return TutorialConfiguration.CreateEmpty();
//     }
//
//     private TutorialConfiguration CreateTutorialConfiguration(CustomTutorialSpecification customTutorialSpecification)
//     {
//       var builder = CreateBuilder(customTutorialSpecification.TutorialStages[0]);
//       
//       foreach (var customTutorialStage in customTutorialSpecification.TutorialStages[1..])
//       {
//         builder.StartNextStage("IDK", _loc.T(customTutorialStage.LocKey));
//         foreach (var customTutorialStep in customTutorialStage.Steps)
//         {
//           builder.AddStep(CreateStep(customTutorialStep));
//         }
//       }
//
//       return builder.Build();
//
//       // return new
//       //   TutorialConfiguration.Builder(_loc.T("Tutorial.Stage.PlaceLumberjacks"))
//       //   .AddStep(AnyBuildingTutorialStep(2, "LumberjackFlag.Folktails"))
//       //
//       //   .StartNextStage(_loc.T("Tutorial.Stage.FinishLumberjacks"))
//       //   .AddStep(FinishedBuildingTutorialStep(2, "LumberjackFlag.Folktails"))
//       //   
//       //   .StartNextStage(_loc.T("Tutorial.Stage.ConnectLumberjacks"))
//       //   .AddStep(ConnectBuildingsTutorialStep(2, "LumberjackFlag.Folktails"))
//       //   
//       //   .StartNextStage(_loc.T("Tutorial.Stage.MarkTrees"))
//       //   .AddStep(MarkTreesTutorialStep())
//       //   
//       //   .StartNextStage(_loc.T("Tutorial.Stage.WaterPump"))
//       //   .AddStep(FinishedBuildingTutorialStep(1, "WaterPump.Folktails"))
//       //   
//       //   .StartNextStage(_loc.T("Tutorial.Stage.Gatherer"))
//       //   .AddStep(FinishedBuildingTutorialStep(1, "GathererFlag.Folktails"))
//       //   
//       //   .StartNextStage(_loc.T("Tutorial.Stage.Farmhouse"))
//       //   .AddStep(FinishedBuildingTutorialStep(1, "EfficientFarmHouse.Folktails"))
//       //   
//       //   .StartNextStage(_loc.T("Tutorial.Stage.MarkCarrots"))
//       //   .AddStep(MarkPlantablesTutorialStep(100, "Carrot"))
//       //   
//       //   .StartNextStage(_loc.T("Tutorial.Stage.BasicStorage"))
//       //   .AddStep(FinishedBuildingTutorialStep(1, "LargePile.Folktails"))
//       //   .AddStep(SelectStockpileGoodTutorialStep(1, "LargePile.Folktails", "Log"))
//       //   .AddStep(FinishedBuildingTutorialStep(3, "SmallTank.Folktails"))
//       //   .AddStep(SelectStockpileGoodTutorialStep(3, "SmallTank.Folktails", "Water"))
//       //   
//       //   .StartNextStage(_loc.T("Tutorial.Stage.ExpandStorage"))
//       //   .AddStep(FinishedBuildingTutorialStep(2, "MediumWarehouse.Folktails"))
//       //   .AddStep(SelectStockpileGoodTutorialStep(1, "MediumWarehouse.Folktails", "Carrot"))
//       //   .AddStep(SelectStockpileGoodTutorialStep(1, "MediumWarehouse.Folktails", "Berries"))
//       //   
//       //   .StartNextStage(_loc.T("Tutorial.Stage.Lodges"))
//       //   .AddStep(FinishedBuildingTutorialStep(6, "Lodge.Folktails", "FlippedLodge.Folktails"))
//       //   
//       //   .StartNextStage(_loc.T("Tutorial.Stage.Inventor"))
//       //   .AddStep(FinishedBuildingTutorialStep(1, "Inventor.Folktails"))
//       //   
//       //   .StartNextStage(_loc.T("Tutorial.Stage.WaterWheel"))
//       //   .AddStep(FinishedBuildingTutorialStep(1, "WaterWheel.Folktails"))
//       //   
//       //   .StartNextStage(_loc.T("Tutorial.Stage.LumberMill"))
//       //   .AddStep(FinishedBuildingTutorialStep(1, "LumberMill.Folktails"))
//       //   .AddStep(PowerBuildingsTutorialStep(1, "LumberMill.Folktails"))
//       //   
//       //   .StartNextStage(_loc.T("Tutorial.Stage.Forester"))
//       //   .AddStep(FinishedBuildingTutorialStep(1, "Forester.Folktails"))
//       //   
//       //   .StartNextStage(_loc.T("Tutorial.Stage.MarkPinesForPlanting"))
//       //   .AddStep(MarkPlantablesTutorialStep(20, "Pine"))
//       //   
//       //   .StartNextStage(_loc.T("Tutorial.Stage.KeepPlaying"))
//       //   .Build();
//     }
//
//     private TutorialConfiguration.Builder CreateBuilder(CustomTutorialStage firstStage)
//     {
//       var builder = new TutorialConfiguration.Builder("IDK", _loc.T(firstStage.LocKey));
//
//       foreach (var customTutorialStep in firstStage.Steps)
//       {
//         builder.AddStep(CreateStep(customTutorialStep));
//       }
//
//       return builder;
//     }
//     
//     private TutorialStep CreateStep(CustomTutorialStep customTutorialStep)
//     {
//       var settings = customTutorialStep.CustomTutorialStepSettings;
//
//       switch (customTutorialStep.StepType)
//       {
//         case "FinishedBuildingTutorialStep":
//           return FinishedBuildingTutorialStep(settings.RequiredAmount, settings.PrefabNames);
//         case "AnyBuildingTutorialStep":
//           return AnyBuildingTutorialStep(settings.RequiredAmount, settings.PrefabNames);
//         case "ConnectBuildingsTutorialStep":
//           return ConnectBuildingsTutorialStep(settings.RequiredAmount, settings.PrefabNames[0]);
//         case "MarkTreesTutorialStep":
//           return MarkTreesTutorialStep();
//         case "MarkPlantablesTutorialStep":
//           return MarkPlantablesTutorialStep(settings.RequiredAmount, settings.PrefabNames[0]);
//         case "PowerBuildingsTutorialStep":
//           return PowerBuildingsTutorialStep(settings.RequiredAmount, settings.PrefabNames[0]);
//         case "SelectStockpileGoodTutorialStep":
//           return SelectStockpileGoodTutorialStep(settings.RequiredAmount, settings.PrefabNames[0], settings.GoodId);
//         case null or "":
//           throw new ArgumentOutOfRangeException($"Found an empty StepType, make sure the StepType is set.");
//         default:
//           throw new ArgumentOutOfRangeException($"{customTutorialStep.StepType} is not a valid StepType.");
//       }
//     }
//
//     private TutorialStep FinishedBuildingTutorialStep(int requiredAmount, params string[] prefabNames)
//     {
//       return (TutorialStep)InaccessibilityUtilities.InvokeInaccessibleMethod(
//         _buildingTutorialStepFactory, 
//         "CreateRequiringFinishedBuildings",
//         new object[] { requiredAmount, prefabNames });
//     }
//
//     private TutorialStep AnyBuildingTutorialStep(int requiredAmount, params string[] prefabNames)
//     {
//       return (TutorialStep)InaccessibilityUtilities.InvokeInaccessibleMethod(
//         _buildingTutorialStepFactory, 
//         "CreateRequiringAnyBuildings",
//         new object[] { requiredAmount, prefabNames });
//     }
//
//     private TutorialStep ConnectBuildingsTutorialStep(int requiredAmount, string prefabName)
//     {
//       return (TutorialStep)InaccessibilityUtilities.InvokeInaccessibleMethod(
//         _connectBuildingsTutorialStepFactory, 
//         "Create",
//         new object[] { requiredAmount, prefabName });
//     }
//
//     private TutorialStep MarkTreesTutorialStep()
//     {
//       return (TutorialStep)InaccessibilityUtilities.InvokeInaccessibleMethod(
//         _markTreesTutorialStepFactory, 
//         "Create");
//     }
//
//     private TutorialStep MarkPlantablesTutorialStep(int requiredAmount, string prefabName)
//     {
//       return (TutorialStep)InaccessibilityUtilities.InvokeInaccessibleMethod(
//         _markPlantablesTutorialStepFactory, 
//         "Create",
//         new object[] { requiredAmount, prefabName });
//     }
//
//     private TutorialStep PowerBuildingsTutorialStep(int requiredAmount, string prefabName)
//     {
//       return (TutorialStep)InaccessibilityUtilities.InvokeInaccessibleMethod(
//         _powerBuildingsTutorialStepFactory, 
//         "Create",
//         new object[] { requiredAmount, prefabName });
//     }
//
//     private TutorialStep SelectStockpileGoodTutorialStep(
//       int requiredAmount,
//       string prefabName,
//       string goodId)
//     {
//       return (TutorialStep)InaccessibilityUtilities.InvokeInaccessibleMethod(
//         _selectStockpileGoodTutorialStepFactory, 
//         "Create",
//         new object[] { prefabName, requiredAmount, goodId });
//     }
//   }
// }