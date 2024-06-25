using Timberborn.BaseComponentSystem;
using UnityEngine;
using UnityEngine.Serialization;

namespace ChooChoo.MovementSystem
{
    public class SlowdownCalculator : BaseComponent
    {
        [FormerlySerializedAs("slowDownDistance")]
        public float _slowDownDistance;

        [FormerlySerializedAs("baseSlowDownSpeed")]
        public float _baseSlowDownSpeed;

        private Vector3 _startLocation;
        private Vector3 _endLocation;

        private bool _shouldCalculateStart;

        public void SetPositions(Vector3 startLocation, Vector3 endLocation)
        {
            _startLocation = startLocation;
            _endLocation = endLocation;

            _shouldCalculateStart = true;
        }

        public float CalculateSlowdown()
        {
            float start = 1;

            if (_shouldCalculateStart)
                start = CalculateStart();

            var end = CalculateEnd();

            return start * end;
        }

        private float CalculateStart()
        {
            var distanceFromStart = Vector3.Distance(TransformFast.position, _startLocation);
            if (distanceFromStart > _slowDownDistance)
            {
                _shouldCalculateStart = false;
                return 1;
            }

            return distanceFromStart / _slowDownDistance + _baseSlowDownSpeed;
        }

        private float CalculateEnd()
        {
            var distanceFromEnd = Vector3.Distance(TransformFast.position, _endLocation);
            if (distanceFromEnd > _slowDownDistance)
                return 1;

            return distanceFromEnd / _slowDownDistance + _baseSlowDownSpeed;
        }
    }
}