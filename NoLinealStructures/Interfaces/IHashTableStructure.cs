using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoLinealStructures.Structures;

namespace NoLinealStructures.Interfaces
{
    interface IHashTableStructure<T>
    {
        void Add(T value, int key);
        T Find(int key, string value);
        void Remove(T value, int key);
        int GetHash(T value);
        int Count();
        void Clear();
        List<T> ToList();

    }
}
