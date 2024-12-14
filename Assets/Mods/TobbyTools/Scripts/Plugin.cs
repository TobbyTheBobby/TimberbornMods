using System.Collections.Generic;
using System.Security.Permissions;
using Timberborn.ModManagerScene;
using TobbyTools.NewGameModeValueSystem;

#pragma warning disable CS0618
[assembly: SecurityPermission(action: SecurityAction.RequestMinimum, SkipVerification = true)]
#pragma warning restore CS0618

namespace TobbyTools
{
    public class Plugin : IModStarter
    {
        public const string PluginGuid = "tobbert.tobbytools";
        
        public void StartMod()
        {
            SubSystemEntryPoints();
        }

        private static void SubSystemEntryPoints()
        {
            var subSystemEntryPoints = new List<ISubSystemEntryPoint>
            {
                // typeof(CustomTutorialSystemEntryPoint),
                new NewGameModeValueSystemEntryPoint(),
                // typeof(OverrideBaseClassSystemEntryPoint),
            };

            foreach (var subSystemEntryPoint in subSystemEntryPoints)
            {
                subSystemEntryPoint.Entry();
            }
        }
    }
}