using System;
using System.Collections.Generic;
using Bindito.Core;
using Timberborn.BaseComponentSystem;
using Timberborn.BlockObjectModelSystem;
using Timberborn.BlockSystem;
using Timberborn.PathSystem;
using Timberborn.WorldPersistence;
using UnityEngine;

namespace Ladder
{
    public class Ladder : BaseComponent, IModelUpdater, IPersistentEntity
    {
        private BlockService _blockService;
        
        private BlockObject _blockObject;

        private DrivewayModel _drivewayModel;
        private SecondDrivewayModel _secondDrivewayModel;
        private ThirdDrivewayModel _thirdDrivewayModel;
        
        private readonly List<DrivewayModelSpec> _drivewayModelSpecs = new();

        private Transform _drivewaysContainer;

        private bool _initialized;
        
        private bool Initialized => _initialized || Initialize();

        [Inject]
        public void InjectDependencies(BlockService blockService)
        {
            _blockService = blockService;
        }
        
        public void Save(IEntitySaver entitySaver)
        {
            
        }

        public void Load(IEntityLoader entityLoader)
        {
            var blockObjectModelController = GetComponentFast<BlockObjectModelController>();
            blockObjectModelController._modelUpdaters.Remove(this);
            blockObjectModelController._modelUpdaters.Add(this);
        }

        private bool Initialize()
        {
            try
            {
                _blockObject = GetComponentFast<BlockObject>();
                if (_blockObject == null)
                    return false;
                
                _drivewayModel = GetComponentFast<DrivewayModel>();
                _secondDrivewayModel = GetComponentFast<SecondDrivewayModel>();
                _thirdDrivewayModel = GetComponentFast<ThirdDrivewayModel>();

                GetComponentsFast(_drivewayModelSpecs);

                OverwriteDrivewayModelSpecAndValidated(_drivewayModel, _drivewayModelSpecs[0]);
                OverwriteDrivewayModelSpecAndValidated(_secondDrivewayModel, _drivewayModelSpecs[1]);
                OverwriteDrivewayModelSpecAndValidated(_thirdDrivewayModel, _drivewayModelSpecs[2]);
            }
            catch (Exception)
            {
                return false;
            }

            _initialized = true;
            return true;
        }
        
        public void UpdateModel()
        {
            if (!Initialized)
                return;

            if (_blockService.GetPathObjectAt(_blockObject.Coordinates) == null)
                return;
            
            if (_drivewayModel._model != null) 
                _drivewayModel._model.SetActive(false);
            if (_secondDrivewayModel._model != null) 
                _secondDrivewayModel._model.SetActive(false);
            if (_thirdDrivewayModel._model != null) 
                _thirdDrivewayModel._model.SetActive(false);
        }
        
        private void OverwriteDrivewayModelSpecAndValidated(DrivewayModel drivewayModel, DrivewayModelSpec drivewayModelSpec)
        {
            drivewayModel._drivewayModelSpec = drivewayModelSpec;
            drivewayModel.ValidateDriveway();
        }
    }
}