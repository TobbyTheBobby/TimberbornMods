using Timberborn.Common;
using UnityEngine;

namespace GlobalMarket
{
    public class RandomAirDestinationPicker
    {
        private readonly IRandomNumberGenerator _randomNumberGenerator;

        private readonly int _randomizeMinAndMax = 9;

        private readonly int _heightMin = 9;

        private readonly int _heightSpread = 4;

        RandomAirDestinationPicker(IRandomNumberGenerator randomNumberGenerator)
        {
            _randomNumberGenerator = randomNumberGenerator;
        }

        public Vector3 RandomAirDestination(Vector3 coordinate)
        {
            var offset = new Vector3(
                _randomNumberGenerator.Range(-_randomizeMinAndMax, _randomizeMinAndMax), 
                _randomNumberGenerator.Range(_heightMin, _heightMin + _heightSpread), 
                _randomNumberGenerator.Range(-_randomizeMinAndMax, _randomizeMinAndMax));
            
            return coordinate + offset;
        }
        
        public Vector3 RandomHeightAirDestination(Vector3 coordinate)
        {
            var offset = new Vector3(0, _randomNumberGenerator.Range(_heightMin, _heightMin + _heightSpread), 0);
            
            return coordinate + offset;
        }
    }
}
