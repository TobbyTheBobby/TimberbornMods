using Timberborn.SingletonSystem;

namespace TestingMod.NewGameModeValueSystemExample
{
    public class SeasonService : ILoadableSingleton
    {
        private readonly SeasonDurationNewGameValue _seasonDurationNewGameValue;
        
        SeasonService(SeasonDurationNewGameValue seasonDurationNewGameValue)
        {
            _seasonDurationNewGameValue = seasonDurationNewGameValue;
        }

        public void Load()
        {
            TobbyTools.Plugin.Log.LogWarning(_seasonDurationNewGameValue.EasyDifficultyValue.ToString());
        }
    }
}