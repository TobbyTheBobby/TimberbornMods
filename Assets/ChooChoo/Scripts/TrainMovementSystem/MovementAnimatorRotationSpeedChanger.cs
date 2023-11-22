using Timberborn.BaseComponentSystem;
using Timberborn.CharacterMovementSystem;
using UnityEngine;

namespace ChooChoo
{
    public class MovementAnimatorRotationSpeedChanger : BaseComponent
    {
        [SerializeField] 
        private float YRotationSpeedMultiplier;
        
        private void Awake()
        {
            var movementAnimator = GetComponentFast<MovementAnimator>();
            ChooChooCore.SetInaccessibleField(movementAnimator, "YRotationSpeed", 360 * YRotationSpeedMultiplier);
        }
    }
}