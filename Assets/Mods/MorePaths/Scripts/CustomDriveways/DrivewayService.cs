using System.Collections.Generic;
using MorePaths.PathSpecificationSystem;
using TimberApi.SingletonSystem;
using Timberborn.PathSystem;
using Timberborn.SingletonSystem;
using UnityEngine;

namespace MorePaths.CustomDriveways
{
    public class DrivewayService : IEarlyLoadableSingleton, ILoadableSingleton
    {
        private static PathSpecificationRepository _pathSpecificationRepository;
        private static DrivewayFactory _drivewayFactory;

        public static Dictionary<Driveway, List<GameObject>> DriveWays { get; private set; }

        private DrivewayService(PathSpecificationRepository pathSpecificationRepository, DrivewayFactory drivewayFactory)
        {
            _pathSpecificationRepository = pathSpecificationRepository;
            _drivewayFactory = drivewayFactory;
        }
        
        public void EarlyLoad()
        {
            DriveWays = _drivewayFactory.CreateDriveways(_pathSpecificationRepository.GetAll());
        }

        public void Load()
        {
            _drivewayFactory.UpdateMaterials(_pathSpecificationRepository.GetAll());
        }
    }
}
