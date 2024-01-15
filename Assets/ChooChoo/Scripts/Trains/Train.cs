using Timberborn.BaseComponentSystem;
using Timberborn.Goods;
using UnityEngine;

namespace ChooChoo.Trains
{
    public class Train : BaseComponent
    {
        [SerializeField]
        private GoodAmountSpecification[] _trainCost;

        public GoodAmountSpecification[] TrainCost => _trainCost;
    }
}