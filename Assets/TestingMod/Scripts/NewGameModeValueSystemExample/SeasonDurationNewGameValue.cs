using Timberborn.Common;
using TobbyTools.NewGameModeValueSystem;

namespace TestingMod.NewGameModeValueSystemExample
{
    public class SeasonDurationNewGameValue : NewGameModeValue<MinMax<int>>
    {
        protected override string SettingLabelLocKey => "SeasonDurationNewGameValue";

        public override string Section => "Seasons";
        
        public override MinMax<int> EasyDifficultyValue => new (1, 2);
        public override MinMax<int> MediumDifficultyValue => new (10, 20);
        public override MinMax<int> HardDifficultyValue => new (100, 200);
    }
}