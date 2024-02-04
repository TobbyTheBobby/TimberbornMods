using HarmonyLib;

namespace TobbyTools.CustomTutorialSystem
{
    public class CustomTutorialSystemEntryPoint : ISubSystemEntryPoint
    {
        public void Entry()
        {
            Harmony.CreateAndPatchAll(typeof(TutorialConfigurationProviderPatch), Plugin.PluginGuid);
        }
    }
}