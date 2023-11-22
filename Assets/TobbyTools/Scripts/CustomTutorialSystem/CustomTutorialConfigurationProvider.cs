using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using TimberApi.Common.SingletonSystem;
using TimberApi.DependencyContainerSystem;
using Timberborn.GameFactionSystem;
using Timberborn.Localization;
using Timberborn.Persistence;
using Timberborn.SingletonSystem;
using Timberborn.TutorialSystem;
using TobbyTools.InaccessibilityUtilitySystem;

namespace TobbyTools.CustomTutorialSystem
{
  internal class CustomTutorialConfigurationProvider : ITutorialConfigurationProvider, IEarlyLoadableSingleton
  {
    private readonly CustomTutorialSpecificationDeserializer _customTutorialSpecificationDeserializer;
    private readonly ISpecificationService _specificationService;
    private readonly FactionService _factionService;
    private readonly ILoc _loc;
    private object _buildingTutorialStepFactory;
    private object _markTreesTutorialStepFactory;
    private object _connectBuildingsTutorialStepFactory;
    private object _markPlantablesTutorialStepFactory;
    private object _powerBuildingsTutorialStepFactory;
    private object _selectStockpileGoodTutorialStepFactory;
    private List<CustomTutorialSpecification> _customTutorialSpecifications;

    public CustomTutorialConfigurationProvider(
      CustomTutorialSpecificationDeserializer customTutorialSpecificationDeserializer,
      ISpecificationService specificationService,
      FactionService factionService,
      ILoc loc)
    {
      _customTutorialSpecificationDeserializer = customTutorialSpecificationDeserializer;
      _specificationService = specificationService;
      _factionService = factionService;
      _loc = loc;
    }
    
    public void EarlyLoad()
    {
      _buildingTutorialStepFactory = DependencyContainer.GetInstance(AccessTools.TypeByName("BuildingTutorialStepFactory"));
      _markTreesTutorialStepFactory = DependencyContainer.GetInstance(AccessTools.TypeByName("MarkTreesTutorialStepFactory"));
      _connectBuildingsTutorialStepFactory = DependencyContainer.GetInstance(AccessTools.TypeByName("ConnectBuildingsTutorialStepFactory"));
      _markPlantablesTutorialStepFactory = DependencyContainer.GetInstance(AccessTools.TypeByName("MarkPlantablesTutorialStepFactory"));
      _powerBuildingsTutorialStepFactory = DependencyContainer.GetInstance(AccessTools.TypeByName("PowerBuildingsTutorialStepFactory"));
      _selectStockpileGoodTutorialStepFactory = DependencyContainer.GetInstance(AccessTools.TypeByName("SelectStockpileGoodTutorialStepFactory"));
      _customTutorialSpecifications = _specificationService.GetSpecifications(_customTutorialSpecificationDeserializer).ToList();
    }

    public TutorialConfiguration Get()
    {
      var currentFaction = _factionService.Current.Id;
      foreach (var customTutorialSpecification in _customTutorialSpecifications)
      {
        if (currentFaction == customTutorialSpecification.CustomTutorialStartingConditions.Faction)
        {
          return CreateTutorialConfiguration(customTutorialSpecification);
        }
      }
      return TutorialConfiguration.CreateEmpty();
    }

