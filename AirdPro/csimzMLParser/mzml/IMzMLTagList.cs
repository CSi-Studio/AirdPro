using System.Collections.Generic;

namespace AirdPro.csimzMLParser.mzml
{
    public interface IMzMLTagList<T> : IEnumerable<T> where T : IMzMLTag
    {        
        void Add(T item);        
        T Get(int index);
        T Remove(int index);
        bool Remove(T item);
        void Clear();
        int IndexOf(T item);
        bool Contains(T item);
        int Size();
    }
}
