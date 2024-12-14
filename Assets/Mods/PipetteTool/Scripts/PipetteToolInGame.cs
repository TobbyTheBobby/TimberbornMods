using Timberborn.BaseComponentSystem;
using Timberborn.ConstructionMode;
using Timberborn.CursorToolSystem;
using Timberborn.Debugging;
using Timberborn.InputSystem;
using Timberborn.Localization;
using Timberborn.MapStateSystem;
using Timberborn.SelectionSystem;
using Timberborn.SingletonSystem;
using Timberborn.ToolSystem;

namespace PipetteTool
{
    public class PipetteToolInGame : PipetteTool
    {
        private readonly ConstructionModeService _constructionModeService;

        private readonly InputService _inputService;

        public PipetteToolInGame(EventBus eventBus, ToolManager toolManager, DevModeManager devModeManager,
            InputService inputService, ConstructionModeService constructionModeService, MapEditorMode mapEditorMode,
            CursorService cursorService, SelectableObjectRaycaster selectableObjectRaycaster, ILoc loc, CursorTool cursorTool, ToolButtonService toolButtonService) : base(eventBus, toolManager, devModeManager, inputService, mapEditorMode, cursorService, selectableObjectRaycaster, loc, cursorTool, toolButtonService)
        {
            _constructionModeService = constructionModeService;
            _inputService = inputService;
        }

        public override void PostProcessInput()
        {
            if (!_inputService.Cancel) 
                return;

            if (!_constructionModeService.InConstructionMode) 
                return;
            
            ConstructionModeServicePatch.SkipNext = true;
            ExitConstructionModeMethod.Invoke(_constructionModeService, new object[] { });
        }

        public override void Exit()
        {
            base.Exit();
            ExitConstructionModeMethod.Invoke(_constructionModeService, new object[] { });
        }

        protected override void SwitchToSelectedBuildingTool(ToolButton tool, BaseComponent hitObject)
        {
            base.SwitchToSelectedBuildingTool(tool, hitObject);
            EnterConstructionModeMethod.Invoke(_constructionModeService, new object[] { });
        }
    }
}