using System.Security.Permissions;
using HarmonyLib;
using Timberborn.ModManagerScene;

#pragma warning disable CS0618
[assembly: SecurityPermission(action: SecurityAction.RequestMinimum, SkipVerification = true)]
#pragma warning restore CS0618

namespace ExampleMod
{
    public class Plugin : IModStarter
    {
        private const string PluginGuid = "Tobbert.ChooChoo";

        public void StartMod()
        {
            new Harmony(PluginGuid).PatchAll();
        }
    }
}