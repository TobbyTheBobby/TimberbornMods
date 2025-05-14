using Timberborn.InputSystem;
using Timberborn.ToolSystem;

namespace PipetteTool.PipetteToolSystem
{
    public class PressMode : IPipetteToolMode, IInputProcessor
    {
        private readonly InputService _inputService;
        private readonly ToolManager _toolManager;
        private readonly PipetteTool _pipetteTool;

        public PressMode(InputService inputService, ToolManager toolManager, PipetteTool pipetteTool)
        {
            _inputService = inputService;
            _toolManager = toolManager;
            _pipetteTool = pipetteTool;
        }

        public string LabelLocKey => "Tobbert.PipetteTool.Setting.PressMode";

        public int SortOrder => 1;

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
            if (!_inputService.IsKeyUpAfterShortHeld(PipetteTool.PipetteToolShortcutKey))
                return false;
            if (_toolManager.ActiveTool == _pipetteTool)
                return false;
            _toolManager.SwitchTool(_pipetteTool);
            return true;
        }
    }
}