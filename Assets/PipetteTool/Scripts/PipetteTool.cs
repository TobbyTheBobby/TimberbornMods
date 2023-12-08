using System;
using System.Reflection;
using TimberApi.DependencyContainerSystem;
using TimberApi.ToolSystem;
using Timberborn.BaseComponentSystem;
using Timberborn.BlockObjectTools;
using Timberborn.BlockSystem;
using Timberborn.ConstructionMode;
using Timberborn.Coordinates;
using Timberborn.Core;
using Timberborn.CursorToolSystem;
using Timberborn.Debugging;
using Timberborn.InputSystem;
using Timberborn.Localization;
using Timberborn.PrefabSystem;
using Timberborn.SelectionSystem;
using Timberborn.SingletonSystem;
using Timberborn.ToolSystem;
using Tool = Timberborn.ToolSystem.Tool;

namespace PipetteTool
{
  public class PipetteTool : Tool, IPriorityInputProcessor, IInputProcessor, ILoadableSingleton, IPipetteTool
  {
    public static readonly string PipetteToolShortcutKey = "Tobbert.PipetteTool.KeyBinding.PipetteToolShortcut";
    public static readonly string PipetteStartKey = "MouseLeft";

    private static readonly string TitleLocKey = "Tobbert.PipetteTool.DisplayName";
    private static readonly string DescriptionLocKey = "Tobbert.PipetteTool.Description";
    public static string CursorKey => "PipetteCursor";

    private readonly EventBus _eventBus;
    private readonly ToolManager _toolManager;
    private readonly DevModeManager _devModeManager;
    private readonly InputService _inputService;
    private readonly MapEditorMode _mapEditorMode;
    private readonly CursorService _cursorService;
    private readonly SelectableObjectRaycaster _selectableObjectRaycaster;
    private readonly ILoc _loc;
    private readonly CursorTool _cursorTool;
    private readonly ToolButtonService _toolButtonService;
    private ToolService _toolService;
    private ToolDescription _toolDescription;

    private bool _shouldPipetNextSelection;
    private bool _inputProcessorEnabled;

    protected readonly MethodInfo EnterConstructionModeMethod;
    protected readonly MethodInfo ExitConstructionModeMethod;
    private readonly FieldInfo _blockObjectToolOrientationField;

    public PipetteTool(EventBus eventBus, ToolManager toolManager, DevModeManager devModeManager, InputService inputService, MapEditorMode mapEditorMode, CursorService cursorService, SelectableObjectRaycaster selectableObjectRaycaster, ILoc loc, CursorTool cursorTool, ToolButtonService toolButtonService)
    {
      _eventBus = eventBus;
      _toolManager = toolManager;
      _devModeManager = devModeManager;
      _inputService = inputService;
      _mapEditorMode = mapEditorMode;
      _cursorService = cursorService;
      _selectableObjectRaycaster = selectableObjectRaycaster;
      _loc = loc;
      _cursorTool = cursorTool;
      _toolButtonService = toolButtonService;
      
      EnterConstructionModeMethod = typeof(ConstructionModeService).GetMethod("EnterConstructionMode", BindingFlags.NonPublic | BindingFlags.Instance);
      ExitConstructionModeMethod = typeof(ConstructionModeService).GetMethod("ExitConstructionMode", BindingFlags.NonPublic | BindingFlags.Instance);
      _blockObjectToolOrientationField = typeof(BlockObjectTool).GetField("_orientation", BindingFlags.NonPublic | BindingFlags.Instance);
    }

    public void Load()
    {
      _toolService = DependencyContainer.GetInstance<ToolService>();
      _inputService.AddInputProcessor((IPriorityInputProcessor)this);
      _eventBus.Register(this);
      _toolDescription = new ToolDescription.Builder(_loc.T(TitleLocKey)).AddSection(_loc.T(DescriptionLocKey)).Build();
    }

    public void SetToolGroup(ToolGroup toolGroup)
    {
      ToolGroup = toolGroup;
    }

    public override ToolDescription Description() => _toolDescription;

    public override void Enter()
    {
      _shouldPipetNextSelection = true;
      _cursorService.SetTemporaryCursor(CursorKey);
    }

    public override void Exit()
    {
      _cursorService.ResetTemporaryCursor();
      _shouldPipetNextSelection = false;
    }

    void IPriorityInputProcessor.ProcessInput()
    {
      if (!_shouldPipetNextSelection && !_inputService.IsKeyHeld(PipetteToolShortcutKey))
      {
        if (_inputProcessorEnabled)
        {
          _inputService.RemoveInputProcessor(this);
          _inputProcessorEnabled = false;
        }
      }
      
      if (_inputProcessorEnabled) 
        return;
      if (_toolManager.ActiveTool != _cursorTool)
        return;
      _inputService.AddInputProcessor((IInputProcessor)this);
      _inputProcessorEnabled = true;
    }

    bool IInputProcessor.ProcessInput()
    {
      if (!_shouldPipetNextSelection && !_inputService.IsKeyHeld(PipetteToolShortcutKey))
      {
        PostProcessInput();
        return false;
      }
      // Plugin.Log.LogInfo(!_inputService.SelectionStart + "   " + _inputService.MouseOverUI + "    " + (!_inputService.SelectionStart || _inputService.MouseOverUI));
      
      if (!_inputService.IsKeyDown(PipetteStartKey) || _inputService.MouseOverUI)
      {
        PostProcessInput();
        return false;
      }

      if (_selectableObjectRaycaster.TryHitSelectableObject(out var hitObject))
      {
        OnSelectableObjectSelected(hitObject);
        PostProcessInput();
        return true;
      }

      PostProcessInput();
      return false;
    }

    public virtual void PostProcessInput()
    {
      // only used in PipetteToolInGame
    }

    public void OnSelectableObjectSelected(BaseComponent hitObject)
    {
      if (!_inputService.IsKeyHeld(PipetteToolShortcutKey) && !_shouldPipetNextSelection)
        return;

      var selectableObjectName = hitObject.GetComponentFast<Prefab>().PrefabName;

      ToolButton toolButton;
      try
      {
        toolButton = _toolService.GetToolButton(selectableObjectName);
      }
      catch (Exception)
      {
        return;
      }

      if (_mapEditorMode.IsMapEditor)
        SwitchToSelectedBuildingTool(toolButton, hitObject);

      if (toolButton.Tool.DevModeTool && !IsDevModeEnabled)
        return;

      SwitchToSelectedBuildingTool(toolButton, hitObject);
    }

     private void ChangeToolOrientation(Tool tool, BlockObject blockObject)
     {
       if (tool is not BlockObjectTool blockObjectTool) 
         return;

       _blockObjectToolOrientationField.SetValue(blockObjectTool, blockObject.Orientation);
       
       if ((blockObject.FlipMode.IsFlipped && blockObjectTool.FlipMode.IsUnflipped) ||
           (blockObject.FlipMode.IsUnflipped && blockObjectTool.FlipMode.IsFlipped))
       {
         blockObjectTool.Flip();
       }
     }
     
     protected virtual void SwitchToSelectedBuildingTool(ToolButton toolButton, BaseComponent hitObject)
     {
       ChangeToolOrientation(toolButton.Tool, hitObject.GetComponentFast<BlockObject>());
       _toolManager.SwitchTool(toolButton.Tool);
       _shouldPipetNextSelection = false;
       _cursorService.ResetTemporaryCursor();
     }
     
     private bool IsDevModeEnabled => _devModeManager.Enabled;
  }
}
