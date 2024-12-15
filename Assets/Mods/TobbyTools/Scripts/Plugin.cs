using System.Security.Permissions;
using Timberborn.ModManagerScene;

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
            
        }
    }
}