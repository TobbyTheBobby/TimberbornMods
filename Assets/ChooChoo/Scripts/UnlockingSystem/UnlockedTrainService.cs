using System.Collections.Generic;
using Timberborn.BlockObjectTools;
using Timberborn.Buildings;
using Timberborn.CoreUI;
using Timberborn.Localization;
using Timberborn.PrefabSystem;
using Timberborn.ScienceSystem;
using Timberborn.SingletonSystem;
using Timberborn.ToolSystem;

namespace ChooChoo
{
  public class UnlockedTrainService : IPostLoadableSingleton
  {
    private static readonly string UnlockPromptLocKey = "Tobbert.Trains.UnlockPrompt";
    private readonly EventBus _eventBus;
    private readonly BuildingService _buildingService;
    private readonly BuildingUnlockingService _buildingUnlockingService;
    private readonly PrefabNameMapper _prefabNameMapper;
    private readonly ToolButtonService _toolButtonService;
    private readonly ILoc _loc;
    private readonly DialogBoxShower _dialogBoxShower;
    private bool _trainsUnlocked;
    private readonly HashSet<string> _unlockedResourceGroups = new();

    public UnlockedTrainService(
      EventBus eventBus,
      BuildingService buildingService,
      BuildingUnlockingService buildingUnlockingService,
      PrefabNameMapper prefabNameMapper,
      ToolButtonService toolButtonService,
      ILoc loc,
      DialogBoxShower dialogBoxShower)
    {
      _eventBus = eventBus;
      _buildingService = buildingService;
      _buildingUnlockingService = buildingUnlockingService;
      _prefabNameMapper = prefabNameMapper;
      _toolButtonService = toolButtonService;
      _loc = loc;
      _dialogBoxShower = dialogBoxShower;
    }

    public void PostLoad()
    {
      _eventBus.Register(this);
      TryUnlockingTrains();
      LockTrains();
    }

    [OnEvent]
    public void OnBuildingUnlocked(BuildingUnlockedEvent buildingUnlockedEvent)
    {
      TryUnlockingTrains(_prefabNameMapper.GetPrefab(_buildingService.GetPrefabName(buildingUnlockedEvent.Building)).GetComponentFast<TrainYard>());
      UnlockTrains();
    }

    private void TryUnlockingTrains()
    {
      foreach (Building building in _buildingService.Buildings)
      {
        if (_buildingUnlockingService.Unlocked(building))
          TryUnlockingTrains(building.GetComponentFast<TrainYard>());
      }
    }

    private void TryUnlockingTrains(TrainYard trainYard)
    {
      if (!(trainYard != null))
        return;
      _trainsUnlocked = true;
    }

    private void LockTrains()
    {
      foreach (ToolButton toolButton in _toolButtonService.ToolButtons)
      {
        BlockObjectTool blockObjectTool = toolButton.Tool as BlockObjectTool;
        
        var trackPiece = blockObjectTool?.Prefab.GetComponentFast<TrackPiece>();
        var trainYard = blockObjectTool?.Prefab.GetComponentFast<TrainYard>();
        if (trackPiece != null && !trainYard && TrainsAreLocked())
        {
          blockObjectTool.Locked = true;
          toolButton.ButtonClicked += (_1, _2) => OnButtonClicked(blockObjectTool);
        }
      }
    }

    private void UnlockTrains()
    {
      foreach (ToolButton toolButton in _toolButtonService.ToolButtons)
      {
        BlockObjectTool blockObjectTool = toolButton.Tool as BlockObjectTool;
        
        var trackPiece = blockObjectTool?.Prefab.GetComponentFast<TrackPiece>();
        var trainYard = blockObjectTool?.Prefab.GetComponentFast<TrainYard>();
        if (trackPiece != null && !trainYard && !TrainsAreLocked())
        {
          blockObjectTool.Locked = false;
        }
      }
    }

    private void OnButtonClicked(BlockObjectTool blockObjectTool)
    {
      if (!TrainsAreLocked())
        return;
      ShowLockedMessage(blockObjectTool);
    }

    private bool TrainsAreLocked() => !_trainsUnlocked;

    private void ShowLockedMessage(BlockObjectTool blockObjectTool)
    {
      string displayNameLocKey = blockObjectTool.Prefab.GetComponentFast<LabeledPrefab>().DisplayNameLocKey;
      string text = _loc.T(UnlockPromptLocKey, _loc.T("Tobbert.TrainYard.DisplayName"), _loc.T(displayNameLocKey));
      _dialogBoxShower.Create().SetMessage(text).Show();
    }
  }
}
