using Bindito.Core;
using Timberborn.GameDistricts;
using Timberborn.SingletonSystem;
using Timberborn.TickSystem;

namespace Unstuckify
{
    public class UnstuckifyComponent : TickableComponent
    {
        private UnstuckifyService _unstuckifyService;
        private UnstuckifySetting _unstuckifySetting;
        private EventBus _eventBus;
        
        private Citizen _citizen;
        
        [Inject]
        public void InjectDependencies(UnstuckifyService unstuckifyService, UnstuckifySetting unstuckifySetting, EventBus eventBus)
        {
            _unstuckifyService = unstuckifyService;
            _unstuckifySetting = unstuckifySetting;
            _eventBus = eventBus;
        }

        private void Awake()
        {
            _citizen = GetComponentFast<Citizen>();
            enabled = _unstuckifySetting.AutomaticUnstukcifySetting.AutomaticUnstuckifyEnabled;
            _eventBus.Register(this);
        }
        
        [OnEvent]
        public void OnUnstuckifySettingChangedEvent(UnstuckifySettingChangedEvent unstuckifySettingChangedEvent)
        {
            enabled = _unstuckifySetting.AutomaticUnstukcifySetting.AutomaticUnstuckifyEnabled;
        }

        public override void Tick()
        {
            if (enabled && !_citizen.HasAssignedDistrict)
            {
                _unstuckifyService.Unstuckify(_citizen);
            }
        }
    }
}