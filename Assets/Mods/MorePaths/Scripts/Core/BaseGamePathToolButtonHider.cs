using Timberborn.BlockObjectTools;
using Timberborn.CoreUI;
using Timberborn.PrefabSystem;
using Timberborn.SingletonSystem;
using Timberborn.ToolSystem;

namespace MorePaths.Core
{
    public class BaseGamePathToolButtonHider : IPostLoadableSingleton
    {
        private readonly ToolButtonService _toolButtonService;

        public BaseGamePathToolButtonHider(ToolButtonService toolButtonService)
        {
            _toolButtonService = toolButtonService;
        }

        public void PostLoad()
        {
            var toolButton = _toolButtonService.GetToolButton<BlockObjectTool>(IsBaseGamePath);

            toolButton?.Root.ToggleDisplayStyle(false);
        }

        private static bool IsBaseGamePath(BlockObjectTool blockObjectTool)
        {
            if (blockObjectTool.Prefab.TryGetComponentFast(out Prefab prefab))
            {
                return prefab.PrefabName == "Path.Folktails" || prefab.PrefabName ==  "Path.IronTeeth";
            }

            return false;
        }
    }
}