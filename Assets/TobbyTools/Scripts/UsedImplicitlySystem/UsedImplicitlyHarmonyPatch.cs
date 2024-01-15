using JetBrains.Annotations;

namespace TobbyTools.UsedImplicitlySystem
{
    [MeansImplicitUse(ImplicitUseTargetFlags.WithMembers)]
    [UsedImplicitly]
    public class UsedImplicitlyHarmonyPatch : HarmonyLib.HarmonyPatch
    {
        
    }
}