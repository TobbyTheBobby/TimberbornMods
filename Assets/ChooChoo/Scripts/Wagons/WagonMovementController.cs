using System.Collections.Generic;
using UnityEngine;

namespace ChooChoo
{
    public class WagonMovementController : MonoBehaviour
    {
        private WagonManager _wagonManager;
        
        private void Awake()
        {
            _wagonManager = GetComponent<WagonManager>();
        }
        
        public void SetNewPathConnections(ITrackFollower trackFollower, List<TrackRoute> pathConnections)
        {
            var trainWagons = _wagonManager.Wagons;
            trainWagons[0].StartMoving(trackFollower, pathConnections);
            for (var index = 1; index < trainWagons.Count; index++)
            {
                var trainWagon = trainWagons[index];
                trainWagon.StartMoving(trainWagons[index - 1].ObjectFollower, pathConnections);
            }
        }

        public void MoveWagons()
        {
            foreach (var trainWagon in _wagonManager.Wagons)
            {
                trainWagon.Move();
            }
        }
        
        public void StopWagons()
        {
            foreach (var trainWagon in _wagonManager.Wagons)
            {
                trainWagon.GetComponentFast<TrainWagon>().Stop();
            }
        }
    }
}