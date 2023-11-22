using System.Reflection;
using HarmonyLib;
using TimberApi.ConsoleSystem;
using TimberApi.ModSystem;
using Timberborn.AssetSystem;
using Object = UnityEngine.Object;

namespace PipetteTool
{
    public class Plugin : IModEntrypoint
    {
        private const string PluginGuid = "tobbert.pipettetool";
        
        public static IConsoleWriter Log;
        
        public void Entry(IMod mod, IConsoleWriter consoleWriter)
        {
            Log = consoleWriter; 
            
            new Harmony(PluginGuid).PatchAll();
        }
    }

    [HarmonyPatch]
    public class CursorServicePatch
    {
        public static MethodInfo TargetMethod()
        {
            return AccessTools.Method(AccessTools.TypeByName("CursorService"), "GetCursor", new []{typeof(string)});
        }
        
        static bool Prefix(ref string cursorName, IResourceAssetLoader ____resourceAssetLoader, ref object __result)
        {
            if (cursorName == PipetteTool.CursorKey)
            {
                __result = ____resourceAssetLoader.Load<Object>("tobbert.pipettetool/tobbert_pipettetool/PipetteToolCursor");
                return false;
            }

            return true;
        }
    }
    
    [HarmonyPatch]
    public class ConstructionModeServicePatch
    {
        public static bool SkipNext;
        
        public static MethodInfo TargetMethod()
        {
            return AccessTools.Method(AccessTools.TypeByName("ConstructionModeService"), "CanExitConstructionMode");
        }
        
        static bool Prefix(ref bool __result)
        {
            if (!SkipNext) 
                return true;
            
            SkipNext = false;
            __result = true;
            return false;

        }
    }
}