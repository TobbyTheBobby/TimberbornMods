using ChooChoo.TrackSystem;
using Timberborn.BaseComponentSystem;

namespace ChooChoo.NavigationSystem
{
    public class TrainDestination : BaseComponent
    {
        public TrackPiece TrackPiece { get; private set; }

        private void Awake()
        {
            TrackPiece = GetComponentFast<TrackPiece>();
        }
    }
}