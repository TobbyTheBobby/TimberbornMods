using System.Reflection;
using HarmonyLib;
using TobbyTools.UsedImplicitlySystem;

namespace TobbyTools.NewGameModeValueSystem
{
    [UsedImplicitlyHarmonyPatch]
    public class NewGameModelPanelPatch
    {
        private static bool _firstTimeCalled;
        
        public static MethodBase TargetMethod()
        {
            return AccessTools.FirstConstructor(AccessTools.TypeByName("NewGameMode"), _ => true);
        }

        public static void Postfix(ref object __instance)
        {
            if (!_firstTimeCalled)
            {
                _firstTimeCalled = true;
                // Plugin.Log.LogError("I was called");
                // var newGameModeValues = DependencyContainer.GetBoundInstances().OfType<INewGameModeValue>();
                // __instance = new ExtendedNewGameMode((NewGameMode)__instance, newGameModeValues);
            }
            else
            {
                _firstTimeCalled = false;
            }
        }
    }
}