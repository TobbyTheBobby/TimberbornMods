using System;
using HarmonyLib;
using TimberApi.ConsoleSystem;
using TimberApi.ModSystem;

namespace TobbyTools
{
    public class Plugin : IModEntrypoint
    {
        public const string PluginGuid = "tobbert.tobbytools";
        
        public static IConsoleWriter Log;

        public static IMod Mod;
        
        public void Entry(IMod mod, IConsoleWriter consoleWriter)
        {
            Log = consoleWriter;

            Mod = mod;
            
            try
            {
                new Harmony(PluginGuid).PatchAll();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}