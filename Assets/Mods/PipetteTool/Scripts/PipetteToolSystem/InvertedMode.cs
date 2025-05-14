using Timberborn.InputSystem;
using Timberborn.SelectionSystem;

namespace PipetteTool.PipetteToolSystem
{
    public class InvertedMode : IPipetteToolMode, IInputProcessor
    {
        private readonly InputService _inputService;
        private readonly PipetteTool _pipetteTool;
        private readonly EntitySelectionService _entitySelectionService;

        public InvertedMode(InputService inputService, PipetteTool pipetteTool, EntitySelectionService entitySelectionService)
        {
            _inputService = inputService;
            _entitySelectionService = entitySelectionService;
            _pipetteTool = pipetteTool;
        }

        public string LabelLocKey => "Tobbert.PipetteTool.Setting.InvertedMode";

        public int SortOrder => 4;

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

            if (_inputService.IsKeyDown(PipetteTool.PipetteToolShortcutKey) && _entitySelectionService.SelectedObject != null)
            {
                _pipetteTool.OnSelectableObjectSelected(_entitySelectionService.SelectedObject);
                return true;
            }

            return false;
        }
    }
}