using System.Collections.Generic;
using System.Linq;
using Bindito.Core;
using Timberborn.BaseComponentSystem;
using Timberborn.PathSystem;

namespace Ladder
{
    public class MultiDrivewayModelFixer : BaseComponent
    {
        private BaseInstantiator _baseInstantiator;

        private List<DrivewayModelSpec> _drivewayModelSpecs = new();
        private List<DrivewayModel> _drivewayModels = new();

        [Inject]
        public void InjectDependencies(BaseInstantiator baseInstantiator)
        {
            _baseInstantiator = baseInstantiator;
        }

        private void Awake()
        {
            GetComponentsFast(_drivewayModelSpecs);
            GetComponentsFast(_drivewayModels);
            
            GameObjectFast.SetActive(false);
            for (var i = 0; i < _drivewayModelSpecs.Count - _drivewayModels.Count; i++)
            {
                _baseInstantiator.AddComponent<DrivewayModel>(GameObjectFast);
            }
            GameObjectFast.SetActive(true);
            
            GetComponentsFast(_drivewayModelSpecs);
            GetComponentsFast(_drivewayModels);
            
            _drivewayModelSpecs = _drivewayModelSpecs.Distinct().ToList();
            _drivewayModels = _drivewayModels.Distinct().ToList();

            for (var i = 0; i < _drivewayModelSpecs.Count; i++)
            {
                var drivewayModel = _drivewayModels[i];
                drivewayModel._drivewayModelSpec = _drivewayModelSpecs[i];
                drivewayModel.ValidateDriveway();
                Destroy(drivewayModel._model);
                drivewayModel._model = drivewayModel._drivewayModelInstantiator.InstantiateModel(drivewayModel, drivewayModel.GetLocalCoordinates(), drivewayModel.GetLocalDirection());
            }
        }
    }
}