using Bindito.Core;
using ChooChoo.NavigationSystem;
using Timberborn.BaseComponentSystem;
using Timberborn.BehaviorSystem;
using Timberborn.Persistence;

namespace ChooChoo.MovementSystem
{
    public class MoveToStationExecutor : BaseComponent, IExecutor
    {
        private TrainPositionDestinationFactory _trainPositionDestinationFactory;

        private Machinist _machinist;

        [Inject]
        public void InjectDependencies(TrainPositionDestinationFactory trainPositionDestinationFactory)
        {
            _trainPositionDestinationFactory = trainPositionDestinationFactory;
        }

        public void Awake()
        {
            _machinist = GetComponentFast<Machinist>();
        }

        public ExecutorStatus Launch(TrainDestination trainDestination)
        {
            return _machinist.GoTo(_trainPositionDestinationFactory.Create(trainDestination));
        }

        public ExecutorStatus Tick(float deltaTimeInHours)
        {
            if (!_machinist.CurrentDestinationReachable)
                return ExecutorStatus.Failure;
            return _machinist.Stopped() ? ExecutorStatus.Success : ExecutorStatus.Running;
        }

        public void Save(IEntitySaver entitySaver)
        {
        }

        public void Load(IEntityLoader entityLoader)
        {
        }
    }
}