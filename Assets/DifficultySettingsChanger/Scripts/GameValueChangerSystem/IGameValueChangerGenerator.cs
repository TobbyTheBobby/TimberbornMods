using System.Collections.Generic;

namespace DifficultySettingsChanger
{
    public interface IGameValueChangerGenerator
    {
        IEnumerable<GameValueChanger> Generate();
    }
}