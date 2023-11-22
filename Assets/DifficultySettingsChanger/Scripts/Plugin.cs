using System.Diagnostics;
using System.Security.Permissions;
using HarmonyLib;
using TimberApi.ConsoleSystem;
using TimberApi.ModSystem;

#pragma warning disable CS0618
[assembly: SecurityPermission(action: SecurityAction.RequestMinimum, SkipVerification = true)]
#pragma warning restore CS0618

namespace DifficultySettingsChanger
{
    public class Plugin : IModEntrypoint
    {
        public static readonly bool LoggingEnabled = false;
        
        private const string PluginGuid = "tobbert.difficultysettingschanger";
        
        public static IConsoleWriter Log;

        public static IMod Mod;

        public static Stopwatch Stopwatch;
        
        public void Entry(IMod mod, IConsoleWriter consoleWriter)
        {
            Stopwatch = new Stopwatch();
        
            Log = consoleWriter;

            Mod = mod;
            
            new Harmony(PluginGuid).PatchAll();
        }
    }
}