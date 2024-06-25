using System.Collections.Generic;
using Timberborn.Common;
using UnityEngine;

namespace ChooChoo.TrackSystem
{
    public class TrackSection
    {
        private readonly HashSet<TrackPiece> _trackPieces = new();

        public bool Occupied;
        public bool HasWaitingTrain;
        public Color32 Color;

        public TrackSection(TrackPiece firstTrackPiece)
        {
            _trackPieces.Add(firstTrackPiece);
            Color = new Color32((byte)(Random.value * 255), (byte)(Random.value * 255), (byte)(Random.value * 255), 255);
        }

        public void Add(TrackPiece trackPiece)
        {
            if (_trackPieces.Contains(trackPiece))
                return;
            _trackPieces.Add(trackPiece);
        }

        public void Merge(TrackSection trackSection)
        {
            foreach (var trackPiece in trackSection._trackPieces)
            {
                trackPiece.TrackSection = this;
            }

            _trackPieces.AddRange(trackSection._trackPieces);
        }

        public void Dissolve(TrackPiece trackPiece)
        {
            foreach (var track in _trackPieces)
                track.ResetTrackPiece();
            _trackPieces.Remove(trackPiece);
            foreach (var track in _trackPieces)
                track.LookForTrackSection();
        }

        public void Refresh()
        {
            foreach (var track in _trackPieces)
                track.ResetTrackPiece();
            foreach (var track in _trackPieces)
                track.LookForTrackSection();
        }

        public void Enter()
        {
            Occupied = true;
        }

        public void Leave()
        {
            Occupied = false;
            HasWaitingTrain = false;
        }
    }
}