using System.Linq;
using TimberApi.SpecificationSystem;
using Timberborn.SingletonSystem;

namespace ChooChoo
{
   public class TrackArrayProvider : ILoadableSingleton
   {
      private readonly TrackPieceSpecificationDeserializer _trackPieceSpecificationDeserializer;
      private readonly IApiSpecificationService _apiSpecificationService;

      private TrackPieceSpecification[] _trackPieceSpecifications;
      
      TrackArrayProvider(TrackPieceSpecificationDeserializer trackPieceSpecificationDeserializer, IApiSpecificationService apiSpecificationService)
      {
         _trackPieceSpecificationDeserializer = trackPieceSpecificationDeserializer;
         _apiSpecificationService = apiSpecificationService;
      }

      public void Load()
      {
         _trackPieceSpecifications = _apiSpecificationService.GetSpecifications(_trackPieceSpecificationDeserializer).ToArray();
      }
      
      public TrackRoute[] GetConnections(string prefabName)
      {
         prefabName = FixPrefabName(prefabName);
         // Plugin.Log.LogWarning("Providing TrackRoutes: " + prefabName);
         return _trackPieceSpecifications.First(specification => specification.Name == prefabName).TrackRoutes.Select(route => route.CreateCopy()).ToArray();
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
