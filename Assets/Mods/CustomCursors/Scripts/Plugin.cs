using System.Security.Permissions;
using HarmonyLib;
using Timberborn.ModManagerScene;

#pragma warning disable CS0618
[assembly: SecurityPermission(action: SecurityAction.RequestMinimum, SkipVerification = true)]
#pragma warning restore CS0618

namespace CustomCursors
{
    public class Plugin : IModStarter
    {
        public const string PluginGuid = "Tobbert.CustomCursors";

        public void StartMod()
        {
            new Harmony(PluginGuid).PatchAll();
        }
    }
}