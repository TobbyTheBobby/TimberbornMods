using System.Security.Permissions;
using HarmonyLib;
using Timberborn.ModManagerScene;

#pragma warning disable CS0618
[assembly: SecurityPermission(action: SecurityAction.RequestMinimum, SkipVerification = true)]
#pragma warning restore CS0618

namespace Unstuckify
{
    public class Unstuckify : IModStarter 
    {
        public const string Id = "Tobbert.Unstuckify";
        
        public void StartMod()
        {
            new Harmony(Id).PatchAll();
        }
    }
}