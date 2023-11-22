using Bindito.Core;
using System.Linq;
using Timberborn.BaseComponentSystem;
using UnityEngine;

namespace ChooChoo
{
  internal class DistributableGoodBringerTrain : BaseComponent
  {
    private const bool ShouldLog = false;
    
    private TrainDestinationService _trainDestinationService;
    private GoodsStationsRepository _goodsStationsRepository;
    private TrainWagonsGoodsManager _trainWagonsGoodsManager;

    [Inject]
    public void InjectDependencies(TrainDestinationService trainDestinationService, GoodsStationsRepository goodsStationsRepository, ChooChooCarryAmountCalculator chooChooCarryAmountCalculator)
    {
      _trainDestinationService = trainDestinationService;
      _goodsStationsRepository = goodsStationsRepository;
    }

    public void Awake()
    {
      _trainWagonsGoodsManager = GetComponentFast<TrainWagonsGoodsManager>();
    }

    public bool BringDistributableGoods()
    {
      if (ShouldLog) Plugin.Log.LogInfo("Looking to move goods");
      var reachableGoodStation = _goodsStationsRepository.GoodsStations
        .OrderByDescending(station => station.ReceivingInventory.UnreservedCapacity())
        .FirstOrDefault(station => _trainDestinationService.DestinationReachableOneWay(TransformFast.position, station.TrainDestinationComponent) && station.enabled);
      if (reachableGoodStation == null)
        return false;

      return BringFromSpecificStation(reachableGoodStation);
    }
      
    public bool BringFromSpecificStation(GoodsStation reachableGoodStation)
    {
      var reachableGoodStations = _goodsStationsRepository.GoodsStations
        .Where(station => _trainDestinationService.TrainDestinationsConnectedBothWays(reachableGoodStation.TrainDestinationComponent, station.TrainDestinationComponent) && station.enabled)
        .OrderBy(goodsStation => Vector3.Distance(TransformFast.position, goodsStation.TransformFast.position))
        .ToArray();

      foreach (var goodsStation in reachableGoodStations)
      {
        // Plugin.Log.LogInfo("Sending: " + goodsStation.TransformFast.position + " Receiving: " + reachableGoodStation.TransformFast.position);
        var goods = goodsStation.SendingInventory.Stock;
        if (ShouldLog) Plugin.Log.LogInfo("Any items to send: " + goods.Any());
        foreach (var goodAmount in goods)
        {
          if (_trainWagonsGoodsManager.IsFullOrReserved)
            break;
          _trainWagonsGoodsManager.TryReservingGood(goodAmount, goodsStation, reachableGoodStation);
        }
      }

      if (_trainWagonsGoodsManager.IsCarryingOrReserved)
      {
        if (ShouldLog) Plugin.Log.LogInfo("Found goods to move");
        return true;
      }
      if (ShouldLog) Plugin.Log.LogWarning("CANNOT Export");
      return false;
    }
  }
}
