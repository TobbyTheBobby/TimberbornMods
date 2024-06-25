using System;
using HarmonyLib;
using TimberApi.ConsoleSystem;
using TimberApi.ModSystem;

namespace DynamicSpecifications
{
    public class Plugin : IModEntrypoint
    {
        private const string PluginGuid = "tobbert.dynamicspecifications";
        
        public static IConsoleWriter Log;

        public static IMod Mod;

        public static readonly bool LoggingEnabled = false;
        
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