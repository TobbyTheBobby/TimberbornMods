using Bindito.Core;
using Timberborn.BaseComponentSystem;
using Timberborn.ConstructibleSystem;
using Timberborn.EntitySystem;

namespace ChooChoo
{
    public class TrainDestination : BaseComponent, IFinishedStateListener, IRegisteredComponent
    {
        private TrainDestinationsRepository _trainDestinationsRepository;

        [Inject]
        public void InjectDependencies(TrainDestinationsRepository trainDestinationsRepository)
        {
            _trainDestinationsRepository = trainDestinationsRepository;
        }

        public void OnEnterFinishedState()
        {
            _trainDestinationsRepository.Register(this);
        }

        public void OnExitFinishedState()
        {
            _trainDestinationsRepository.UnRegister(this);
        }
    }
}