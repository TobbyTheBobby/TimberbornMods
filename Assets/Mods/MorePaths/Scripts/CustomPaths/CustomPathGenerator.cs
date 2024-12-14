using MorePaths.PathSpecificationSystem;
using Timberborn.BaseComponentSystem;
using Timberborn.GameFactionSystem;
using Timberborn.PathSystem;
using Timberborn.PrefabSystem;
using UnityEngine;

namespace MorePaths.CustomPaths
{
    public class CustomPathGenerator
    {
        private readonly PathSpecificationRepository _pathSpecificationRepository;
        private readonly CustomPathsRepository _customPathsRepository;
        private readonly CustomPathFactory _customPathFactory;
        private readonly BaseInstantiator _baseInstantiator;
        private readonly FactionService _factionService;
        
        public CustomPathGenerator(
            PathSpecificationRepository pathSpecificationRepository, 
            CustomPathsRepository customPathsRepository,
            CustomPathFactory customPathFactory,
            BaseInstantiator baseInstantiator,
            FactionService factionService)
        {
            _pathSpecificationRepository = pathSpecificationRepository;
            _customPathsRepository = customPathsRepository;
            _customPathFactory = customPathFactory;
            _baseInstantiator = baseInstantiator;
            _factionService = factionService;
        }
        
        public void VeryEarlyLoad()
        {
            foreach (var dynamicPathModel in Resources.LoadAll<DynamicPathModel>(""))
            {
                if (!dynamicPathModel.GetComponentFast<Prefab>().Name.Contains(_factionService.Current.Id)) 
                    continue;
                foreach (var pathSpecification in _pathSpecificationRepository.GetAll())
                {
                    _customPathsRepository.Add(_customPathFactory.CreateCustomPath(dynamicPathModel, pathSpecification));
                }
            }
        }
    }
}