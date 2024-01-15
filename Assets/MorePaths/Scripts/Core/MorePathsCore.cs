using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using MorePaths.CustomPaths;
using MorePaths.Specifications;
using TimberApi.Common.SingletonSystem;
using Timberborn.Persistence;

namespace MorePaths.Core
{
    public class MorePathsCore : ITimberApiLoadableSingleton
    {
        private readonly ISpecificationService _specificationService;
        private readonly PathSpecificationObjectDeserializer _pathSpecificationObjectDeserializer;

        private MethodInfo _methodInfo;
        public ImmutableArray<PathSpecification> PathsSpecifications;
        private List<CustomPath> _customPaths;
        public List<CustomPath> CustomPaths
        {
            get
            {
                if (_customPaths != null) 
                    return _customPaths;
                _customPaths = TimberApi.DependencyContainerSystem.DependencyContainer.GetInstance<CustomPathFactory>().CreatePathsFromSpecification();
                return _customPaths;
            }
        }

        public MorePathsCore(ISpecificationService specificationService, PathSpecificationObjectDeserializer pathSpecificationObjectDeserializer)
        {
            _specificationService = specificationService;
            _pathSpecificationObjectDeserializer = pathSpecificationObjectDeserializer;
        }

        public void Load()
        {
            LoadPathSpecifications();
        }

        private void LoadPathSpecifications()
        {
            if (PathsSpecifications != null)
                return;
            var list = _specificationService.GetSpecifications(_pathSpecificationObjectDeserializer).Where(specification => specification.Enabled).ToList();
            var orderedList = list.OrderBy(specification => specification.ToolOrder);
            PathsSpecifications = orderedList.ToImmutableArray();
        }
    }
}