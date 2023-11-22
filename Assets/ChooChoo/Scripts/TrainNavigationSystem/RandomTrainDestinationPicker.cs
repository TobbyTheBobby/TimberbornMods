using Timberborn.BlockSystem;
using Timberborn.Common;
using UnityEngine;

namespace ChooChoo
{
    public class RandomTrainDestinationPicker
    {
        private readonly TrainDestinationsRepository _trainDestinationsRepository;
        
        private readonly IRandomNumberGenerator _randomNumberGenerator;

        private readonly int _heightMin = 9;

        private readonly int _heightSpread = 4;

        RandomTrainDestinationPicker(TrainDestinationsRepository trainDestinationsRepository, IRandomNumberGenerator randomNumberGenerator)
        {
            _trainDestinationsRepository = trainDestinationsRepository;
            _randomNumberGenerator = randomNumberGenerator;
        }

        public Vector3 RandomTrainDestination()
        {
            var list = _trainDestinationsRepository.TrainDestinations;
            var trainDestination = list[_randomNumberGenerator.Range(0, list.Count)];
            var wrongCoordinate = trainDestination.GetComponentFast<BlockObject>().Coordinates;
            var coordinate = new Vector3(wrongCoordinate.x, wrongCoordinate.z, wrongCoordinate.y);
            
            return coordinate;
        }
        
        public Vector3 RandomHeightAirDestination(Vector3 coordinate)
        {
            var offset = new Vector3(0, _randomNumberGenerator.Range(_heightMin, _heightMin + _heightSpread), 0);
            
            return coordinate + offset;
        }
    }
}
