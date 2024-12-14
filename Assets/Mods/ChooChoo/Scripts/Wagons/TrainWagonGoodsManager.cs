using Bindito.Core;
using ChooChoo.DistributionSystem;
using ChooChoo.NavigationSystem;
using Timberborn.BaseComponentSystem;
using Timberborn.Carrying;
using Timberborn.Goods;
using Timberborn.InventorySystem;
using UnityEngine;

namespace ChooChoo.Wagons
{
    public class TrainWagonGoodsManager : BaseComponent
    {
        private const bool ShouldLog = false;

        private ChooChooCarryAmountCalculator _chooChooCarryAmountCalculator;

        public GoodCarrier GoodCarrier { get; private set; }
        public GoodReserver GoodReserver { get; private set; }

        [Inject]
        public void InjectDependencies(ChooChooCarryAmountCalculator chooChooCarryAmountCalculator)
        {
            _chooChooCarryAmountCalculator = chooChooCarryAmountCalculator;
        }

        public bool IsCarrying => GoodCarrier.IsCarrying;

        public bool IsFullOrReserved =>
            IsCarrying &&
            _chooChooCarryAmountCalculator.IsAtMaximumCarryCapacity(GoodCarrier.LiftingCapacity, GoodCarrier.CarriedGoods) ||
            (HasReservedStock &&
             _chooChooCarryAmountCalculator.IsAtMaximumCarryCapacity(GoodCarrier.LiftingCapacity, GoodReserver.StockReservation.GoodAmount));

        public bool IsCarryingOrReserved => GoodCarrier.IsCarrying || GoodReserver.HasReservedStock;

        public bool HasReservedCapacity => GoodReserver.HasReservedCapacity;

        public bool HasReservedStock => GoodReserver.HasReservedStock;

        public void Awake()
        {
            GoodCarrier = GetComponentFast<GoodCarrier>();
            GoodReserver = GetComponentFast<GoodReserver>();
        }

        public bool TryReservingGood(GoodAmount goodAmount, Inventory sendingInventory, Inventory receivingInventory,
            ref int remainingToBeReservedAmount)
        {
            var toBeReservedGoodId = goodAmount.GoodId;
            if (ShouldLog) Debug.Log("Looking to reserve: " + remainingToBeReservedAmount + " " + toBeReservedGoodId);

            var carry = _chooChooCarryAmountCalculator.AmountToCarry(GoodCarrier.LiftingCapacity,
                MaxTakeableAmount(sendingInventory, toBeReservedGoodId, remainingToBeReservedAmount));
            if (ShouldLog) Debug.Log("Carry Amount: " + carry.Amount);
            if (carry.Amount <= 0)
                return false;

            var maxAmountToCarry = _chooChooCarryAmountCalculator.MaxAmountToCarry(GoodCarrier.LiftingCapacity, toBeReservedGoodId);
            if (ShouldLog) Debug.Log("Max Amount able to carry: " + maxAmountToCarry);

            var maxGiveableAmount = receivingInventory.UnreservedCapacity(toBeReservedGoodId);
            if (ShouldLog) Debug.Log("Max giveable amount: " + maxGiveableAmount);

            if (maxGiveableAmount == 0)
                return false;

            if (carry.Amount > maxGiveableAmount)
                carry = new GoodAmount(toBeReservedGoodId, maxGiveableAmount);

            // there are 2 situations: toBeReservedAmount is 60 or 10 and carrying capacity is to be expected 50

            if (IsCarrying && HasReservedStock)
            {
                // Currently not implemented as they are currently limited to using the same sending point. 
                // Implementation requires being able to use different stations as sending. 
                if (ShouldLog) Debug.LogError("Both Carrying AND already has Reserved Stock.");
                return false;
            }

            if (IsCarrying || HasReservedStock)
            {
                if (ShouldLog) Debug.LogWarning("Is carrying OR has Reserved");
                var currentAmount = IsCarrying ? GoodCarrier.CarriedGoods.Amount : GoodReserver.StockReservation.GoodAmount.Amount;
                // 30 = 50 - 20 which means that fillable amount is 30.
                var fillableAmount = FillableAmount(maxAmountToCarry, currentAmount);
                if (ShouldLog) Debug.Log("Fillable amount: " + fillableAmount);
                // 30 > 0 which means that there the wagon can be topped up.
                if (SameOriginAndDestinationAndGood(sendingInventory, receivingInventory, toBeReservedGoodId) && fillableAmount > 0)
                {
                    if (ShouldLog) Debug.Log("Fillable");
                    if (carry.Amount > fillableAmount)
                    {
                        // 60 > 30 which means that there is more to reserve than the amount that can be filled. It will try to top up the wagon and there will be a remainder that has to be reserved. 
                        Reserve(sendingInventory, receivingInventory, toBeReservedGoodId, maxAmountToCarry);
                        remainingToBeReservedAmount -= fillableAmount;
                        return true;
                    }

                    // 10 > 30 which means that there is less to reserve than the amount that can be filled. Means it can still be filled, but the current queue item is completed. 
                    var combinedAmount = carry.Amount + currentAmount;
                    Reserve(sendingInventory, receivingInventory, toBeReservedGoodId, combinedAmount);
                    remainingToBeReservedAmount -= remainingToBeReservedAmount;
                    return true;
                }

                if (ShouldLog) Debug.Log("Not fillable");
                return false;
            }

            if (ShouldLog) Debug.LogWarning("Is Empty");
            // 60 > 50 
            if (remainingToBeReservedAmount > carry.Amount)
            {
                Reserve(sendingInventory, receivingInventory, toBeReservedGoodId, carry.Amount);
                remainingToBeReservedAmount -= carry.Amount;
                return true;
            }

            // 10 > 50
            Reserve(sendingInventory, receivingInventory, toBeReservedGoodId, remainingToBeReservedAmount);
            remainingToBeReservedAmount -= remainingToBeReservedAmount;
            return true;
        }

