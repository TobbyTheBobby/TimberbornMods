using System.Collections.Generic;
using Bindito.Core;
using Timberborn.BaseComponentSystem;
using Timberborn.CoreUI;
using Timberborn.EntityPanelSystem;
using Timberborn.Localization;

namespace ChooChoo.TrainYards
{
    public class TrainYardDescriber : BaseComponent, IEntityDescriber
    {
        private static readonly string CapacityLocKey = "Inventory.Capacity";
        private ILoc _loc;
        private TrainYard _trainYard;

        [Inject]
        public void InjectDependencies(ILoc loc)
        {
            _loc = loc;
        }

        public void Awake()
        {
            _trainYard = GetComponentFast<TrainYard>();
        }

        public IEnumerable<EntityDescription> DescribeEntity()
        {
            if (!_trainYard.enabled)
                yield return EntityDescription.CreateTextSection($"{SpecialStrings.RowStarter}{_loc.T(CapacityLocKey)} {_trainYard.MaxCapacity}",
                    100);
        }
    }
}