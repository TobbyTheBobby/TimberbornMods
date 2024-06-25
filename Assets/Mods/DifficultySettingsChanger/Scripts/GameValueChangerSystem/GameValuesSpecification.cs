using System.Collections.Generic;

namespace DifficultySettingsChanger
{
    public class GameValuesSpecification
    {
        public readonly List<GameValueSpecification> GameValueSpecifications;

        public GameValuesSpecification(List<GameValueSpecification> gameValueSpecifications)
        {
            GameValueSpecifications = gameValueSpecifications;
        }
    }
}