    private TutorialConfiguration CreateTutorialConfiguration(CustomTutorialSpecification customTutorialSpecification)
    {
      var builder = CreateBuilder(customTutorialSpecification.TutorialStages[0]);
      
      foreach (var customTutorialStage in customTutorialSpecification.TutorialStages[1..])
      {
        builder.StartNextStage(_loc.T(customTutorialStage.LocKey));
        foreach (var customTutorialStep in customTutorialStage.Steps)
        {
          builder.AddStep(CreateStep(customTutorialStep));
        }
      }

      return builder.Build();

      // return new
      //   TutorialConfiguration.Builder(_loc.T("Tutorial.Stage.PlaceLumberjacks"))
      //   .AddStep(AnyBuildingTutorialStep(2, "LumberjackFlag.Folktails"))
      //
      //   .StartNextStage(_loc.T("Tutorial.Stage.FinishLumberjacks"))
      //   .AddStep(FinishedBuildingTutorialStep(2, "LumberjackFlag.Folktails"))
      //   
      //   .StartNextStage(_loc.T("Tutorial.Stage.ConnectLumberjacks"))
      //   .AddStep(ConnectBuildingsTutorialStep(2, "LumberjackFlag.Folktails"))
      //   
      //   .StartNextStage(_loc.T("Tutorial.Stage.MarkTrees"))
      //   .AddStep(MarkTreesTutorialStep())
      //   
      //   .StartNextStage(_loc.T("Tutorial.Stage.WaterPump"))
      //   .AddStep(FinishedBuildingTutorialStep(1, "WaterPump.Folktails"))
      //   
      //   .StartNextStage(_loc.T("Tutorial.Stage.Gatherer"))
      //   .AddStep(FinishedBuildingTutorialStep(1, "GathererFlag.Folktails"))
      //   
      //   .StartNextStage(_loc.T("Tutorial.Stage.Farmhouse"))
      //   .AddStep(FinishedBuildingTutorialStep(1, "EfficientFarmHouse.Folktails"))
      //   
      //   .StartNextStage(_loc.T("Tutorial.Stage.MarkCarrots"))
      //   .AddStep(MarkPlantablesTutorialStep(100, "Carrot"))
      //   
      //   .StartNextStage(_loc.T("Tutorial.Stage.BasicStorage"))
      //   .AddStep(FinishedBuildingTutorialStep(1, "LargePile.Folktails"))
      //   .AddStep(SelectStockpileGoodTutorialStep(1, "LargePile.Folktails", "Log"))
      //   .AddStep(FinishedBuildingTutorialStep(3, "SmallTank.Folktails"))
      //   .AddStep(SelectStockpileGoodTutorialStep(3, "SmallTank.Folktails", "Water"))
      //   
      //   .StartNextStage(_loc.T("Tutorial.Stage.ExpandStorage"))
      //   .AddStep(FinishedBuildingTutorialStep(2, "MediumWarehouse.Folktails"))
      //   .AddStep(SelectStockpileGoodTutorialStep(1, "MediumWarehouse.Folktails", "Carrot"))
      //   .AddStep(SelectStockpileGoodTutorialStep(1, "MediumWarehouse.Folktails", "Berries"))
      //   
      //   .StartNextStage(_loc.T("Tutorial.Stage.Lodges"))
      //   .AddStep(FinishedBuildingTutorialStep(6, "Lodge.Folktails", "FlippedLodge.Folktails"))
      //   
      //   .StartNextStage(_loc.T("Tutorial.Stage.Inventor"))
      //   .AddStep(FinishedBuildingTutorialStep(1, "Inventor.Folktails"))
      //   
      //   .StartNextStage(_loc.T("Tutorial.Stage.WaterWheel"))
      //   .AddStep(FinishedBuildingTutorialStep(1, "WaterWheel.Folktails"))
      //   
      //   .StartNextStage(_loc.T("Tutorial.Stage.LumberMill"))
      //   .AddStep(FinishedBuildingTutorialStep(1, "LumberMill.Folktails"))
      //   .AddStep(PowerBuildingsTutorialStep(1, "LumberMill.Folktails"))
      //   
      //   .StartNextStage(_loc.T("Tutorial.Stage.Forester"))
      //   .AddStep(FinishedBuildingTutorialStep(1, "Forester.Folktails"))
      //   
      //   .StartNextStage(_loc.T("Tutorial.Stage.MarkPinesForPlanting"))
      //   .AddStep(MarkPlantablesTutorialStep(20, "Pine"))
      //   
      //   .StartNextStage(_loc.T("Tutorial.Stage.KeepPlaying"))
      //   .Build();
    }

