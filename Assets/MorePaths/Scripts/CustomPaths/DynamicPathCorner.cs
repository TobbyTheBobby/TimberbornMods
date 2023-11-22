using System.Collections.Generic;
using System.Linq;
using Bindito.Core;
using Timberborn.BaseComponentSystem;
using Timberborn.BlockSystem;
using Timberborn.Buildings;
using Timberborn.Navigation;
using Timberborn.PreviewSystem;
using Timberborn.TerrainSystem;
using UnityEngine;

namespace MorePaths
{
    public class DynamicPathCorner : BaseComponent, IModelUpdater
    {
        private BuildingModelUpdater _buildingModelUpdater;
        private PreviewBlockService _previewBlockService;
        private ITerrainService _terrainService;
        private MorePathsCore _morePathsCore;
        private BlockService _blockService;
        
        private GameObject _cornerDownLeft;
        private GameObject _cornerDownRight;
        private GameObject _cornerUpLeft;
        private GameObject _cornerUpRight;

        private BlockObject _blockObject;

        private readonly List<List<Vector3Int>> _neighboringCoordinates = new()
        {
            new List<Vector3Int>
            {
                new(0, -1 , 0),
                new(-1, -1 , 0),
                new(-1, 0 , 0),
            },
            new List<Vector3Int>
            {
                new(-1, 0 , 0),
                new(-1, 1 , 0),
                new(0, 1 , 0),
            },
            new List<Vector3Int>
            {
                new(0, 1 , 0),
                new(1, 1 , 0),
                new(1, 0 , 0),
            },
            new List<Vector3Int>
            {
                new(1, 0 , 0),
                new(1, -1 , 0),
                new(0, -1 , 0),
            },
        };
        
        [Inject]
        public void InjectDependencies(BuildingModelUpdater buildingModelUpdater, PreviewBlockService previewBlockService, ITerrainService terrainService, MorePathsCore morePathsCore, BlockService blockService)
        {
            _buildingModelUpdater = buildingModelUpdater;
            _previewBlockService = previewBlockService;
            _terrainService = terrainService;
            _blockService = blockService;
        }
        
        void Awake()
        {
            _blockObject = GetComponentFast<BlockObject>();
        }

        public void CreatePathCorners(GameObject pathCorner)
        {
            var corner1 = Instantiate(pathCorner, TransformFast);
            corner1.name = "corner1";
            _cornerDownLeft = corner1;
            corner1.SetActive(false);
            
            var corner2 = Instantiate(pathCorner, TransformFast, true);            
            corner2.transform.position += new Vector3(0, 0, 0.75f);
            corner2.name = "corner2";
            _cornerUpLeft = corner2;
            corner2.SetActive(false);

            var corner3 = Instantiate(pathCorner, TransformFast, true);
            corner3.transform.position += new Vector3(0.75f, 0, 0.75f);
            corner3.name = "corner3";
            _cornerUpRight = corner3;
            corner3.SetActive(false);

            var corner4 = Instantiate(pathCorner, TransformFast, true);
            corner4.transform.position += new Vector3(0.75f, 0, 0);
            corner4.name = "corner4";
            _cornerDownRight = corner4;
            corner4.SetActive(false);
        }

        public void UpdateModel()
        {
            if (!isActiveAndEnabled)
                return;

            foreach (var (quadrantList, i) in _neighboringCoordinates.Select((value, i) => ( value, i )))
            {
                var coords1 = _blockObject.Transform(quadrantList[0]);
                var obj1 = _terrainService.OnGround(coords1) ? AnythingOnFloor(coords1) : null;
                var coords2 = _blockObject.Transform(quadrantList[1]);
                var obj2 = _terrainService.OnGround(coords2) ? AnythingOnFloor(coords2) : null;
                var coords3 = _blockObject.Transform(quadrantList[2]);
                var obj3 = _terrainService.OnGround(coords3) ? AnythingOnFloor(coords3) : null;

                var flag = obj1 != null && obj2 != null && obj3 != null;
                
                // if (obj2 != null && obj2.TryGetComponentFast(out IModelUpdater modelUpdater))
                //     modelUpdater.UpdateModel();

                switch (i)
                {
                    case 0:
                        _cornerDownLeft.SetActive(flag);
                        break;
                    case 1:
                        _cornerUpLeft.SetActive(flag);
                        break;
                    case 2:
                        _cornerUpRight.SetActive(flag);
                        break;
                    case 3:
                        _cornerDownRight.SetActive(flag);
                        break;
                }
            }
        }

        private DynamicPathCorner AnythingOnFloor(Vector3Int coords)
        {
            var realObjectOnFloor = _blockService.GetFloorObjectComponentAt<DynamicPathCorner>(coords);
            if (realObjectOnFloor)
            {
                return realObjectOnFloor;
            }
            
            var previewObjectOnFloor = _previewBlockService.GetFloorObjectComponentAt<DynamicPathCorner>(coords);
            if (previewObjectOnFloor)
            {
                return previewObjectOnFloor;
            }

            return null;
        }
    }
}
