using System.Collections.Generic;

namespace ChooChoo
{
    public class GoodsStationsRepository
    {
        private readonly List<GoodsStation> _goodsStations = new();

        public List<GoodsStation> GoodsStations => _goodsStations;

        public void Register(GoodsStation goodsStation)
        {
            _goodsStations.Add(goodsStation);
        }

        public void UnRegister(GoodsStation goodsStation)
        {
            _goodsStations.Remove(goodsStation);
        }
    }
}