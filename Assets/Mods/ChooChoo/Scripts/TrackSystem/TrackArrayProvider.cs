using System.Linq;
using Timberborn.Persistence;
using Timberborn.SingletonSystem;

namespace ChooChoo.TrackSystem
{
    public class TrackArrayProvider : ILoadableSingleton
    {
        private readonly TrackPieceSpecificationDeserializer _trackPieceSpecificationDeserializer;
        private readonly ISpecificationService _specificationService;

        private TrackPieceSpecification[] _trackPieceSpecifications;

        private TrackArrayProvider(
            TrackPieceSpecificationDeserializer trackPieceSpecificationDeserializer,
            ISpecificationService specificationService)
        {
            _trackPieceSpecificationDeserializer = trackPieceSpecificationDeserializer;
            _specificationService = specificationService;
        }

        public void Load()
        {
            _trackPieceSpecifications = _specificationService.GetSpecifications(_trackPieceSpecificationDeserializer).ToArray();
        }

        public TrackRoute[] GetConnections(string prefabName)
        {
            prefabName = FixPrefabName(prefabName);
            // Plugin.Log.LogWarning("Providing TrackRoutes: " + prefabName);
            return _trackPieceSpecifications.First(specification => specification.Name == prefabName).TrackRoutes.Select(route => route.CreateCopy())
                .ToArray();
        }

        private string FixPrefabName(string prefabName)
        {
            return prefabName
                .Replace("(Clone)", "")
                .Replace(".Folktails", "")
                .Replace(".IronTeeth", "")
                .Replace("Preview", "");
        }
    }
}