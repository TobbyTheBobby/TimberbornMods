using System.Linq;
using Bindito.Core;
using Timberborn.BaseComponentSystem;
using Timberborn.BlockObjectModelSystem;
using Timberborn.BlockSystem;
using Timberborn.Buildings;
using Timberborn.Coordinates;
using Timberborn.SingletonSystem;
using Timberborn.TerrainSystem;
using UnityEngine;

namespace MorePlatforms
{
    public class DirectionalConnector : BaseComponent, IModelUpdater
    {
        [SerializeField]
        private GameObject ModelUp; 
        [SerializeField]
        private GameObject ModelDown; 
        [SerializeField]
        private GameObject ModelRight; 
        [SerializeField]
        private GameObject ModelLeft;
        [SerializeField]
        private GameObject ModelUpDown;
        [SerializeField]
        private GameObject ModelLeftRight;

        private BuildingModelUpdater _buildingModelUpdater;
        private PreviewBlockService _previewBlockService;
        private ITerrainService _terrainService;
        private BlockService _blockService;
        private EventBus _eventBus;

        private BlockObject _blockObject;
    
        [Inject]
        public void InjectDependencies(BuildingModelUpdater buildingModelUpdater, PreviewBlockService previewBlockService, ITerrainService terrainService, BlockService blockService, EventBus eventBus)
        {
            _buildingModelUpdater = buildingModelUpdater;
            _previewBlockService = previewBlockService;
            _terrainService = terrainService;
            _blockService = blockService;
            _eventBus = eventBus;
        }

        private void Awake()
        {
            _blockObject = GetComponentFast<BlockObject>();
        }

        private void Start()
        {
            _eventBus.Register(this);
        }

        public void UpdateModel()
        {
            var upOccupied = CoordinatesAreOccupied(Direction2D.Up);
            var downOccupied = CoordinatesAreOccupied(Direction2D.Down);
            var rightOccupied = CoordinatesAreOccupied(Direction2D.Right);
            var leftOccupied = CoordinatesAreOccupied(Direction2D.Left);
        
            ModelUp.SetActive(upOccupied);
            ModelDown.SetActive(downOccupied);
            ModelRight.SetActive(rightOccupied);
            ModelLeft.SetActive(leftOccupied);
        
            ModelUpDown.SetActive(upOccupied && downOccupied);
            ModelLeftRight.SetActive(rightOccupied && leftOccupied);
        }
    
        [OnEvent]
        public void OnBlockObjectSetEvent(BlockObjectSetEvent blockObjectSetEvent)
        {
            if (blockObjectSetEvent.BlockObject != _blockObject)
                return;

            var upCoordinates = GetCoordinates(Direction2D.Up);
            var downCoordinates = GetCoordinates(Direction2D.Down);
            var rightCoordinates = GetCoordinates(Direction2D.Right);
            var leftCoordinates = GetCoordinates(Direction2D.Left);

            _buildingModelUpdater.UpdateBuildingModelsAt(upCoordinates);
            _buildingModelUpdater.UpdateBuildingModelsAt(downCoordinates);
            _buildingModelUpdater.UpdateBuildingModelsAt(rightCoordinates);
            _buildingModelUpdater.UpdateBuildingModelsAt(leftCoordinates);
        }

        private bool CoordinatesAreOccupied(Direction2D direction2D)
        {
            var coordinates = GetCoordinates(direction2D);

            return _blockService.GetObjectsAt(coordinates).Any() || _previewBlockService.GetPreviewsAt(coordinates).Any() || _terrainService.Underground(coordinates);
        }

        private Vector3Int GetCoordinates(Direction2D direction2D)
        {
            var origin = _blockObject.PositionedBlocks.GetOccupiedCoordinates().First();
            var direction2D1 = _blockObject.Orientation.Transform(direction2D);
            return origin + direction2D1.ToOffset();
        }
    }
}
