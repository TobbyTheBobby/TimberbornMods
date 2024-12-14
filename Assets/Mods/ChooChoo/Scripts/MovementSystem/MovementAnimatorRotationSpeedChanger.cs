using ChooChoo.InaccessibilityUtilitySystem;
using Timberborn.BaseComponentSystem;
using Timberborn.CharacterMovementSystem;
using UnityEngine;
using UnityEngine.Serialization;

namespace ChooChoo.MovementSystem
{
    public class MovementAnimatorRotationSpeedChanger : BaseComponent
    {
        [FormerlySerializedAs("YRotationSpeedMultiplier")]
        [SerializeField]
        private float _yRotationSpeedMultiplier;

        private void Awake()
        {
            var movementAnimator = GetComponentFast<MovementAnimator>();
            InaccessibilityUtilities.SetInaccessibleField(movementAnimator, "YRotationSpeed", 360 * _yRotationSpeedMultiplier);
        }
    }
}