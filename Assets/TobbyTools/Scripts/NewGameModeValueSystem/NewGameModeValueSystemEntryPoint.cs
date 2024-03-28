using HarmonyLib;

namespace TobbyTools.NewGameModeValueSystem
{
    public class NewGameModeValueSystemEntryPoint : ISubSystemEntryPoint
    {
        public void Entry()
        {
            Harmony.CreateAndPatchAll(typeof(NewGameModeConstructorPatch), Plugin.PluginGuid);
            Harmony.CreateAndPatchAll(typeof(NewGameModelPanelPatch), Plugin.PluginGuid);
            Harmony.CreateAndPatchAll(typeof(EasyDifficultyNewGameModePanelPatch), Plugin.PluginGuid);
            Harmony.CreateAndPatchAll(typeof(NormalDifficultyNewGameModePanelPatch), Plugin.PluginGuid);
            Harmony.CreateAndPatchAll(typeof(HardDifficultyNewGameModePanelPatch), Plugin.PluginGuid);
        }
    }
}