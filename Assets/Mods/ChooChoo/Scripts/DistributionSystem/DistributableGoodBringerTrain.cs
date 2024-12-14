using System.Collections.Generic;
using System.Linq;
using Bindito.Core;
using ChooChoo.BuildingRegistrySystem;
using ChooChoo.Extensions;
using ChooChoo.GoodsStations;
using ChooChoo.NavigationSystem;
using ChooChoo.TrackSystem;
using ChooChoo.Wagons;
using Timberborn.BaseComponentSystem;
using UnityEngine;

namespace ChooChoo.DistributionSystem
{
    internal class DistributableGoodBringerTrain : BaseComponent
    {
        private readonly bool _shouldLog = false;

        private BuildingRegistry<GoodsStation> _goodsStationsRegistry;
        private TrainDestinationService _trainDestinationService;
        private TrainNavigationService _trainNavigationService;

        private TrainWagonsGoodsManager _trainWagonsGoodsManager;

        [Inject]
        public void InjectDependencies(
            BuildingRegistry<GoodsStation> goodsStationsRegistry,
            TrainDestinationService trainDestinationService,
            TrainNavigationService trainNavigationService)
        {
            _goodsStationsRegistry = goodsStationsRegistry;
            _trainDestinationService = trainDestinationService;
            _trainNavigationService = trainNavigationService;
        }

        public void Awake()
        {
            _trainWagonsGoodsManager = GetComponentFast<TrainWagonsGoodsManager>();
        }

        public bool BringDistributableGoods()
        {
            if (_shouldLog) Debug.Log("Looking to move goods");
                
            // var reachableGoodStation = _goodsStationsRegistry.Finished
            //     .OrderByDescending(station => station.ReceivingInventory.UnreservedCapacity())
            //     .FirstOrDefault(station =>
            //         _trainDestinationService.DestinationReachableOneWay(TransformFast.position, station.GetComponentFast<TrainDestination>()) &&
            //         station.enabled);
            
            var reachableGoodStation = _goodsStationsRegistry.Finished
                .OrderByDescending(station => station.ReceivingInventory.UnreservedCapacity())
                .FirstOrDefault(station =>
                    _trainNavigationService.FindTrackPath(TransformFast, station.GetComponentFast<TrainDestination>(), new List<TrackRoute>()) &&
                    station.enabled);
            
            if (reachableGoodStation == null)
                return false;

            return BringFromSpecificStation(reachableGoodStation);
        }

        public bool BringFromSpecificStation(GoodsStation reachableGoodStation)
        {
            var reachableGoodStations = _goodsStationsRegistry.Finished
                .Where(station =>
                    _trainDestinationService.TrainDestinationsConnectedBothWays(reachableGoodStation.GetComponentFast<TrainDestination>(),
                        station.GetComponentFast<TrainDestination>()) && station.enabled)
                .OrderBy(goodsStation => Vector3.Distance(TransformFast.position, goodsStation.TransformFast.position))
                .ToArray();

            foreach (var goodsStation in reachableGoodStations)
            {
                // Plugin.Log.LogInfo("Sending: " + goodsStation.TransformFast.position + " Receiving: " + reachableGoodStation.TransformFast.position);
                var goods = goodsStation.SendingInventory.Stock;
                if (_shouldLog) Debug.Log("Any items to send: " + goods.Any());
                foreach (var goodAmount in goods)
                {
                    if (_trainWagonsGoodsManager.IsFullOrReserved)
                        break;
                    _trainWagonsGoodsManager.TryReservingGood(goodAmount, goodsStation, reachableGoodStation);
                }
            }

            if (_trainWagonsGoodsManager.IsCarryingOrReserved)
            {
                if (_shouldLog) Debug.Log("Found goods to move");
                return true;
            }

            if (_shouldLog) Debug.LogWarning("CANNOT Export");
            return false;
        }
    }
}