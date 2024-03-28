using System.Reflection;
using HarmonyLib;
using TimberApi.DependencyContainerSystem;
using Timberborn.MainMenuScene;
using TobbyTools.UsedImplicitlySystem;

namespace TobbyTools.NewGameModeValueSystem
{
    [UsedImplicitlyHarmonyPatch]
    public class EasyDifficultyNewGameModePanelPatch
    {
        public static MethodBase TargetMethod()
        {
            return AccessTools.Method(typeof(NewGameModePanel), "OnEasyModeButtonClicked");
        }

        public static void Postfix()
        {
            var newGameValueRepository = DependencyContainer.GetInstance<NewGameValueRepository>();
            foreach (var newGameModeValue in newGameValueRepository.NewGameModeValues)
            {
                newGameModeValue.OnEasyModeButtonClicked();
            }
        }
    }
    
    [UsedImplicitlyHarmonyPatch]
    public class NormalDifficultyNewGameModePanelPatch
    {
        public static MethodBase TargetMethod()
        {
            return AccessTools.Method(typeof(NewGameModePanel), "OnNormalModeButtonClicked");
        }

        public static void Postfix()
        {
            var newGameValueRepository = DependencyContainer.GetInstance<NewGameValueRepository>();
            foreach (var newGameModeValue in newGameValueRepository.NewGameModeValues)
            {
                newGameModeValue.OnNormalModeButtonClicked();
            }
        }
    }
    
    [UsedImplicitlyHarmonyPatch]
    public class HardDifficultyNewGameModePanelPatch
    {
        public static MethodBase TargetMethod()
        {
            return AccessTools.Method(typeof(NewGameModePanel), "OnHardModeButtonClicked");
        }

        public static void Postfix()
        {
            var newGameValueRepository = DependencyContainer.GetInstance<NewGameValueRepository>();
            foreach (var newGameModeValue in newGameValueRepository.NewGameModeValues)
            {
                newGameModeValue.OnHardModeButtonClicked();
            }
        }
    }
}