using System.Collections.Generic;
using Bindito.Core;
using Timberborn.BaseComponentSystem;
using Timberborn.BlockSystem;
using Timberborn.CharacterMovementSystem;
using Timberborn.WalkingSystem;
using TobbyTools.InaccessibilityUtilitySystem;
using UnityEngine;

namespace ChooChoo.BeaverNavigationUtilities
{
    public class PathFollowerUtilities : BaseComponent
    {
        private PathConverter _pathConverter;

        private PathFollower _pathFollower;

        private BlockObject[] _toBeVisitedBlockObjects;

        [Inject]
        public void InjectDependencies(PathConverter pathConverter)
        {
            _pathConverter = pathConverter;
        }

        private void Start()
        {
            var walker = GetComponentFast<Walker>();
            walker.StartedNewPath += OnStartedNewPath;
            _pathFollower = (PathFollower)InaccessibilityUtilities.GetInaccessibleField(walker, "_pathFollower");
        }

        public BlockObject GetBlockObjectAtIndex(int index)
        {
            if (_toBeVisitedBlockObjects == null)
                return null;

            return index >= _toBeVisitedBlockObjects.Length ? null : _toBeVisitedBlockObjects[index];
        }

        private void OnStartedNewPath(object sender, StartedNewPathEventArgs e)
        {
            var pathCorners = (IReadOnlyList<Vector3>)InaccessibilityUtilities.GetInaccessibleField(_pathFollower, "_pathCorners");
            _toBeVisitedBlockObjects = _pathConverter.ConvertPath(pathCorners);
        }
    }
}