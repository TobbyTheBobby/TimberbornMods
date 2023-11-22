using System.Reflection;
using HarmonyLib;
using TimberApi.DependencyContainerSystem;
using UnityEngine.UIElements;

namespace Unstuckify
{
    [HarmonyPatch]
    public class SettingsPatch
    {
        static MethodInfo TargetMethod()
        {
            return AccessTools.Method(AccessTools.TypeByName("GameSavingSettingsController"), "InitializeAutoSavingOnToggle", new []
            {
                typeof(VisualElement)
            });
        }
        
        static void Postfix(ref VisualElement root)
        {
            DependencyContainer.GetInstance<UnstuckifySettingsUI>().InitializeSelectorSettings(ref root);
        }
    }
}