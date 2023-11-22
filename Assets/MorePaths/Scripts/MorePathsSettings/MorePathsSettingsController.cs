using System.Collections.Generic;
using System.Linq;
using Timberborn.BlockObjectTools;
using Timberborn.CoreUI;
using Timberborn.EntitySystem;
using Timberborn.PathSystem;
using Timberborn.PrefabSystem;
using Timberborn.SingletonSystem;
using Timberborn.ToolSystem;

namespace MorePaths
{
    public class MorePathsSettingsController : IPostLoadableSingleton
    {
        private readonly ToolButtonService _toolButtonService;

        private readonly MorePathsSettings _morePathsSettings;

        private readonly EventBus _eventBus;

        private List<ToolButton> _pathToolButtons;

        MorePathsSettingsController(ToolButtonService toolButtonService, MorePathsSettings morePathsSettings, EventBus eventBus)
        {
            _toolButtonService = toolButtonService;
            _morePathsSettings = morePathsSettings;
            _eventBus = eventBus;
        }
        
        public void PostLoad()
        {
            _eventBus.Register(this);
            GetPathToolButtons();
            UpdateButtons();
        }

        [OnEvent]
        public void OnSettingsChanged(MorePathsSettingsChanged morePathsSettingsChanged)
        {
            UpdateButtons();
        }

        private void GetPathToolButtons()
        {
            var blockObjectToolButtons = _toolButtonService.ToolButtons.Where(button => button.Tool.GetType() == typeof(BlockObjectTool));
            var toolButtons = blockObjectToolButtons.Where(button =>
            {
                BlockObjectTool blockObjectTool = button.Tool as BlockObjectTool;
                return blockObjectTool.Prefab.TryGetComponentFast(out DynamicPathModel _);
            });
            _pathToolButtons = toolButtons.ToList();
        }

        private void UpdateButtons()
        {
            foreach (var toolButton in _pathToolButtons)
            {
                var tool = toolButton.Tool as BlockObjectTool;
                var gameObjectName = tool.Prefab.GetComponentFast<Prefab>().PrefabName;
                if (gameObjectName.Equals("Path.Folktails")  || gameObjectName.Equals("Path.IronTeeth"))
                    // continue;
                    gameObjectName = "DefaultPath";
                var setting = _morePathsSettings.GetSetting(gameObjectName);
                toolButton.Root.ToggleDisplayStyle(setting.Enabled);
            }
        }
    }
}
