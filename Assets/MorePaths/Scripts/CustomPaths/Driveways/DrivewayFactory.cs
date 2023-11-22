using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using Timberborn.Meshy;
using Timberborn.PathSystem;
using Timberborn.PrefabOptimization;
using UnityEngine;

namespace MorePaths
{
    public class DrivewayFactory
    {
        private readonly OptimizedPrefabInstantiator _optimizedPrefabInstantiator;
        private readonly DrivewayModelInstantiator _drivewayModelInstantiator;
        private readonly MorePathsCore _morePathsCore;
        private readonly MethodInfo _methodInfo = typeof(DrivewayModelInstantiator).GetMethod("GetModelPrefab", BindingFlags.NonPublic | BindingFlags.Instance);

        private DrivewayFactory(OptimizedPrefabInstantiator optimizedPrefabInstantiator, DrivewayModelInstantiator drivewayModelInstantiator, MorePathsCore morePathsCore)
        {
            _optimizedPrefabInstantiator = optimizedPrefabInstantiator;
            _drivewayModelInstantiator = drivewayModelInstantiator;
            _morePathsCore = morePathsCore;
        }

        public Dictionary<Driveway, List<GameObject>> CreateDriveways(ImmutableArray<PathSpecification> pathSpecifications)
        {
            var drivewaysDictionary = new Dictionary<Driveway, List<GameObject>>();

            foreach (var driveway in Enum.GetValues(typeof(Driveway)).Cast<Driveway>())
            {
                if (driveway == Driveway.None) 
                    continue;

                var drivewayList = new List<GameObject>();
                var originalDrivewayModel = (GameObject)_methodInfo.Invoke(_drivewayModelInstantiator, new object[] { driveway });
                originalDrivewayModel.SetActive(false);

                foreach (var pathSpecification in pathSpecifications)
                {
                    if (pathSpecification.Name == "DefaultPath") continue;
                    var newDriveway = CreateDriveway(originalDrivewayModel, pathSpecification);

                    drivewayList.Add(newDriveway);
                }

                drivewaysDictionary.Add(driveway, drivewayList);
            }

            return drivewaysDictionary;
        }

        private GameObject CreateDriveway(GameObject originalDrivewayModel, PathSpecification pathSpecification)
        {
            GameObject driveway = _optimizedPrefabInstantiator.Instantiate(originalDrivewayModel, new GameObject().transform);

            driveway.name = pathSpecification.Name;

            return driveway;
        }

        public void UpdateMaterials(ImmutableArray<PathSpecification> pathSpecifications)
        {
            foreach (var driveways in DrivewayService.DriveWays.Values)
            {
                foreach (var driveway in driveways)
                {
                    var pathSpecification = pathSpecifications.First(specification => driveway.name == specification.Name);
                
                    var material = new Material(CustomPathFactory.ActivePathMaterial);

                    material.mainTexture = _morePathsCore.TryLoadTexture(pathSpecification.Name, pathSpecification.PathTexture);

                    material.SetFloat("_MainTexScale", pathSpecification.MainTextureScale);
                    material.SetFloat("_NoiseTexScale", pathSpecification.NoiseTexScale);
                    material.SetVector("_MainColor", new Vector4(pathSpecification.MainColorRed, pathSpecification.MainColorGreen, pathSpecification.MainColorBlue, 1f));

                    driveway.GetComponentInChildren<MeshRenderer>().sharedMaterial = material;
                }
            }
        }
    }
}
