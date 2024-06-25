using System;
using Timberborn.Persistence;

namespace ChooChoo.TrackSystem
{
    public class TrackPieceSpecificationDeserializer : IObjectSerializer<TrackPieceSpecification>
    {
        private readonly TrackRouteObjectDeserializer _trackRouteObjectDeserializer;

        private TrackPieceSpecificationDeserializer(TrackRouteObjectDeserializer trackRouteObjectDeserializer)
        {
            _trackRouteObjectDeserializer = trackRouteObjectDeserializer;
        }
        
        public void Serialize(TrackPieceSpecification value, IObjectSaver objectSaver)
        {
            throw new NotSupportedException();
        }

        public Obsoletable<TrackPieceSpecification> Deserialize(IObjectLoader objectLoader)
        {
            return (Obsoletable<TrackPieceSpecification>) new TrackPieceSpecification(
                objectLoader.Get(new PropertyKey<string>("Name")),
                objectLoader.Get(new ListKey<TrackRoute>("TrackRoutes"), _trackRouteObjectDeserializer).ToArray());
        }
    }
}
