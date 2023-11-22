using System.Collections.Generic;
using ChooChoo;
using Timberborn.Common;
using UnityEngine;

public class TrackSection
{
    public readonly HashSet<TrackPiece> TrackPieces = new();

    public bool Occupied;

    public bool HasWaitingTrain;

    public Color32 Color;

    public TrackSection(TrackPiece firstTrackPiece)
    {
        TrackPieces.Add(firstTrackPiece);
        Color = new Color32((byte)(Random.value * 255), (byte)(Random.value * 255), (byte)(Random.value * 255), 255);
    }

    public void Add(TrackPiece trackPiece)
    {
        TrackPieces.Add(trackPiece);
    }

    public void Merge(TrackSection trackSection)
    {
        foreach (var trackPiece in trackSection.TrackPieces)
        {
            trackPiece.TrackSection = this;
        }
        TrackPieces.AddRange(trackSection.TrackPieces);
    }
    
    public void Dissolve(TrackPiece trackPiece)
    {
        TrackPieces.Remove(trackPiece);
        foreach (var track in TrackPieces)
            track.ResetTrackPiece();
        foreach (var track in TrackPieces)
            track.LookForTrackSection();
    }
    
    public void Refresh()
    {
        foreach (var track in TrackPieces)
            track.ResetTrackPiece();
        foreach (var track in TrackPieces)
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
