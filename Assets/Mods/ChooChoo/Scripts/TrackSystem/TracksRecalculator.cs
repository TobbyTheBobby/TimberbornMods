using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ChooChoo.BuildingRegistrySystem;
using ChooChoo.Extensions;
using Timberborn.SingletonSystem;

namespace ChooChoo.TrackSystem
{
    public class TracksRecalculator : IPostLoadableSingleton
    {
        private readonly BuildingRegistry<TrackPiece> _trackPieceRegistry;
        private readonly EventBus _eventBus;

        private CancellationTokenSource _cancellationTokenSource = new();

        public TracksRecalculator(BuildingRegistry<TrackPiece> trackPieceRegistry, EventBus eventBus)
        {
            _trackPieceRegistry = trackPieceRegistry;
            _eventBus = eventBus;
        }

        public void PostLoad()
        {
            _eventBus.Register(this);
            RecalculateTracks();
        }

        [OnEvent]
        public void OnTrackUpdate(TracksUpdatedEvent tracksUpdatedEvent)
        {
            RecalculateTracks();
        }

        private async void RecalculateTracks()
        {
            var stopwatch = Stopwatch.StartNew();

            var finishedTrackPieces = _trackPieceRegistry.Finished.ToList();

            if (_cancellationTokenSource.Token.CanBeCanceled)
            {
                _cancellationTokenSource.Cancel();
            }

            _cancellationTokenSource = new CancellationTokenSource();
            var token = _cancellationTokenSource.Token;

            await Task.Run(() =>
            {
                foreach (var trackPiece in finishedTrackPieces)
                {
                    if (token.IsCancellationRequested)
                        return;
                    trackPiece.ResetTrackPiece();
                }

                foreach (var trackPiece in finishedTrackPieces)
                {
                    if (token.IsCancellationRequested)
                        return;
                    trackPiece.LookForTrackSection();
                }
            }, token);
            
            stopwatch.Stop();
            stopwatch.LogTime("OnTrackUpdate");
            
            _eventBus.Post(new TracksRecalculatedEvent());
        }
    }
}