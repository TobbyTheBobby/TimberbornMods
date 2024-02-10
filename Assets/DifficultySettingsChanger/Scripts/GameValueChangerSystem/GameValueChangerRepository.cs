using System.Collections.Generic;
using System.Collections.Immutable;
using TimberApi.Common.SingletonSystem;

namespace DifficultySettingsChanger
{
    public class GameValueChangerRepository : IEarlyLoadableSingleton
    {
        private readonly IEnumerable<IGameValueChangerGenerator> _gameValueChangerGenerators;

        private readonly List<GameValueChanger> _gamevalueChangers = new();

        public ImmutableArray<GameValueChanger> GamevalueChangers => _gamevalueChangers.ToImmutableArray();

        GameValueChangerRepository(IEnumerable<IGameValueChangerGenerator> gameValueChangerGenerators)
        {
            _gameValueChangerGenerators = gameValueChangerGenerators;
        }

        public void EarlyLoad()
        {
            _gamevalueChangers.Clear();
            foreach (var gameValueChangerGenerator in _gameValueChangerGenerators)
            {
                _gamevalueChangers.AddRange(gameValueChangerGenerator.Generate());
            }
        }
    }
}