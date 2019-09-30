using System.Collections.Generic;

namespace TowerDefense.Extensions
{
    public static class ListExtensions
    {
        public static LinkedListNode<T> Add<T>(this LinkedList<T> list, T value)
        {
            return list.AddLast(value);
        }
    }
}
