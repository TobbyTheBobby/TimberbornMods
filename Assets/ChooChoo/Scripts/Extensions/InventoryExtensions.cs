using Timberborn.InventorySystem;

namespace ChooChoo
{
    public static class InventoryExtensions
    {
        public static int UnreservedAmountInStockAndIncoming(this Inventory @inventory, string goodId)
        {
            return @inventory.UnreservedAmountInStock(goodId) + @inventory.ReservedCapacity(goodId);
        }
        
        public static int UnreservedCapacity(this Inventory @inventory)
        {
            int unreservedCapacity = 0;
            foreach (var storableGoodAmount in @inventory.AllowedGoods)
            {
                unreservedCapacity += @inventory.UnreservedCapacity(storableGoodAmount.StorableGood.GoodId);
            }
            
            return unreservedCapacity;
        }
    }
}