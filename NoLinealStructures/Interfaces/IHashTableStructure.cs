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
        void Add(T value);
        int Find(T value);
        void Remove(T value);

    }
}
