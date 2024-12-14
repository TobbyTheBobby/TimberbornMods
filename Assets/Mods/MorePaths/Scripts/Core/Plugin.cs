using System.Security.Permissions;
using HarmonyLib;
using Timberborn.ModManagerScene;

#pragma warning disable CS0618
[assembly: SecurityPermission(action: SecurityAction.RequestMinimum, SkipVerification = true)]
#pragma warning restore CS0618

namespace MorePaths.Core
{
    public class Plugin : IModStarter
    {
        public const string PluginGuid = "Tobbert.MorePaths";

        public static IModEnvironment ModEnvironment;
        
        public static string Path => ModEnvironment.ModPath;
        
        public void StartMod(IModEnvironment modEnvironment)
        {
            ModEnvironment = modEnvironment;
            
            new Harmony(PluginGuid).PatchAll();
        }
    }
}