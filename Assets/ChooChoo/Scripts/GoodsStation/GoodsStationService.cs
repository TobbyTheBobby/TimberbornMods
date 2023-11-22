using Timberborn.InventorySystem;

namespace ChooChoo
{
    public class GoodsStationService
    {
        private readonly GoodsStationsRepository _goodsStationsRepository;

        GoodsStationService(GoodsStationsRepository goodsStationsRepository)
        {
            _goodsStationsRepository = goodsStationsRepository;
        }

        public Inventory GoodsStationWithStock(string goodId)
        {
            foreach (var goodsStation in _goodsStationsRepository.GoodsStations)
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