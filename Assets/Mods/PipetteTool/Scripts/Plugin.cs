using System.Reflection;
using HarmonyLib;
using Timberborn.ModManagerScene;

namespace PipetteTool
{
    public class Plugin : IModStarter
    {
        private const string PluginGuid = "tobbert.pipettetool";
        
        public void StartMod()
        {
            new Harmony(PluginGuid).PatchAll();
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