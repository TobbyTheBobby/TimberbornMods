using Bindito.Core;
using Timberborn.EntitySystem;
using Timberborn.SingletonSystem;
using Timberborn.TickSystem;

namespace ChooChoo
{
    public class TrackObserver : 
        TickableComponent,
        IInitializableEntity,
        IDeletableEntity
    {
        private EventBus _eventBus;

        private TrackSectionOccupier _trackSectionOccupier;

        private Machinist _machinist;

        private bool _tracksUpdated = true;
        
        [Inject]
        public void InjectDependencies(EventBus eventBus)
        {
            _eventBus = eventBus;
        }

        private void Awake()
        {
            _machinist = GetComponentFast<Machinist>();
            _trackSectionOccupier = GetComponentFast<TrackSectionOccupier>();
        }

        public new void Start()
        {
            _machinist.RefreshPath();
        }

        public override void Tick()
        {
            if (!_tracksUpdated) 
                return;
            _machinist.RefreshPath();
            _trackSectionOccupier.OccupyCurrentTrackSection();
            _tracksUpdated = false;
        }

        [OnEvent]
        public void OnTrackUpdate(OnTracksUpdatedEvent onTracksUpdatedEvent)
        {
            _tracksUpdated = true;
        }

        public void InitializeEntity()
        {
            _eventBus.Register(this);
        }

        public void DeleteEntity()
        {
            _eventBus.Unregister(this);
        }
    }
}