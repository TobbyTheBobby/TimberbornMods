using System.Collections.Generic;
using Bindito.Core;
using ChooChoo.GoodsStations;
using Timberborn.BaseComponentSystem;
using Timberborn.CoreUI;
using Timberborn.EntityPanelSystem;
using Timberborn.Localization;

namespace ChooChoo.GoodsStationUI
{
    public class GoodsStationDescriber : BaseComponent, IEntityDescriber
    {
        private static readonly string CapacityLocKey = "Inventory.Capacity";
        private ILoc _loc;
        private GoodsStation _goodsStation;

        [Inject]
        public void InjectDependencies(ILoc loc)
        {
            _loc = loc;
        }

        public void Awake()
        {
            _goodsStation = GetComponentFast<GoodsStation>();
        }

        public IEnumerable<EntityDescription> DescribeEntity()
        {
            if (!_goodsStation.enabled)
                yield return EntityDescription.CreateTextSection($"{SpecialStrings.RowStarter}{_loc.T(CapacityLocKey)} {GoodsStation.Capacity}", 100);
        }
    }
}