using Bindito.Core;
using Timberborn.BlockSystem;
using Timberborn.EntitySystem;
using UnityEngine;

namespace ChooChoo
{
    public class TrackSectionOccupier : MonoBehaviour, IDeletableEntity
    {
        private BlockService _blockService;
        
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
            TrackSection.Enter();
        }

        public void OccupyCurrentTrackSection()
        {
            var startTrackPiece = _blockService.GetFloorObjectComponentAt<TrackPiece>(transform.position.ToBlockServicePosition());
            if (startTrackPiece == null)
                return;
            if (!startTrackPiece.TrackSection.Occupied)
                OccupyNextTrackSection(startTrackPiece.TrackSection);
        }
    }
}