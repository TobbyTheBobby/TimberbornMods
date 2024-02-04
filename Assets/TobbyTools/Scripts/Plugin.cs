using System;
using System.Collections.Generic;
using TimberApi.ConsoleSystem;
using TimberApi.ModSystem;
using TobbyTools.CustomTutorialSystem;
using TobbyTools.HookSystem;

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

            SubSystemEntryPoints();
        }

        private static void SubSystemEntryPoints()
        {
            var subSystemEntryPoints = new List<Type>()
            {
                typeof(CustomTutorialSystemEntryPoint),
                // typeof(OverrideBaseClassSystemEntryPoint),
            };

            foreach (var subSystemEntryPoint in subSystemEntryPoints)
            {
                var instance = (ISubSystemEntryPoint)Activator.CreateInstance(subSystemEntryPoint);
                instance.Entry();
            }
        }
    }
}