using ChooChoo.BuildingRegistrySystem;
using Timberborn.BlockSystem;
using Timberborn.Common;
using TobbyTools.BuildingRegistrySystem;
using UnityEngine;

namespace ChooChoo.NavigationSystem
{
    public class RandomTrainDestinationPicker
    {
        private readonly BuildingRegistry<TrainDestination> _trainDestinationRegistry;
        private readonly IRandomNumberGenerator _randomNumberGenerator;

        private RandomTrainDestinationPicker(
            BuildingRegistry<TrainDestination> trainDestinationRegistry, 
            IRandomNumberGenerator randomNumberGenerator)
        {
            _trainDestinationRegistry = trainDestinationRegistry;
            _randomNumberGenerator = randomNumberGenerator;
        }

        public Vector3 RandomTrainDestination()
        {
            var list = _trainDestinationRegistry.All;
            var trainDestination = list[_randomNumberGenerator.Range(0, list.Count)];
            var wrongCoordinate = trainDestination.GetComponentFast<BlockObject>().Coordinates;
            var coordinate = new Vector3(wrongCoordinate.x, wrongCoordinate.z, wrongCoordinate.y);

            return coordinate;
        }
    }
}