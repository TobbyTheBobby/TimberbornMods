using System.Collections.Generic;
using System.Linq;
using ChooChoo.Extensions;
using ChooChoo.GoodsStations;
using ChooChoo.NavigationSystem;
using Timberborn.BaseComponentSystem;
using Timberborn.Goods;
using Timberborn.InventorySystem;

namespace ChooChoo.Wagons
{
    public class TrainWagonsGoodsManager : BaseComponent
    {
        private List<TrainWagonGoodsManager> Wagons { get; } = new();
        public List<TrainWagonGoodsManager> MostRecentWagons { get; } = new();

        public bool IsCarrying => Wagons.Any(wagon => wagon.IsCarrying);

        public bool IsFullOrReserved => MostRecentWagons.All(wagon => wagon.IsFullOrReserved);

        public bool IsCarryingOrReserved => MostRecentWagons.Any(wagon => wagon.IsCarryingOrReserved);

        public bool HasReservedCapacity => MostRecentWagons.Any(wagon => wagon.HasReservedCapacity);

        public bool HasReservedStock => MostRecentWagons.Any(wagon => wagon.HasReservedStock);

        private void Start()
        {
            foreach (var trainWagon in GetComponentFast<WagonManager>().Wagons)
            {
                var trainWagonGoodsManager = trainWagon.GetComponentFast<TrainWagonGoodsManager>();
                Wagons.Add(trainWagonGoodsManager);
                MostRecentWagons.Add(trainWagonGoodsManager);
            }
        }

        // Important to remember that reserving can be from a different inventory every time. So the stock reservation might be from a different inventory.
        public void TryReservingGood(GoodAmount goodAmount, GoodsStation sendingGoodsStation, GoodsStation receivingGoodsStation)
        {
            var remainingToBeReservedAmount = goodAmount.Amount;

            foreach (var currentWagon in Wagons)
            {
                if (currentWagon.TryReservingGood(goodAmount, sendingGoodsStation.SendingInventory, receivingGoodsStation.ReceivingInventory,
                        ref remainingToBeReservedAmount))
                {
                    MostRecentWagons.MoveItemToFront(currentWagon);
                }

                if (remainingToBeReservedAmount <= 0)
                    break;
            }
            // Plugin.Log.LogInfo("remainingToBeReservedAmount: " + remainingToBeReservedAmount);
        }

        public void TryDeliveringGoods(Inventory currentInvetory)
        {
            foreach (var trainWagon in Wagons)
                trainWagon.TryDeliveringGoods(currentInvetory);
        }

        public void EmptyWagons()
        {
            foreach (var trainWagon in Wagons)
                trainWagon.GoodCarrier.EmptyHands();
        }

        public void UnreserveCapacity()
        {
            foreach (var trainWagon in Wagons)
                trainWagon.GoodReserver.UnreserveCapacity();
        }

        public void UnreserveStock()
        {
            foreach (var trainWagon in Wagons)
                trainWagon.GoodReserver.UnreserveStock();
        }

        public void TryRetrievingGoods(TrainDestination destination)
        {
            foreach (var trainWagon in Wagons)
                trainWagon.TryRetrievingGoods(destination);
        }
    }
}