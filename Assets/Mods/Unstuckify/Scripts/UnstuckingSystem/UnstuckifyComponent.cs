using Bindito.Core;
using Timberborn.GameDistricts;
using Timberborn.TickSystem;
using Unstuckify.SettingsSystem;

namespace Unstuckify.UnstuckingSystem
{
    public class UnstuckifyComponent : TickableComponent
    {
        private UnstuckifySettingsOwner _unstuckifySettingsOwner;
        private UnstuckifyService _unstuckifyService;
        
        private Citizen _citizen;
        
        [Inject]
        public void InjectDependencies(UnstuckifySettingsOwner unstuckifySettingsOwner, UnstuckifyService unstuckifyService)
        {
            _unstuckifySettingsOwner = unstuckifySettingsOwner;
            _unstuckifyService = unstuckifyService;
        }

        private void Awake()
        {
            _citizen = GetComponentFast<Citizen>();
            enabled = _unstuckifySettingsOwner.UnstuckifyEnabledSetting.Value;
            _unstuckifySettingsOwner.UnstuckifyEnabledSetting.ValueChanged += OnUnstuckifySettingChanged;
        }

        private void OnUnstuckifySettingChanged(object sender, bool e)
        {
            enabled = _unstuckifySettingsOwner.UnstuckifyEnabledSetting.Value;
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