using Timberborn.InputSystem;
using Timberborn.ToolSystem;

namespace PipetteTool.PipetteToolSystem
{
    public class HoldMode : IPipetteToolMode, IInputProcessor
    {
        private readonly InputService _inputService;
        private readonly ToolManager _toolManager;
        private readonly PipetteTool _pipetteTool;

        private Tool _previousTool;

        public HoldMode(InputService inputService, ToolManager toolManager, PipetteTool pipetteTool)
        {
            _inputService = inputService;
            _toolManager = toolManager;
            _pipetteTool = pipetteTool;
        }

        public string LabelLocKey => "Tobbert.PipetteTool.Setting.HoldMode";


        public int SortOrder => 0;

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

            if (_inputService.IsKeyHeld(PipetteTool.PipetteToolShortcutKey))
            {
                if (_toolManager.ActiveTool == _pipetteTool)
                    return false;

                _previousTool = _toolManager.ActiveTool;
                _toolManager.SwitchTool(_pipetteTool);

                return false;
            }

            if (_toolManager.ActiveTool == _pipetteTool && _previousTool != null)
            {
                _toolManager.SwitchTool(_previousTool);
                _previousTool = null;
                return false;
            }

            return false;
        }
    }
}