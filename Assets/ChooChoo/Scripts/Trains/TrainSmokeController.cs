using System.Collections.Generic;
using System.Linq;
using Bindito.Core;
using Timberborn.BehaviorSystem;
using Timberborn.TickSystem;
using Timberborn.TimeSystem;
using UnityEngine;

namespace ChooChoo
{
    public class TrainSmokeController : TickableComponent
    {
        private IEnumerable<GameObject> _smokeObjects;

        private IDayNightCycle _dayNightCycle;
        
        private WaitExecutor _waitExecutor;

        [Inject]
        public void InjectDependencies(IDayNightCycle dayNightCycles)
        {
            _dayNightCycle = dayNightCycles;
        }

        public override void StartTickable()
        {
            _waitExecutor = GetComponentFast<WaitExecutor>();
            _smokeObjects = ChooChooCore.FindAllBodyParts(TransformFast, "Smoke").Select(transformComponent => transformComponent.gameObject);
        }

        public override void Tick()
        {
            var active = !IsWaiting();
            foreach (var smokeObject in _smokeObjects)
                smokeObject.SetActive(active);
        }

        private bool IsWaiting()
        {
            return !(_dayNightCycle.PartialDayNumber > (float)ChooChooCore.GetInaccessibleField(_waitExecutor, "_finishTimestamp"));
        }
    }
}