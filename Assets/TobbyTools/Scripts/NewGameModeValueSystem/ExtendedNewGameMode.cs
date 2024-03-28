using System.Collections.Generic;
using Timberborn.GameSceneLoading;

namespace TobbyTools.NewGameModeValueSystem
{
    public class ExtendedNewGameMode : NewGameMode
    {
        public IEnumerable<INewGameModeValue> NewGameModeValues;

        public ExtendedNewGameMode(NewGameMode newGameMode, IEnumerable<INewGameModeValue> newGameModeValues) : base(newGameMode.StartingAdults, newGameMode.AdultAgeProgress, newGameMode.StartingChildren, newGameMode.ChildAgeProgress, newGameMode.FoodConsumption, newGameMode.WaterConsumption, newGameMode.StartingFood, newGameMode.StartingWater, newGameMode.TemperateWeatherDuration, newGameMode.DroughtDuration, newGameMode.DroughtDurationHandicapMultiplier, newGameMode.DroughtDurationHandicapCycles, newGameMode.CyclesBeforeRandomizingBadtide, newGameMode.ChanceForBadtide, newGameMode.BadtideDuration, newGameMode.BadtideDurationHandicapMultiplier, newGameMode.BadtideDurationHandicapCycles, newGameMode.InjuryChance, newGameMode.DemolishableRecoveryRate)
        {
            NewGameModeValues = newGameModeValues;
        }
    }
}