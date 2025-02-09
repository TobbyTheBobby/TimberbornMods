using System;
using System.Collections.Generic;
using Timberborn.BlockObjectTools;
using Timberborn.BlockSystem;
using Timberborn.EntitySystem;
using Timberborn.GameFactionSystem;
using Timberborn.PrefabSystem;
using Timberborn.SingletonSystem;
using UnityEngine;

namespace Ladder
{
    public class LadderService : ILoadableSingleton
    {
        public static LadderService Instance;

        private readonly BlockObjectPlacerService _blockObjectPlacerService;
        private readonly PrefabNameMapper _prefabNameMapper;
        private readonly FactionService _factionService;
        private readonly EntityService _entityService;
        private readonly BlockService _blockService;
        private readonly EventBus _eventBus;

        private readonly HashSet<Vector3Int> _verticalObjectsList = new();
        private IBlockObjectPlacer _blockObjectPlacer;
        private BlockObjectSpec _blockObjectSpec;

        private LadderService(BlockObjectPlacerService blockObjectPlacerService, PrefabNameMapper prefabNameMapper, FactionService factionService, EntityService entityService, BlockService blockService, EventBus eventBus)
        {
            Instance = this;
            _blockObjectPlacerService = blockObjectPlacerService;
            _prefabNameMapper = prefabNameMapper;
            _factionService = factionService;
            _entityService = entityService;
            _blockService = blockService;
            _eventBus = eventBus;
        }

        public void Load()
        {
            var prefabSpec = _prefabNameMapper.GetPrefab(_prefabNameMapper.TryGetPrefabName($"Path.{_factionService.Current.Id}", out var prefabName) ? prefabName : "Path.Folktails");
            _blockObjectSpec = prefabSpec.GetComponentFast<BlockObjectSpec>();
            _blockObjectPlacer = _blockObjectPlacerService.GetMatchingPlacer(_blockObjectSpec);
            _eventBus.Register(this);
        }

        public bool IsLadder(Vector3 nodePosition, Vector3 neighborNodePosition)
        {
            nodePosition += new Vector3(0, (nodePosition.y - neighborNodePosition.y) / 2, 0);

            return IsLadder(nodePosition);
        }

        [OnEvent]
        public void OnBlockObjectSet(BlockObjectSetEvent blockObjectSetEvent)
        {
            if (blockObjectSetEvent.BlockObject.GetComponentFast<PrefabSpec>() == null)
                return;

            if (blockObjectSetEvent.BlockObject.GetComponentFast<Ladder>())
            {
                ReplacePathObject(blockObjectSetEvent.BlockObject);
                
                var coordinate = blockObjectSetEvent.BlockObject.Coordinates;
                _verticalObjectsList.Add(coordinate);
            }
        }

        [OnEvent]
        public void OnConstructibleEnteredFinishedState(EnteredFinishedStateEvent enteredFinishedStateEvent)
        {
            if (enteredFinishedStateEvent.BlockObject.GetComponentFast<Ladder>())
            {
                ReplacePathObject(enteredFinishedStateEvent.BlockObject);
            }
        }

        [OnEvent]
        public void OnBlockObjectUnset(BlockObjectUnsetEvent blockObjectUnsetEvent)
        {
            if (blockObjectUnsetEvent.BlockObject == null)
                return;

            var coordinate = blockObjectUnsetEvent.BlockObject.Coordinates;
            _verticalObjectsList.Remove(coordinate);
        }

        private void ReplacePathObject(BlockObject blockObject)
        {
            var pathObjectAt = _blockService.GetPathObjectAt(blockObject.Placement.Coordinates);
            if (pathObjectAt != null)
            {
                _entityService.Delete(pathObjectAt);
            }

            _blockObjectPlacer.Place(_blockObjectSpec, blockObject.Placement);
        }

        private bool IsLadder(Vector3 coordinates)
        {
            var checkCoordinates = new Vector3Int
            {
                x = Convert.ToInt32(Math.Floor(coordinates.x)),
                y = Convert.ToInt32(Math.Floor(coordinates.z)),
                z = Convert.ToInt32(Math.Floor(coordinates.y))
            };

            return !_verticalObjectsList.Contains(checkCoordinates);
        }
    }
}