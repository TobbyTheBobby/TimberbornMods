using System.Collections.Generic;
using System.Collections.Immutable;
using TimberApi.Common.SingletonSystem;

namespace DifficultySettingsChanger
{
    public class GameValueChangerRepository : IEarlyLoadableSingleton
    {
        private readonly IEnumerable<IGameValueChangerGenerator> _gameValueChangerGenerators;

        private ImmutableArray<GameValueChanger> _gameValueChangers;

        public ImmutableArray<GameValueChanger> GameValueChangers => _gameValueChangers;

        private GameValueChangerRepository(IEnumerable<IGameValueChangerGenerator> gameValueChangerGenerators)
        {
            _gameValueChangerGenerators = gameValueChangerGenerators;
        }

        public void EarlyLoad()
        {
            var list = new List<GameValueChanger>();
            foreach (var gameValueChangerGenerator in _gameValueChangerGenerators)
            {
                list.AddRange(gameValueChangerGenerator.Generate());
            }

            _gameValueChangers = list.ToImmutableArray();
        }
    }
}