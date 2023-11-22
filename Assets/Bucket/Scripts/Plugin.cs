using System.Reflection;
using HarmonyLib;
using TimberApi.ConsoleSystem;
using TimberApi.ModSystem;
using UnityEngine;

namespace CustomCursors
{
    public class Plugin : IModEntrypoint
    {
        public const string PluginGuid = "tobbert.bucket";

        public static IConsoleWriter Log;

        public void Entry(IMod mod, IConsoleWriter consoleWriter)
        {
            Log = consoleWriter;
            
            new Harmony(PluginGuid).PatchAll();
        }
    }
    
    [HarmonyPatch]
    public class SettingsPatch
    {
        static MethodInfo TargetMethod()
        {
            return AccessTools.Method(AccessTools.TypeByName("StockpileBannerSetter"), "Awake");
        }
        
        static void Postfix(ref MeshRenderer ____meshRenderer)
        {
            ____meshRenderer = ____meshRenderer.GetComponentInChildren<MeshRenderer>();
        }
    }
}