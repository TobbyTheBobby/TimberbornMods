using Bindito.Core;
using Timberborn.BaseComponentSystem;
using Timberborn.BlockSystem;
using Timberborn.CharacterMovementSystem;
using Timberborn.WalkingSystem;

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
            // walker.
            // walker.StartedNewPath += OnStartedNewPath;
            _pathFollower = walker.PathFollower;
        }

        public BlockObject GetBlockObjectAtIndex(int index)
        {
            if (_toBeVisitedBlockObjects == null)
                return null;

            return index >= _toBeVisitedBlockObjects.Length ? null : _toBeVisitedBlockObjects[index];
        }

        private void OnStartedNewPath(object sender, StartedNewPathEventArgs e)
        {
            _toBeVisitedBlockObjects = _pathConverter.ConvertPath(_pathFollower._pathCorners);
        }
    }
}