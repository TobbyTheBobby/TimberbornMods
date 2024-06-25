using Bindito.Core;
using ChooChoo.Extensions;
using JetBrains.Annotations;
using Timberborn.BaseComponentSystem;
using Timberborn.BlockSystem;
using Timberborn.EntitySystem;

namespace ChooChoo.TrackSystem
{
    public class TrackSectionOccupier : BaseComponent, IDeletableEntity
    {
        private BlockService _blockService;

        [CanBeNull]
        public TrackSection TrackSection { get; private set; }

        [Inject]
        public void InjectDependencies(BlockService blockService)
        {
            _blockService = blockService;
        }

        public void DeleteEntity()
        {
            TrackSection?.Leave();
        }

        public void OccupyNextTrackSection(TrackSection trackSection)
        {
            TrackSection?.Leave();
            TrackSection = trackSection;
            TrackSection?.Enter();
        }

        public void OccupyCurrentTrackSection()
        {
            var startTrackPiece = _blockService.GetFloorObjectComponentAt<TrackPiece>(TransformFast.position.ToBlockServicePosition());
            if (startTrackPiece == null)
                return;
            if (!startTrackPiece.TrackSection.Occupied)
                OccupyNextTrackSection(startTrackPiece.TrackSection);
        }
    }
}