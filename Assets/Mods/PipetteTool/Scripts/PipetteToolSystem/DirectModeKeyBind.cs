using PipetteTool.SettingsSystem;
using Timberborn.InputSystem;
using Timberborn.ToolSystem;

namespace PipetteTool.PipetteToolSystem
{
    public class DirectModeKeyBind : IPipetteToolMode, IInputProcessor
    {
        private readonly InputService _inputService;
        private readonly PipetteTool _pipetteTool;
        private readonly ToolManager _toolManager;

        private Tool _previousTool;

        public DirectModeKeyBind(InputService inputService, PipetteTool pipetteTool, ToolManager toolManager)
        {
            _inputService = inputService;
            _pipetteTool = pipetteTool;
            _toolManager = toolManager;
        }

        public string LabelLocKey => "Tobbert.PipetteTool.Setting.DirectModeKeyBind";

        public int SortOrder => 3;

        public void EnterMode()
        {
            _inputService.AddInputProcessor(this);
        }

        public void ExitMode()
        {
            _inputService.RemoveInputProcessor(this);
        }

        public bool ProcessInput()
        {
            if (_pipetteTool.PipetteBlocked)
                return false;
            if (_inputService.IsKeyUpAfterShortHeld(PipetteTool.PipetteToolShortcutKey))
            {
                _previousTool = _toolManager.ActiveTool;
                _toolManager.SwitchToDefaultTool();

                if (_pipetteTool.TryHitSelectableObject())
                {
                    _previousTool = null;
                    return true;
                }

                _toolManager.SwitchTool(_previousTool);
                _previousTool = null;
            }

            return false;
        }
    }
}