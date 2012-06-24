using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Asn1Lib
{
    public class AutoGrowArray<T> : IEnumerable<T>
    {
        private int Index = 0;
        private int GrowFactor = 2;
        private T[] Items;

        public T this[int n]
        {
            get
            {
                return Items[n];
            }
        }

        public AutoGrowArray(int initialSize)
        {
            Items = new T[initialSize];
        }

        public void Add(T value)
        {
            if(Index >= Items.Length)
            {
                T[] newItems = new T[Items.Length * GrowFactor];
                Array.Copy(Items, newItems, Items.Length);
                Items = newItems;
            }

            Items[Index++] = value;
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int n = 0; n < Index; ++n)
            {
                yield return Items[n];
            }
        }

        public T[] ToArray()
        {
            T[] retArray = new T[Index];
            Array.Copy(Items, retArray, Index);
            return retArray;
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
