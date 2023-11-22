using System.Collections.Generic;
using TimberApi.Common.SingletonSystem;
using Timberborn.PathSystem;
using Timberborn.SingletonSystem;
using UnityEngine;

namespace MorePaths
{
    public class DrivewayService : IEarlyLoadableSingleton, ILoadableSingleton
    {
        private static MorePathsCore _morePathsCore;
        private static DrivewayFactory _drivewayFactory;

        public static Dictionary<Driveway, List<GameObject>> DriveWays { get; private set; }

        DrivewayService(MorePathsCore morePathsCore, DrivewayFactory drivewayFactory)
        {
            _morePathsCore = morePathsCore;
            _drivewayFactory = drivewayFactory;
        }
        
        public void EarlyLoad()
        {
            DriveWays = _drivewayFactory.CreateDriveways(_morePathsCore.PathsSpecifications);
        }


        public void Load()
        {
            _drivewayFactory.UpdateMaterials(_morePathsCore.PathsSpecifications);
        }
    }
}
