// using Bindito.Core;
// using GlobalMarket;
// using Timberborn.CharacterModelSystem;
// using Timberborn.Common;
// using Timberborn.TickSystem;
//
// public class AirBalloonFlyingAnimator : TickableComponent
// {
//     private IRandomNumberGenerator _randomNumberGenerator;
//     private CharacterAnimator _characterAnimator;
//     private FlyingRootBehavior _flyingRootBehavior;
//     private bool _isFlying;
//         
//     [Inject]
//     public void InjectDependencies(IRandomNumberGenerator randomNumberGenerator)
//     {
//         _randomNumberGenerator = randomNumberGenerator;
//     }
//     
//     void Awake()
//     {
//         _characterAnimator = GetComponent<CharacterAnimator>();
//         _flyingRootBehavior = GetComponent<FlyingRootBehavior>();
//     }
//
//     public override void Tick()
//     {
//         if (_flyingRootBehavior.IsReturned)
//         {
//             _characterAnimator.SetFloat("FlyingSpeed", 0);
//         }
//         else
//         {
//             var speed = _randomNumberGenerator.Range(0.9f, 1.1f);
//             _characterAnimator.SetFloat("FlyingSpeed", speed);
//         }
//     }
// }
