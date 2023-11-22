using System.Collections.Generic;

namespace ChooChoo
{
    static class ListExtensions
    {
        public static void MoveItemToFront<T>(this List<T> list, T item)
        {
            list.Remove(item);
            list.Insert(0, item);
        }
        
        public static void MoveItemToEnd<T>(this List<T> list, T item)
        {
            list.Remove(item);
            list.Add(item);
        }
    }
}