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
            lock (_trackPieces)
            {
                _trackPieces.Add(trackPiece);
            }
        }

        public void Merge(TrackSection trackSection)
        {
            lock (_trackPieces)
            {
                foreach (var trackPiece in trackSection._trackPieces)
                {
                    trackPiece.TrackSection = this;
                }
                
                _trackPieces.AddRange(trackSection._trackPieces);
            }
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