using ChooChoo.BuildingRegistrySystem;
using Timberborn.InventorySystem;
using TobbyTools.BuildingRegistrySystem;

namespace ChooChoo.GoodsStations
{
    public class GoodsStationService
    {
        private readonly BuildingRegistry<GoodsStation> _goodsStationRegistry;

        private GoodsStationService(BuildingRegistry<GoodsStation> goodsStationsRegistry)
        {
            _goodsStationRegistry = goodsStationsRegistry;
        }

        public Inventory GoodsStationWithStock(string goodId)
        {
            foreach (var goodsStation in _goodsStationRegistry.All)
            {
                if (goodsStation.SendingInventory.AmountInStock(goodId) > 0)
                {
                    return goodsStation.SendingInventory;
                }
            }

            return null;
        }
    }
}