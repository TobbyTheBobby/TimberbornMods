using System.Collections.Immutable;
using System.Linq;
using TimberApi.Common.SingletonSystem;
using TimberApi.SpecificationSystem;

namespace DifficultySettingsChanger
{
    public class GameValueSpecificationRepository : IEarlyLoadableSingleton
    {
        private readonly GameValuesSpecificationDeserializer _gameValuesSpecificationDeserializer;
        private readonly IApiSpecificationService _apiSpecificationService;

        private ImmutableArray<GameValueSpecification> _gameValueSpecifications;

        public ImmutableArray<GameValueSpecification> GameValueSpecifications => _gameValueSpecifications;

        GameValueSpecificationRepository(GameValuesSpecificationDeserializer gameValuesSpecificationDeserializer, IApiSpecificationService apiSpecificationService)
        {
            _gameValuesSpecificationDeserializer = gameValuesSpecificationDeserializer;
            _apiSpecificationService = apiSpecificationService;
        }
        
        public void EarlyLoad()
        {
            var gameValuesSpecification = _apiSpecificationService.GetSpecifications(_gameValuesSpecificationDeserializer).FirstOrDefault();

            if (gameValuesSpecification == null)
            {
                _gameValueSpecifications = new ImmutableArray<GameValueSpecification>();
                return;
            }
            
            _gameValueSpecifications = gameValuesSpecification.GameValueSpecifications.ToImmutableArray();
        }
    }
}