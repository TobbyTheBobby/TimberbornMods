using System;
using System.Reflection;
using PipetteTool.SettingsSystem;
using Timberborn.BaseComponentSystem;
using Timberborn.BlockObjectTools;
using Timberborn.BlockSystem;
using Timberborn.ConstructionMode;
using Timberborn.CursorToolSystem;
using Timberborn.Debugging;
using Timberborn.InputSystem;
using Timberborn.Localization;
using Timberborn.PlantingUI;
using Timberborn.PrefabSystem;
using Timberborn.SelectionSystem;
using Timberborn.SingletonSystem;
using Timberborn.ToolSystem;
using Tool = Timberborn.ToolSystem.Tool;

namespace PipetteTool.PipetteToolSystem
{
    public class PipetteTool : Tool, IPriorityInputProcessor, IInputProcessor, ILoadableSingleton
    {
        public static readonly string PipetteToolShortcutKey = "Tobbert.PipetteTool.KeyBinding.PipetteTool";

        private static readonly string TitleLocKey = "Tobbert.PipetteTool.DisplayName";
        private static readonly string DescriptionLocKey = "Tobbert.PipetteTool.Description";
        private static string CursorKey => "PipetteToolCursor";

        private readonly EventBus _eventBus;
        private readonly ToolManager _toolManager;
        private readonly DevModeManager _devModeManager;
        private readonly InputService _inputService;
        private readonly CursorService _cursorService;
        private readonly SelectableObjectRaycaster _selectableObjectRaycaster;
        private readonly ILoc _loc;
        private readonly ToolButtonService _toolButtonService;
        private readonly ConstructionModeService _constructionModeService;
        private readonly PipetteToolSettingsOwner _pipetteToolSettingsOwner;
        private readonly PlantingModeService _plantingModeService;
        private ToolDescription _toolDescription;

        private bool _shouldPipetteNextSelection;
        private bool _inputProcessorEnabled;

        private readonly FieldInfo _blockObjectToolOrientationField;

        public bool PipetteBlocked => !_pipetteToolSettingsOwner.PipetteToolForcePipette.Value && !_toolManager.IsDefaultToolActive && _toolManager.ActiveTool != this;

        public PipetteTool(
            EventBus eventBus,
            ToolManager toolManager,
            DevModeManager devModeManager,
            InputService inputService,
            CursorService cursorService,
            SelectableObjectRaycaster selectableObjectRaycaster,
            ILoc loc,
            CursorTool cursorTool,
            ToolButtonService toolButtonService,
            ConstructionModeService constructionModeService,
            PipetteToolSettingsOwner pipetteToolSettingsOwner,
            PlantingModeService plantingModeService)
        {
            _eventBus = eventBus;
            _toolManager = toolManager;
            _devModeManager = devModeManager;
            _inputService = inputService;
            _cursorService = cursorService;
            _selectableObjectRaycaster = selectableObjectRaycaster;
            _loc = loc;
            _toolButtonService = toolButtonService;
            _constructionModeService = constructionModeService;
            _pipetteToolSettingsOwner = pipetteToolSettingsOwner;
            _plantingModeService = plantingModeService;

            _blockObjectToolOrientationField = typeof(BlockObjectTool).GetField("_orientation", BindingFlags.NonPublic | BindingFlags.Instance);
        }

        public void Load()
        {
            _inputService.AddInputProcessor((IPriorityInputProcessor)this);
            _eventBus.Register(this);
            _toolDescription = new ToolDescription.Builder(_loc.T(TitleLocKey)).AddSection(_loc.T(DescriptionLocKey)).Build();
        }

        public override ToolDescription Description()
        {
            return _toolDescription;
        }

        public override void Enter()
        {
            _shouldPipetteNextSelection = true;
            _cursorService.SetTemporaryCursor(CursorKey);
        }

        public override void Exit()
        {
            _cursorService.ResetCursor();
            _shouldPipetteNextSelection = false;
            _constructionModeService.ExitConstructionMode();
            _plantingModeService.ExitPlantingMode();
        }

        void IPriorityInputProcessor.ProcessInput()
        {
            if (!_shouldPipetteNextSelection)
            {
                if (_inputProcessorEnabled)
                {
                    _inputService.RemoveInputProcessor(this);
                    _inputProcessorEnabled = false;
                }
            }

            if (_inputProcessorEnabled)
                return;
            _inputService.AddInputProcessor((IInputProcessor)this);
            _inputProcessorEnabled = true;
        }

        bool IInputProcessor.ProcessInput()
        {
            if (!_shouldPipetteNextSelection)
                return false;
            if (!_inputService.IsKeyDown("MouseLeft") || _inputService.MouseOverUI)
                return false;
            return TryHitSelectableObject();
        }

        public bool TryHitSelectableObject()
        {
            return _selectableObjectRaycaster.TryHitSelectableObject(out var hitObject) && OnSelectableObjectSelected(hitObject);
        }

        public bool OnSelectableObjectSelected(BaseComponent hitObject)
        {
            var selectableObjectName = hitObject.GetComponentFast<PrefabSpec>().PrefabName;

            ToolButton toolButton;
            try
            {
                toolButton = _toolButtonService.GetToolButton<Tool>(tool => ValidTool(tool, selectableObjectName));
            }
            catch (Exception exception)
            {
                // Debug.LogError(exception);
                return false;
            }

            if (toolButton.Tool.DevModeTool && DevModeDisabled)
                return false;

            SwitchToSelectedBuildingTool(toolButton, hitObject);
            return true;
        }

        private static bool ValidTool(Tool tool, string selectableObjectName)
        {
            switch (tool)
            {
                case BlockObjectTool blockObjectTool:
                    return blockObjectTool.Prefab.GetComponentFast<PrefabSpec>().PrefabName == selectableObjectName;
                case PlantingTool plantingTool:
                    return plantingTool.PlantableSpec.GetComponentFast<PrefabSpec>().PrefabName == selectableObjectName;
                default:
                    return false;
            }
        }

        private void SwitchToSelectedBuildingTool(ToolButton toolButton, BaseComponent hitObject)
        {
            switch (toolButton.Tool)
            {
                case BlockObjectTool blockObjectTool:
                    _constructionModeService.EnterConstructionMode();
                    ChangeToolOrientation(blockObjectTool, hitObject.GetComponentFast<BlockObject>());
                    break;
                case PlantingTool:
                    _plantingModeService.EnterPlantingMode();
                    break;
            }

            _toolManager.SwitchTool(toolButton.Tool);
            _shouldPipetteNextSelection = false;
            _cursorService.ResetTemporaryCursor();
        }

        private void ChangeToolOrientation(BlockObjectTool blockObjectTool, BlockObject blockObject)
        {
            _blockObjectToolOrientationField.SetValue(blockObjectTool, blockObject.Orientation);

            if ((blockObject.FlipMode.IsFlipped && blockObjectTool.FlipMode.IsUnflipped) ||
                (blockObject.FlipMode.IsUnflipped && blockObjectTool.FlipMode.IsFlipped))
            {
                blockObjectTool.Flip();
            }
        }

        private bool DevModeDisabled => !_devModeManager.Enabled;
    }
}