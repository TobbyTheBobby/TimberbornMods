using System;
using System.Collections.Generic;
using System.Linq;
using Bindito.Core;
using Timberborn.BaseComponentSystem;
using Timberborn.BlockSystem;
using Timberborn.Goods;
using Timberborn.InventorySystem;
using Timberborn.Persistence;

namespace ChooChoo.GoodsStations
{
    public class LimitableGoodDisallower : BaseComponent, IFinishedStateListener, IPersistentEntity, IGoodDisallower
    {
        private static readonly ComponentKey LimitedGoodDisallowerKey = new(nameof(LimitableGoodDisallower));
        private static readonly ListKey<GoodAmount> LimitsKey = new(nameof(_limits));

        public event EventHandler<DisallowedGoodsChangedEventArgs> DisallowedGoodsChanged;

        private GoodAmountSerializer _goodAmountSerializer;

        private readonly Dictionary<string, int> _limits = new();
        private string _componentName;

        [Inject]
        public void InjectDependencies(GoodAmountSerializer savedGoodObjectSerializer)
        {
            _goodAmountSerializer = savedGoodObjectSerializer;
        }

        public void Awake()
        {
            enabled = false;
        }

        public void OnEnterFinishedState()
        {
            enabled = true;
        }

        public void OnExitFinishedState()
        {
            enabled = false;
        }

        public void Save(IEntitySaver entitySaver)
        {
            var component = entitySaver.GetComponent(LimitedGoodDisallowerKey, _componentName);
            var savedGoods = _limits.Select(good => new GoodAmount(good.Key, good.Value)).ToList();
            component.Set(LimitsKey, savedGoods, _goodAmountSerializer);
        }

        public void Load(IEntityLoader entityLoader)
        {
            if (!entityLoader.HasComponent(LimitedGoodDisallowerKey, _componentName))
                return;
            var component = entityLoader.GetComponent(LimitedGoodDisallowerKey, _componentName);
            if (!component.Has(LimitsKey))
                return;
            foreach (var limit in component.Get(LimitsKey, _goodAmountSerializer))
            {
                _limits.Add(limit.GoodId, limit.Amount);
            }
        }

        public bool HasAllowedGoods => _limits.Values.Any(amount => amount > 0);

        public void SetComponentName(string value)
        {
            _componentName = value;
        }

        public int AllowedAmount(string goodId)
        {
            return _limits.TryGetValue(goodId, out var num) ? num : 0;
        }

        public void SetAllowedAmount(string goodId, int amount)
        {
            _limits[goodId] = amount;
            InvokeDisallowedGoodsChangedEvent(goodId);
        }

        private void InvokeDisallowedGoodsChangedEvent(string goodId)
        {
            var disallowedGoodsChanged = DisallowedGoodsChanged;
            if (disallowedGoodsChanged == null)
                return;
            disallowedGoodsChanged(this, new DisallowedGoodsChangedEventArgs(goodId));
        }
    }
}