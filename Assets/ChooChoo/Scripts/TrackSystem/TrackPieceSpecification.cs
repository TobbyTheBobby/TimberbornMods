namespace ChooChoo
{
    public class TrackPieceSpecification
    {
        public TrackPieceSpecification(
            string name,
            TrackRoute[] trackRoutes)
        {
            Name = name;
            TrackRoutes = trackRoutes;
        }
        
        public string Name { get; }
        public TrackRoute[] TrackRoutes { get; }
    }
}