using System.Reflection;
using HarmonyLib;
using TimberApi.ConsoleSystem;
using TimberApi.DependencyContainerSystem;
using TimberApi.ModSystem;
using UnityEngine.UIElements;

namespace CustomCursors
{
    public class Plugin : IModEntrypoint
    {
        public const string PluginGuid = "tobbert.customcursors";
        public static string MyPath;
        public static IConsoleWriter Log;

        public void Entry(IMod mod, IConsoleWriter consoleWriter)
        {
            MyPath = mod.DirectoryPath;
            Log = consoleWriter;
            new Harmony(PluginGuid).PatchAll();
        }
    }


    [HarmonyPatch]
    public class StartGrabbingPatch
    {
        private static MethodInfo TargetMethod()
        {
            return AccessTools.Method(AccessTools.TypeByName("GrabbingCameraTargetPicker"), "StartGrabbing");
        }

        private static void Postfix()
        {
            DependencyContainer.GetInstance<CustomCursorsService>().StartGrabbing();
        }
    }
    
    [HarmonyPatch]
    public class StopGrabbingPatch
    {
        private static MethodInfo TargetMethod()
        {
            return AccessTools.Method(AccessTools.TypeByName("GrabbingCameraTargetPicker"), "StopGrabbing");
        }

        private static void Postfix()
        {
            DependencyContainer.GetInstance<CustomCursorsService>().StopGrabbing();
        }
    }
    
    [HarmonyPatch]
    public class SettingsPatch
    {
        private static MethodInfo TargetMethod()
        {
            return AccessTools.Method(AccessTools.TypeByName("GameSavingSettingsController"), "InitializeAutoSavingOnToggle", new []
            {
                typeof(VisualElement)
            });
        }

        private static void Postfix(ref VisualElement root)
        {
            DependencyContainer.GetInstance<CustomCursorsService>().InitializeSelectorSettings(ref root);
        }
    }
}