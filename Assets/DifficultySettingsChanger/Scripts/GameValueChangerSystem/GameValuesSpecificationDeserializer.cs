using System;
using Timberborn.Persistence;

namespace DifficultySettingsChanger
{
    public class GameValuesSpecificationDeserializer : IObjectSerializer<GameValuesSpecification>
    {
        private readonly GameValueSpecificationDeserializer _gameValueSpecificationDeserializer;
        
        GameValuesSpecificationDeserializer(GameValueSpecificationDeserializer gameValueSpecificationDeserializer)
        {
            _gameValueSpecificationDeserializer = gameValueSpecificationDeserializer;
        }
        
        public void Serialize(GameValuesSpecification value, IObjectSaver objectSaver)
        {
            throw new NotImplementedException();
        }

        public Obsoletable<GameValuesSpecification> Deserialize(IObjectLoader objectLoader)
        {
            return (Obsoletable<GameValuesSpecification>)new GameValuesSpecification(objectLoader.Get(new ListKey<GameValueSpecification>("GameValueSpecifications"), _gameValueSpecificationDeserializer));
        }
    }
}
