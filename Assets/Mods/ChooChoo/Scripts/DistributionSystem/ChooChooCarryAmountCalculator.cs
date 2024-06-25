using System;
using Timberborn.Goods;
using Timberborn.InventorySystem;
using UnityEngine;

namespace ChooChoo.DistributionSystem
{
    public class ChooChooCarryAmountCalculator
    {
        private readonly IGoodService _goodService;

        public ChooChooCarryAmountCalculator(IGoodService goodService) => _goodService = goodService;

        public GoodAmount AmountToCarry(
            int liftingCapacity,
            string goodId,
            IAmountProvider input,
            IAmountProvider output)
        {
            var good = new GoodAmount(goodId, output.UnreservedAmountInStock(goodId));
            return AmountToCarry(liftingCapacity, good);
        }

        public GoodAmount AmountToCarry(
            int liftingCapacity,
            GoodAmount good)
        {
            var good1 = _goodService.GetGood(good.GoodId);
            var num1 = Math.Max(liftingCapacity / good1.Weight, 1);
            var amount = Mathf.Min(new[]
            {
                num1,
                good.Amount
            });
            // Plugin.Log.LogError(amount + " " + num1 + " " + good.Amount);
            return new GoodAmount(good.GoodId, amount);
        }

        public int MaxAmountToCarry(
            int liftingCapacity,
            string goodId)
        {
            var good = _goodService.GetGood(goodId);
            var maxAmount = liftingCapacity / good.Weight;
            // Plugin.Log.LogError("Max Amount able to carry: " + maxAmount);
            return maxAmount;
        }

        public bool IsAtMaximumCarryCapacity(int liftingCapacity, GoodAmount currentGoodAmount)
        {
            var good = _goodService.GetGood(currentGoodAmount.GoodId);
            var maxAmount = liftingCapacity / good.Weight;
            var currentWeight = good.Weight * (currentGoodAmount.Amount + 1);
            // Plugin.Log.LogError("maxAmount " + maxAmount + " currentWeight " + currentWeight + " Result: " + (currentWeight > maxAmount));
            return currentWeight > maxAmount;
        }
    }
}