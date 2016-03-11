using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharpFora.WebSockets
{
    public class ConcurrentHashSet<T> : IEnumerable<T>
    {
        private HashSet<T> Data { get; } = new HashSet<T>();

        public void Add(T item)
        {
            lock (Data) 
                Data.Add(item);
        }

        public void Remove(T item)
        {
            lock (Data) 
                Data.Remove(item);
        }

        public IEnumerator<T> GetEnumerator()
        {
            lock (Data)
                return Data.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() =>
            this.GetEnumerator();
    }
}
