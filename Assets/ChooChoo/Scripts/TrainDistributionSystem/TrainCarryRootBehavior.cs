using Bindito.Core;
using System;
using System.Linq;
using Timberborn.BehaviorSystem;
using Timberborn.InventorySystem;

namespace ChooChoo
{
  public class TrainCarryRootBehavior : RootBehavior
  {
    private ChooChooCarryAmountCalculator _chooChooCarryAmountCalculator;
    private DistributableGoodBringerTrain _distributableGoodBringerTrain;
    private TrainWagonsGoodsManager _trainWagonsGoodsManager;
    private MoveToStationExecutor _moveToStationExecutor;

    [Inject]
    public void InjectDependencies(ChooChooCarryAmountCalculator chooChooCarryAmountCalculator) 
    {
      _chooChooCarryAmountCalculator = chooChooCarryAmountCalculator;
    }

    public void Awake()
    {
      _distributableGoodBringerTrain = GetComponentFast<DistributableGoodBringerTrain>();
      _trainWagonsGoodsManager = GetComponentFast<TrainWagonsGoodsManager>();
      _moveToStationExecutor = GetComponentFast<MoveToStationExecutor>();
    }

    public override Decision Decide(BehaviorAgent agent)
    {
      if (_trainWagonsGoodsManager.IsCarrying)
      {
        if (!_trainWagonsGoodsManager.HasReservedCapacity 
            // && !ReserveCapacityForCarriedGoods()
            )
        {
          _trainWagonsGoodsManager.EmptyWagons();
          return Decision.ReleaseNow();
        }

        var currentInventory = _trainWagonsGoodsManager.MostRecentWagons.First(wagon => wagon.GoodReserver.HasReservedCapacity).GoodReserver.CapacityReservation.Inventory;
        var executorStatus = _moveToStationExecutor.Launch(currentInventory.GetComponentFast<TrainDestination>());
        // Plugin.Log.LogError("CapacityReservation " + executorStatus); 
        switch (executorStatus)
        {
          case ExecutorStatus.Success:
            return CompleteDelivery(currentInventory);
          case ExecutorStatus.Failure:
            _trainWagonsGoodsManager.UnreserveCapacity();
            return Decision.ReleaseNextTick();
          case ExecutorStatus.Running:
            return Decision.ReturnWhenFinished(_moveToStationExecutor);
          default:
            throw new ArgumentOutOfRangeException();
        }
      }

      if (!_trainWagonsGoodsManager.HasReservedCapacity)
        return Decision.ReleaseNow();
      if (_trainWagonsGoodsManager.HasReservedStock)
      {
        var station = _trainWagonsGoodsManager.MostRecentWagons.First(wagon => wagon.GoodReserver.HasReservedStock)
          .GoodReserver.StockReservation.Inventory.GetComponentFast<TrainDestination>();
        var executorStatus = _moveToStationExecutor.Launch(station);
        // Plugin.Log.LogError("StockReservation " + executorStatus);
        switch (executorStatus)
        {
          case ExecutorStatus.Success:
            return CompleteRetrieval(station);
          case ExecutorStatus.Failure:
            return UnreserveGood();
          case ExecutorStatus.Running:
            return Decision.ReturnWhenFinished(_moveToStationExecutor);
          default:
            throw new ArgumentOutOfRangeException();
        }
      }

      _trainWagonsGoodsManager.UnreserveCapacity();
      return Decision.ReleaseNextTick();
    }

    private Decision CompleteDelivery(Inventory currentInventory)
    {
      _trainWagonsGoodsManager.TryDeliveringGoods(currentInventory);
      return Decision.ReturnNextTick();
    }

    private Decision CompleteRetrieval(TrainDestination destination)
    {
      _distributableGoodBringerTrain.BringFromSpecificStation(destination.GetComponentFast<GoodsStation>());
      _trainWagonsGoodsManager.TryRetrievingGoods(destination);
      return Decision.ReturnNextTick();
    }

    private Decision UnreserveGood()
    {
      _trainWagonsGoodsManager.UnreserveStock();
      return Decision.ReleaseNextTick();
    }
  }
}