        public void TryRetrievingGoods(TrainDestination trainDestination)
        {
            if (!HasReservedStock)
                return;
            var stockReservation = GoodReserver.StockReservation;
            GoodReserver.UnreserveStock();
            stockReservation.Inventory.Take(stockReservation.GoodAmount);
            // if (IsCarrying)
            //     GoodCarrier.PutGoodsInHands(new GoodAmount(stockReservation.GoodAmount.GoodId, GoodCarrier.CarriedGoods.Amount + stockReservation.GoodAmount.Amount));
            // else
            GoodCarrier.PutGoodsInHands(stockReservation.GoodAmount);
            // var goodsStation = stockReservation.Inventory.GetComponentFast<GoodsStation>();
            // foreach (var goodAmountRoute in _currentTrainDistributableGoods)
            //     goodsStation.ResolveRetrieval(goodAmountRoute);


            // if (stockReservation.Inventory.TryGetComponentFast(out TrainDestination component) && component == trainDestination)
            // {
            //     GoodReserver.UnreserveStock();
            //     stockReservation.Inventory.Take(stockReservation.GoodAmount);
            //     // if (IsCarrying)
            //     //     GoodCarrier.PutGoodsInHands(new GoodAmount(stockReservation.GoodAmount.GoodId, GoodCarrier.CarriedGoods.Amount + stockReservation.GoodAmount.Amount));
            //     // else
            //     GoodCarrier.PutGoodsInHands(stockReservation.GoodAmount);
            // }
        }

        public void TryDeliveringGoods(Inventory currentInvetory)
        {
            var capacityReservation = GoodReserver.CapacityReservation;

            if (currentInvetory != capacityReservation.Inventory)
                return;
            GoodReserver.UnreserveCapacity();
            var goodAmount = capacityReservation.GoodAmount;
            var unreservedCapacity = capacityReservation.Inventory.UnreservedCapacity(goodAmount.GoodId);
            if (unreservedCapacity <= 0)
                return;
            if (unreservedCapacity < goodAmount.Amount)
                capacityReservation.Inventory.Give(new GoodAmount(goodAmount.GoodId, unreservedCapacity));
            else
                capacityReservation.Inventory.Give(goodAmount);
            GoodCarrier.EmptyHands();
            if (ShouldLog) Debug.Log("Wagon delivering: " + capacityReservation.GoodAmount);
        }

        private bool SameOriginAndDestinationAndGood(Inventory origin, Inventory destination, string goodId)
        {
            // Debug.Log("Same origin: " + (GoodReserver.CapacityReservation.Inventory == destination) + "Same destination: " + (GoodReserver.CapacityReservation.Inventory == destination) + " And goodId: " + (GoodReserver.CapacityReservation.GoodAmount.GoodId == goodId));
            return GoodReserver.StockReservation.Inventory == origin && GoodReserver.CapacityReservation.Inventory == destination &&
                   GoodReserver.CapacityReservation.GoodAmount.GoodId == goodId;
        }

        private int FillableAmount(int maxAmount, int currentAmount)
        {
            // if (_shouldLog) Debug.Log($"Calculate fillable amount (max amount - current amount): {maxAmount} - {currentAmount} = {maxAmount - currentAmount}");
            return maxAmount - currentAmount;
        }

        private void Reserve(Inventory sendingInventory, Inventory receivingInventory, string goodId, int amountToBeReserved)
        {
            var goodAmount = new GoodAmount(goodId, amountToBeReserved);
            GoodReserver.ReserveExactStockAmount(sendingInventory, goodAmount);
            GoodReserver.ReserveCapacity(receivingInventory, goodAmount);
        }

        private GoodAmount MaxTakeableAmount(
            Inventory inventory,
            string goodId,
            int goodAmount)
        {
            if (ShouldLog)
                Debug.Log("MaxTakeableAmount     UnreservedAmountInStock: " + inventory.UnreservedAmountInStock(goodId) + "   goodAmount: " +
                                   goodAmount);

            var amount = Mathf.Min(inventory.UnreservedAmountInStock(goodId), goodAmount);
            return new GoodAmount(goodId, amount);
        }
    }
}