namespace ChooChoo
{
    public interface ITrackFollower
    {
        public int CurrentCornerIndex { get; }
        
        // public List<TrackSection> OccupiedTrackSections { get; set; }

        // public List<TrackConnection> PathConnections { get;  }
    }
}