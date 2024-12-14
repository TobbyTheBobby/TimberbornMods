using System;
using Bindito.Core;
using ChooChoo.Extensions;
using ChooChoo.MovementSystem;
using ChooChoo.NavigationSystem;
using Timberborn.BehaviorSystem;
using Timberborn.BlockSystem;

namespace ChooChoo.TrainYards
{
    public class ReturnToTrainYardBehavior : RootBehavior
    {
        private BlockService _blockService;
        private MoveToStationExecutor _moveToStationExecutor;
        private WaitExecutor _waitExecutor;
        private TrainYardSubject _trainYardSubject;

        [Inject]
        public void InjectDependencies(BlockService blockService)
        {
            _blockService = blockService;
        }

        public void Awake()
        {
            _moveToStationExecutor = GetComponentFast<MoveToStationExecutor>();
            _waitExecutor = GetComponentFast<WaitExecutor>();
            _trainYardSubject = GetComponentFast<TrainYardSubject>();
        }

        public override Decision Decide(BehaviorAgent agent)
        {
            var currentTrainDestination = _blockService.GetBottomObjectComponentAt<TrainDestination>(TransformFast.position.ToBlockServicePosition());
            if (currentTrainDestination == _trainYardSubject.HomeTrainYard)
            {
                _waitExecutor.LaunchForSpecifiedTime(1);
                return Decision.ReleaseWhenFinished(_waitExecutor);
            }

            return ReturnToOriginalTrainYard();
        }

        private Decision ReturnToOriginalTrainYard()
        {
            // Plugin.Log.LogWarning( "Returning Home");
            switch (_moveToStationExecutor.Launch(_trainYardSubject.HomeTrainYard))
            {
                case ExecutorStatus.Success:
                    return Decision.ReleaseNow();
                case ExecutorStatus.Failure:
                    return Decision.ReleaseNow();
                case ExecutorStatus.Running:
                    return Decision.ReturnWhenFinished(_moveToStationExecutor);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}