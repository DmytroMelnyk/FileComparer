using System.Collections;
using System.Collections.Generic;

namespace Algorithm
{
    public class ArrayComparer<T> : IEqualityComparer<T> 
        where T : IStructuralEquatable
    {
        public bool Equals(T x, T y)
        {
            return StructuralComparisons.StructuralEqualityComparer.Equals(x, y);
        }

        public int GetHashCode(T obj)
        {
            return StructuralComparisons.StructuralEqualityComparer.GetHashCode(obj);
        }

        public static ArrayComparer<T> Default { get; } = new ArrayComparer<T>();
    }
}
