using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using TimberApi.ConsoleSystem;
using TimberApi.HarmonyPatcherSystem;
using TimberApi.ModSystem;
using UnityEngine;

namespace TobbyTools
{
    public class Plugin : IModEntrypoint
    {
        public const string PluginGuid = "tobbert.tobbytools";
        
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
        
        // public class HarmonyPatcherActivator
        // {
        //     private readonly IEnumerable<IHarmonyPatcher> _harmonyPatchers;
        //
        //     public HarmonyPatcherActivator() => _harmonyPatchers = CreateHarmonyPatchers();
        //
        //     public void PatchAll()
        //     {
        //         foreach (IHarmonyPatcher harmonyPatcher in _harmonyPatchers)
        //         {
        //             Harmony harmony = new Harmony(harmonyPatcher.UniqueId);
        //             try
        //             {
        //                 harmonyPatcher.Apply(harmony);
        //             }
        //             catch (Exception ex)
        //             {
        //                 Log.Log("Patcher " + harmonyPatcher.UniqueId + " failed.", LogType.Error);
        //             }
        //         }
        //     }
        //
        //     private static IEnumerable<IHarmonyPatcher> CreateHarmonyPatchers() => ((IEnumerable<Assembly>) AppDomain.CurrentDomain.GetAssemblies()).SelectMany<Assembly, Type>((Func<Assembly, IEnumerable<Type>>) (x => (IEnumerable<Type>) x.GetTypes())).Where<Type>((Func<Type, bool>) (x => typeof (IHarmonyPatcher).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)).Select<Type, IHarmonyPatcher>((Func<Type, IHarmonyPatcher>) (x => (IHarmonyPatcher) Activator.CreateInstance(x)));
        // }
    }
}