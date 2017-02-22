using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SongConstructionService
{
    // This class is an array wrapper which allows for insertion in a modular sense.
    // Fill the array using Add(T) As soon as the array is filled, calls to Add(T) will start again from 
    // the beginning of the array and overwrite the current values ad-infinitum.
    public class ModuloArray<T>
    {
        public T[] Items;
        public int Index;

        // This variable will count up to the length of Items and then stop. Assume that no Items are ever deleted
        public int Count { get; set; }

        public ModuloArray(int size)
        {
            Items = new T[size];
            Index = 0;
            Count = 0;
        }

        public void Add(T item)
        {
            Items[Index] = item;
            Index = (Index + 1) % Items.Length;
            if (Count < Items.Count())
            {
                Count += 1;
            }
        }

        public object this[int i]
        {
            get { return Items[i]; }
            set { Items[i] = Items[i % Count]; }
        }

    }
}
