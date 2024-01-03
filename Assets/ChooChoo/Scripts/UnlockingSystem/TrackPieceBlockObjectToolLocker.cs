using System;
using System.Linq;
using Timberborn.BlockObjectTools;
using Timberborn.Buildings;
using Timberborn.CoreUI;
using Timberborn.Localization;
using Timberborn.PrefabSystem;
using Timberborn.ScienceSystem;
using Timberborn.ToolSystem;

namespace ChooChoo
{
  public class TrackPieceBlockObjectToolLocker : IToolLocker
  {
    private static readonly string UnlockPromptLocKey = "Tobbert.Trains.UnlockPrompt";
    private readonly ILoc _loc;
    private readonly DialogBoxShower _dialogBoxShower;
    private readonly BuildingUnlockingService _buildingUnlockingService;

    private Building _trainYard;

    public TrackPieceBlockObjectToolLocker(
      ILoc loc,
      DialogBoxShower dialogBoxShower,
      BuildingUnlockingService buildingUnlockingService)
    {
      _loc = loc;
      _dialogBoxShower = dialogBoxShower;
      _buildingUnlockingService = buildingUnlockingService;
    }

    public bool ShouldLock(Tool tool)
    {
      var isBuilding = TryGetBuildingFromTool(tool, out var building);

      var isTrainYard = false;

      if (isBuilding && building.TryGetComponentFast(out TrainYard trainYard))
      {
        _trainYard = trainYard.GetComponentFast<Building>();
        isTrainYard = true;
      }
      
      return 
        isBuilding &&
        building.TryGetComponentFast(out TrackPiece _) &&
        !isTrainYard &&
        !_buildingUnlockingService._unlockedBuildings.Any(buildingName => buildingName.Contains("TrainYard"));
    }

    public void TryToUnlock(Tool tool, Action successCallback, Action failCallback)
    {
      BlockObjectTool plantingToolUnsafe = GetPlantingToolUnsafe(tool);

      if (_buildingUnlockingService.Unlocked(_trainYard))
        successCallback();
      else
        ShowLockedMessage(plantingToolUnsafe, failCallback);
    }
    
    private static bool TryGetBuildingFromTool(Tool tool, out Building building)
    {
      if (tool is BlockObjectTool blockObjectTool)
      {
        Building componentFast = blockObjectTool.Prefab.GetComponentFast<Building>();
        if (componentFast != null)
        {
          building = componentFast;
          return true;
        }
      }
      building = (Building) null;
      return false;
    }

    public static BlockObjectTool GetPlantingToolUnsafe(Tool tool)
    {
      if (IsBlockObjectTool(tool, out var blockObjectTool))
        return blockObjectTool;
      throw new InvalidOperationException($"Tool {tool} is not a PlantingTool");
    }

    public static bool IsBlockObjectTool(Tool tool, out BlockObjectTool blockObjectTool)
    {
      blockObjectTool = tool as BlockObjectTool;
      return blockObjectTool != null;
    }

    public void ShowLockedMessage(BlockObjectTool blockObjectTool, Action failCallback)
    {
      string buildingNameLocKey = "Tobbert.TrainYard.DisplayName";
      string displayNameLocKey = blockObjectTool.Prefab.GetComponentFast<LabeledPrefab>().DisplayNameLocKey;
      string text = _loc.T(UnlockPromptLocKey, _loc.T(buildingNameLocKey), _loc.T(displayNameLocKey));
      _dialogBoxShower.Create().SetMessage(text).SetConfirmButton(failCallback).Show();
    }
  }
}