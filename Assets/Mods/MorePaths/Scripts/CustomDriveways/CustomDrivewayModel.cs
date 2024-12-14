using System.Collections.Generic;
using Bindito.Core;
using MorePaths.Core;
using Timberborn.BaseComponentSystem;
using Timberborn.BlockObjectModelSystem;
using Timberborn.BlockSystem;
using Timberborn.Common;
using Timberborn.Coordinates;
using Timberborn.PathSystem;
using Timberborn.TerrainSystem;
using UnityEngine;

namespace MorePaths.CustomDriveways
{
  public class CustomDrivewayModel : BaseComponent, IModelUpdater
  {
      private PreviewBlockService _previewBlockService;
      private IConnectionService _connectionService;
      private ITerrainService _terrainService;
      private BlockService _blockService;
      
      private readonly Dictionary<DrivewayModel, List<GameObject>> _drivewayModelModels = new ();
      private readonly Dictionary<DrivewayModel, GameObject> _baseGameDrivewayModels = new();
      private readonly List<DrivewayModel> _drivewayModels = new();
        
      [Inject]
      public void InjectDependencies(PreviewBlockService previewBlockService, IConnectionService connectionService, ITerrainService terrainService, BlockService blockService)
      {
          _previewBlockService = previewBlockService;
          _connectionService = connectionService;
          _terrainService = terrainService;
          _blockService = blockService;
      }

      private void Start()
      {
          GetComponentsFast(_drivewayModels);
          InstantiateDriveways();
      }

      private void InstantiateDriveways()
      {
          foreach (var drivewayModel in _drivewayModels)
          {
              var localCoordinates = drivewayModel.GetLocalCoordinates();
              var localDirection = drivewayModel.GetLocalDirection();
            
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
          return _baseGameDrivewayModels.GetOrAdd(drivewayModel, () => drivewayModel._model);
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
          var path = GetPath(drivewayModel);
      
          var positionedDirection = drivewayModel.GetPositionedDirection();
          var positionedCoordinates = drivewayModel.GetPositionedCoordinates();
          
          var onGround = _terrainService.OnGround(positionedCoordinates);

          foreach (var pathObject in DrivewayService.DriveWays[drivewayModel.Driveway])
          {
              if (path != null)
              {
                  if (path.name.Replace("(Clone)", "") != pathObject.name) 
                      continue;
                  
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
                          bool flag2 = path;
                          var enabled = flag1 && flag2 && onGround;
                          tempModel.SetActive(enabled);
                      }
                  }
              }
              else
              {
                  GetBaseDrivewayModel(drivewayModel).SetActive(_connectionService.CanConnectInDirection(positionedCoordinates, positionedDirection) && onGround);

                  foreach (var tempModel in _drivewayModelModels[drivewayModel])
                  { 
                      tempModel.SetActive(false);
                  }
              }
          }
      }
      
      private GameObject GetPath(DrivewayModel drivewayModel)
      {
          var direction = drivewayModel.GetPositionedDirection();
          var coordinates = drivewayModel.GetPositionedCoordinates();
      
          var checkObjectCoordinates = coordinates + direction.ToOffset();
          var path = _blockService.GetPathObjectAt(checkObjectCoordinates);

          if (path != null)
              return path.GameObjectFast;

          // var previewPath = _previewBlockService.GetBottomObjectComponentAt<DynamicPathModel>(checkObjectCoordinates);
          // return previewPath != null ? previewPath.GameObjectFast : null;
          
          return null;
      }
  }
}
