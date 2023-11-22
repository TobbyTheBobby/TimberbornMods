using UnityEngine;

namespace ChooChoo
{
    public class SlowdownCalculator : MonoBehaviour
    {
        public float slowDownDistance;
        public float baseSlowDownSpeed;
        
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
            
            float end = CalculateEnd();
            
            return start * end;
        }
    
        private float CalculateStart()
        {
            var distanceFromStart = Vector3.Distance(transform.position, _startLocation);
            if (distanceFromStart > slowDownDistance)
            {
                _shouldCalculateStart = false;
                return 1;
            }
            
            return distanceFromStart / slowDownDistance + baseSlowDownSpeed;
        }
        
        private float CalculateEnd()
        {
            var distanceFromEnd = Vector3.Distance(transform.position, _endLocation);
            if (distanceFromEnd > slowDownDistance)
                return 1;

            return distanceFromEnd / slowDownDistance + baseSlowDownSpeed;
        }
    }
}