    private TutorialConfiguration.Builder CreateBuilder(CustomTutorialStage firstStage)
    {
      var builder = new TutorialConfiguration.Builder(_loc.T(firstStage.LocKey));

      foreach (var customTutorialStep in firstStage.Steps)
      {
        builder.AddStep(CreateStep(customTutorialStep));
      }

      return builder;
    }
    
    private TutorialStep CreateStep(CustomTutorialStep customTutorialStep)
    {
      var settings = customTutorialStep.CustomTutorialStepSettings;

      switch (customTutorialStep.StepType)
      {
        case "FinishedBuildingTutorialStep":
          return FinishedBuildingTutorialStep(settings.RequiredAmount, settings.PrefabNames);
        case "AnyBuildingTutorialStep":
          return AnyBuildingTutorialStep(settings.RequiredAmount, settings.PrefabNames);
        case "ConnectBuildingsTutorialStep":
          return ConnectBuildingsTutorialStep(settings.RequiredAmount, settings.PrefabNames[0]);
        case "MarkTreesTutorialStep":
          return MarkTreesTutorialStep();
        case "MarkPlantablesTutorialStep":
          return MarkPlantablesTutorialStep(settings.RequiredAmount, settings.PrefabNames[0]);
        case "PowerBuildingsTutorialStep":
          return PowerBuildingsTutorialStep(settings.RequiredAmount, settings.PrefabNames[0]);
        case "SelectStockpileGoodTutorialStep":
          return SelectStockpileGoodTutorialStep(settings.RequiredAmount, settings.PrefabNames[0], settings.GoodId);
        case null or "":
          throw new ArgumentOutOfRangeException($"Found an empty StepType, make sure the StepType is set.");
        default:
          throw new ArgumentOutOfRangeException($"{customTutorialStep.StepType} is not a valid StepType.");
      }
    }

    private TutorialStep FinishedBuildingTutorialStep(int requiredAmount, params string[] prefabNames)
    {
      return (TutorialStep)InaccessibilityUtilities.InvokeInaccesableMethod(
        _buildingTutorialStepFactory, 
        "CreateRequiringFinishedBuildings",
        new object[] { requiredAmount, prefabNames });
    }

    private TutorialStep AnyBuildingTutorialStep(int requiredAmount, params string[] prefabNames)
    {
      return (TutorialStep)InaccessibilityUtilities.InvokeInaccesableMethod(
        _buildingTutorialStepFactory, 
        "CreateRequiringAnyBuildings",
        new object[] { requiredAmount, prefabNames });
    }

    private TutorialStep ConnectBuildingsTutorialStep(int requiredAmount, string prefabName)
    {
      return (TutorialStep)InaccessibilityUtilities.InvokeInaccesableMethod(
        _connectBuildingsTutorialStepFactory, 
        "Create",
        new object[] { requiredAmount, prefabName });
    }

    private TutorialStep MarkTreesTutorialStep()
    {
      return (TutorialStep)InaccessibilityUtilities.InvokeInaccesableMethod(
        _markTreesTutorialStepFactory, 
        "Create");
    }

    private TutorialStep MarkPlantablesTutorialStep(int requiredAmount, string prefabName)
    {
      return (TutorialStep)InaccessibilityUtilities.InvokeInaccesableMethod(
        _markPlantablesTutorialStepFactory, 
        "Create",
        new object[] { requiredAmount, prefabName });
    }

    private TutorialStep PowerBuildingsTutorialStep(int requiredAmount, string prefabName)
    {
      return (TutorialStep)InaccessibilityUtilities.InvokeInaccesableMethod(
        _powerBuildingsTutorialStepFactory, 
        "Create",
        new object[] { requiredAmount, prefabName });
    }

    private TutorialStep SelectStockpileGoodTutorialStep(
      int requiredAmount,
      string prefabName,
      string goodId)
    {
      return (TutorialStep)InaccessibilityUtilities.InvokeInaccesableMethod(
        _selectStockpileGoodTutorialStepFactory, 
        "Create",
        new object[] { prefabName, requiredAmount, goodId });
    }
  }
}