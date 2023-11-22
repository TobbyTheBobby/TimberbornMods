using System.Collections.Generic;
using System.Linq;
using Bindito.Core;
using Timberborn.BaseComponentSystem;
using Timberborn.BlockSystem;
using Timberborn.Buildings;
using Timberborn.Common;
using Timberborn.Coordinates;
using Timberborn.PathSystem;
using Timberborn.PrefabOptimization;
using Timberborn.PreviewSystem;
using Timberborn.TerrainSystem;
using UnityEngine;

namespace MorePaths
{
  public class CustomDrivewayModel : BaseComponent, IModelUpdater
  {
      private PreviewBlockService _previewBlockService;
      private IConnectionService _connectionService;
      private ITerrainService _terrainService;
      private MorePathsCore _morePathsCore;
      private BlockService _blockService;
      
      private readonly Dictionary<DrivewayModel, List<GameObject>> _drivewayModelModels = new ();
      private readonly Dictionary<DrivewayModel, GameObject> _baseGameDrivewayModels = new();
      private readonly List<DrivewayModel> _drivewayModels = new();
        
      [Inject]
      public void InjectDependencies(PreviewBlockService previewBlockService, IConnectionService connectionService, ITerrainService terrainService, MorePathsCore morePathsCore, BlockService blockService)
      {
          _previewBlockService = previewBlockService;
          _connectionService = connectionService;
          _terrainService = terrainService;
          _morePathsCore = morePathsCore;
          _blockService = blockService;
      }

      void Awake()
      {
          var modelUpdaters = (List<IModelUpdater>)_morePathsCore.GetPrivateField(GetComponentFast<BuildingModel>(), "_modelUpdaters");
          modelUpdaters.Add(this);
          GetComponentFast<BuildingModel>().UpdateModel();
          GetComponentsFast(_drivewayModels);
          InstantiateDriveways();
      }

      private void InstantiateDriveways()
      {
          foreach (var drivewayModel in _drivewayModels)
          {
              var localCoordinates = (Vector3Int)_morePathsCore.InvokeInaccesableMethod(drivewayModel, "GetLocalCoordinates");
              var localDirection = (Direction2D)_morePathsCore.InvokeInaccesableMethod(drivewayModel, "GetLocalDirection");
            
              InstantiateModel(drivewayModel, localCoordinates, localDirection, DrivewayService.DriveWays);
          }
      }
      
      private void InstantiateModel(
          DrivewayModel drivewayModel,
          Vector3Int coordinates,
          Direction2D direction,
          Dictionary<Driveway, List<GameObject>> driveways)
      {
          foreach (var prefab in driveways[drivewayModel.Driveway])
          { 
              prefab.SetActive(false); 
              var model = Instantiate(prefab, GetBaseDrivewayModel(drivewayModel).transform.parent, false); 
              model.transform.localPosition = CoordinateSystem.GridToWorld(BlockCalculations.Pivot(coordinates, direction.ToOrientation()));
              model.transform.localRotation = direction.ToWorldSpaceRotation(); 
              model.name = prefab.name; 
              _drivewayModelModels.GetOrAdd(drivewayModel, () => new List<GameObject>()).Add(model);
          }
      }

      private GameObject GetBaseDrivewayModel(DrivewayModel drivewayModel)
      {
          return _baseGameDrivewayModels.GetOrAdd(drivewayModel, () => (GameObject)_morePathsCore.GetPrivateField(drivewayModel, "_model"));
      }
      
      public void UpdateModel()
      {
          foreach (var drivewayModel in _drivewayModels)
          {
              UpdateAllDriveways(drivewayModel);
          }
      }

      private void UpdateAllDriveways(DrivewayModel drivewayModel)
      { 
          GameObject path = GetPath(drivewayModel);
      
          var positionedDirection = (Direction2D)_morePathsCore.InvokeInaccesableMethod(drivewayModel, "GetPositionedDirection");
          var positionedCoordinates = (Vector3Int)_morePathsCore.InvokeInaccesableMethod(drivewayModel, "GetPositionedCoordinates");
          
          bool onGround = _terrainService.OnGround(positionedCoordinates);

          foreach (var pathObject in DrivewayService.DriveWays[drivewayModel.Driveway])
          {
              if (path != null)
              {
                  if (path.name.Replace("(Clone)", "") == pathObject.name)
                  {
                      if (pathObject.name == "Path.Folktails" | pathObject.name == "Path.IronTeeth")
                      {
                          GetBaseDrivewayModel(drivewayModel).SetActive(onGround);
      
                          foreach (var tempModel in _drivewayModelModels[drivewayModel])
                          { 
                               tempModel.SetActive(false);
                          }
                      }
                      else
                      {
                          GetBaseDrivewayModel(drivewayModel).SetActive(false);
      
                          foreach (var tempModel in _drivewayModelModels[drivewayModel])
                          {
                                var flag1 = tempModel.name == path.name.Replace("(Clone)", "");
                                var flag2 = path.GetComponent<BlockObject>().Finished;
                                var enabled = flag1 && flag2 && onGround;
                                tempModel.SetActive(enabled);
                          }
                      }
                  }
              }
              else
              {
                  GetBaseDrivewayModel(drivewayModel).SetActive(_connectionService.CanConnectInDirection(positionedCoordinates, positionedDirection) &&  onGround);

                  foreach (var tempModel in _drivewayModelModels[drivewayModel])
                  { 
                      tempModel.SetActive(false);
                  }
              }
          }
      }
      
      private GameObject GetPath(DrivewayModel drivewayModel)
      {
          var direction = (Direction2D)_morePathsCore.InvokeInaccesableMethod(drivewayModel, "GetPositionedDirection");
          var coordinates = (Vector3Int)_morePathsCore.InvokeInaccesableMethod(drivewayModel, "GetPositionedCoordinates");
      
          Vector3Int checkObjectCoordinates = coordinates + direction.ToOffset();
          var paths = _blockService.GetObjectsWithComponentAt<DynamicPathModel>(checkObjectCoordinates).ToArray();

          if (paths.Any())
              return paths.First().GameObjectFast;

          var previewPath = _previewBlockService.GetFloorObjectComponentAt<DynamicPathModel>(checkObjectCoordinates);
          return previewPath != null ? previewPath.GameObjectFast : null;
      }
  }
}
