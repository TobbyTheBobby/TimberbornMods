using System.Collections.Generic;
using Timberborn.Common;
using UnityEngine;

namespace GlobalMarket
{
    public class FlightNavigationService
    {
        private readonly RandomAirDestinationPicker _randomAirDestinationPicker;

        private readonly IRandomNumberGenerator _randomNumberGenerator;

        private int PathLength => _randomNumberGenerator.Range(0, 5);
        
        FlightNavigationService(RandomAirDestinationPicker randomAirDestinationPicker, IRandomNumberGenerator randomNumberGenerator)
        {
            _randomAirDestinationPicker = randomAirDestinationPicker;
            _randomNumberGenerator = randomNumberGenerator;
        }
        
        public bool GenerateRandomFlyingPath(Vector3 start, Vector3 destination, List<Vector3> _tempPathCorners)
        {
            _tempPathCorners.Add(start);
            // for (int i = 0; i < PathLength; i++)
            // {
            //     _randomAirDestinationPicker.RandomAirDestination(start);
            // }
            _tempPathCorners.Add(destination);
            return true;
        }
    }
}
