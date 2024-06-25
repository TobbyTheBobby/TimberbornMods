using Timberborn.TickSystem;
using UnityEngine;

namespace ChooChoo.Trains
{
    public class WindServiceParticleInterface : TickableComponent
    {
        [SerializeField]
        public ParticleSystemForceField particleForceField;
        private Vector3 PrefabRotation => TransformFast.rotation.eulerAngles;
        private readonly float _strengthFactor = 0.1f;

        public override void Tick()
        {
            if (!isActiveAndEnabled)
                return;

            var smokeVector = RotateVector(StaticWindService.WindService.WindDirection, -PrefabRotation.y);
            particleForceField.directionX = smokeVector.x * _strengthFactor;
            particleForceField.directionY = smokeVector.y * _strengthFactor;
        }

        private static Vector2 RotateVector(Vector2 v, float degrees)
        {
            var radians = degrees * Mathf.Deg2Rad;
            return new Vector2(
                v.x * Mathf.Cos(radians) - v.y * Mathf.Sin(radians),
                v.x * Mathf.Sin(radians) + v.y * Mathf.Cos(radians)
            );
        }
    }
}