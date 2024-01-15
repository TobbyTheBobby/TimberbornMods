using System.Reflection;
using HarmonyLib;
using TimberApi.DependencyContainerSystem;
using TobbyTools.UsedImplicitlySystem;
using UnityEngine.UIElements;

namespace ChooChoo.Settings
{
    [UsedImplicitlyHarmonyPatch]
    public class SettingsPatch
    {
        private static MethodInfo TargetMethod()
        {
            return AccessTools.Method(AccessTools.TypeByName("GameSavingSettingsController"), "InitializeAutoSavingOnToggle", new[]
            {
                typeof(VisualElement)
            });
        }

        private static void Postfix(ref VisualElement root)
        {
            DependencyContainer.GetInstance<ChooChooSettingsUI>().InitializeSelectorSettings(ref root);
        }
    }
}