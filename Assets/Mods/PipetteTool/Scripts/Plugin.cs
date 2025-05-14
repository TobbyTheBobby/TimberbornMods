using HarmonyLib;
using Timberborn.ModManagerScene;

namespace PipetteTool
{
    public class Plugin : IModStarter
    {
        public const string Id = "Tobbert.PipetteTool";

        public void StartMod()
        {
            new Harmony(Id).PatchAll();
        }
    }
}