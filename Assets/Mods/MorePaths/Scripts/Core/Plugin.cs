using HarmonyLib;
using TimberApi.ConsoleSystem;
using TimberApi.ModSystem;

namespace MorePaths
{
    public class Plugin : IModEntrypoint
    {
        public const string PluginGuid = "tobbert.morepaths";

        public static IMod Mod;
        
        public static string Path => Mod.DirectoryPath;
        
        public static IConsoleWriter Log;
        
        public void Entry(IMod mod, IConsoleWriter consoleWriter)
        {
            Mod = mod;
            
            Log = consoleWriter;

            new Harmony(PluginGuid).PatchAll();
        }
    }
}