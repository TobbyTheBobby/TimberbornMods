using System;
using System.Linq;
using Timberborn.BlockSystem;
using Timberborn.GameDistricts;
using Timberborn.WalkingSystem;
using UnityEngine;

namespace Unstuckify
{
    public class UnstuckifyService
    {
        private readonly DistrictCenterRegistry _districtCenterRegistry;
        private readonly WalkerService _walkerService;

        private UnstuckifyService(DistrictCenterRegistry districtCenterRegistry, WalkerService walkerService)
        {
            _districtCenterRegistry = districtCenterRegistry;
            _walkerService = walkerService;
        }

        public void Unstuckify(Citizen citizen)
        {
            citizen.TransformFast.position = ClosestDistrictCenterPosition(citizen.TransformFast.position);
            citizen.UnassignDistrict();
        }

        private Vector3 ClosestDistrictCenterPosition(Vector3 currentPosition)
        {
            try
            {
                var closestDistrictCenterPosition = _districtCenterRegistry.AllDistrictCenters
                    .Select(districtCenter =>
                    {
                        var coordinates = districtCenter.GetComponentFast<BlockObject>().PositionedEntrance.DoorstepCoordinates;
                        return new Vector3(coordinates.x, coordinates.z, coordinates.y);
                    })
                    .OrderBy(coordinates => Vector3.Distance(coordinates, currentPosition))
                    .First();

                var closestPositionOnNavMesh = _walkerService.ClosestPositionOnNavMesh(closestDistrictCenterPosition);
                // Plugin.Log.LogInfo("Trying to Unstuckify from " + currentPosition + " to " + closestPositionOnNavMesh);
                return closestPositionOnNavMesh;
            }
            catch (Exception)
            {
                Debug.LogError("You do not have any District Centers.");
                return currentPosition;
            }
        }
    }
}