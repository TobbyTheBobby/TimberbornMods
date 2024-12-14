using System.Collections.Immutable;
using System.Linq;
using Timberborn.Persistence;
using Timberborn.SingletonSystem;

namespace MorePaths.PathSpecificationSystem
{
    public class PathSpecificationRepository
    {
        private readonly PathSpecificationObjectDeserializer _pathSpecificationObjectDeserializer;
        private readonly ISpecificationService _specificationService;

        private ImmutableArray<PathSpecification> PathSpecifications;
        
        public PathSpecificationRepository(
            PathSpecificationObjectDeserializer pathSpecificationObjectDeserializer,
            ISpecificationService specificationService)
        {
            _pathSpecificationObjectDeserializer = pathSpecificationObjectDeserializer;
            _specificationService = specificationService;
        }
        
        public void Load()
        {
            Test();
        }

        public ImmutableArray<PathSpecification> GetAll()
        {
            return Test();
        }

        private ImmutableArray<PathSpecification> Test()
        {
            if (PathSpecifications != null)
                return PathSpecifications;
            
            // Debug.LogError(_specificationService.GetSpecifications(_pathSpecificationObjectDeserializer).Count());
            
            var list = _specificationService.GetSpecifications(_pathSpecificationObjectDeserializer).Where(specification => specification.Enabled).ToList();
            var orderedList = list.OrderBy(specification => specification.ToolOrder);
            // Debug.LogWarning($"orderedList.count {orderedList.Count()}");
            // foreach (var VARIABLE in orderedList)
            // {
            //     Debug.LogWarning(VARIABLE.Name);
            // }
            
            PathSpecifications = orderedList.ToImmutableArray();
            return PathSpecifications;
        }
    }
}