using System.Reflection;
using HarmonyLib;
using TimberApi.ConsoleSystem;
using TimberApi.ModSystem;
using TobbyTools.UsedImplicitlySystem;
using UnityEngine;

namespace TestingMod
{
    public class Plugin : IModEntrypoint
    {
        public const string PluginGuid = "tobbert.testingmod";

        public static IConsoleWriter Log;

        public void Entry(IMod mod, IConsoleWriter consoleWriter)
        {
            Log = consoleWriter;
            
            new Harmony(PluginGuid).PatchAll();
        }
    }
    
    [UsedImplicitlyHarmonyPatch]
    public class SettingsPatch
    {
        private static MethodInfo TargetMethod()
        {
            return AccessTools.Method(AccessTools.TypeByName("StockpileBannerSetter"), "Awake");
        }
        
        private static void Postfix(ref MeshRenderer ____meshRenderer)
        {
            ____meshRenderer = ____meshRenderer.GetComponentInChildren<MeshRenderer>();
        }
    }
}