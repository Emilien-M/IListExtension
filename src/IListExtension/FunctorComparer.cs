using System;
using System.Collections.Generic;

namespace IListExtension
{
    internal sealed class FunctorComparer<T> : IComparer<T>
    {
        Comparison<T> comparison;
 
        public FunctorComparer(Comparison<T> comparison)
        {
            this.comparison = comparison;
        }
 
        public int Compare(T x, T y)
        {
            return comparison(x, y);
        }
    }
}