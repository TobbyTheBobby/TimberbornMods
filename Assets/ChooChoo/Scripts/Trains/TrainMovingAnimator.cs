using Bindito.Core;
using Timberborn.CharacterModelSystem;
using Timberborn.Common;
using Timberborn.TickSystem;

namespace ChooChoo
{
    public class TrainMovingAnimator : TickableComponent
    {
        private IRandomNumberGenerator _randomNumberGenerator;
        private CharacterAnimator _characterAnimator;
        private bool _isFlying;

        [Inject]
        public void InjectDependencies(IRandomNumberGenerator randomNumberGenerator)
        {
            _randomNumberGenerator = randomNumberGenerator;
        }

        void Awake()
        {
            // _characterAnimator = GetComponentFast<CharacterAnimator>();
            // _characterAnimator.SetFloat("MovementSpeed", 1);
            // _flyingRootBehavior = GetComponent<FlyingRootBehavior>();
        }

        public override void Tick()
        {
            // if (_flyingRootBehavior.IsReturned)
            // {
            //     _characterAnimator.SetFloat("FlyingSpeed", 0);
            // }
            // else
            // {
            //     var speed = _randomNumberGenerator.Range(0.9f, 1.1f);
            //     _characterAnimator.SetFloat("FlyingSpeed", speed);
            // }
        }
    }
}