using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoLinealStructures.Structures
{
    public class HashTable<T> : Interfaces.IHashTableStructure<T>
    {
        private List<T>[] Dictionary = new List<T>[10];
        public Delegate GetKeyValue;

        public void Add(T value, int key)
        {
            if (Dictionary[key] == null)
            {
                Dictionary[key] = new List<T>();
                Dictionary[key].Add(value);
            }
            else
            {
                Dictionary[key].Add(value);
            }
        }

        public int Count()
        {
            int elements = 0;
            for(int i = 0; i < Dictionary.Length - 1; i++)
            {
                if (Dictionary[i] != null)
                {
                    elements += Dictionary[i].Count();
                }               
            }
            return elements;
        }

        public T Find(int key, string value)
        {
            if ((string)GetKeyValue.DynamicInvoke(Dictionary[key].First()) == value)
            {
                return Dictionary[key].First();
            }
            else
            {
                for(int i = 0; i < Dictionary[key].Count(); i++)
                {
                    if((string)GetKeyValue.DynamicInvoke(Dictionary[key].ElementAt(i)) == value)
                    {
                        return Dictionary[key].ElementAt(i);
                    }
                }
            }
            return default;
        }

        public int GetHash(T value)
        {
            int HashCode = 0;
            string Key = (string)GetKeyValue.DynamicInvoke(value);

            for (int i = 0; i < Key.Length; i++)
            {
                HashCode += (int)Key.ElementAt(i);
            }
            return HashCode % 10;
        }

        public void Remove(T value, int key)
        {
            Dictionary[key].Remove(value);
        }

        public void Clear()
        {
            for (int i = 0; i < Dictionary.Length; i++)
            {
                if (Dictionary[i] != null)
                {
                    Dictionary[i] = null;
                }
            }
        }
    }
}
