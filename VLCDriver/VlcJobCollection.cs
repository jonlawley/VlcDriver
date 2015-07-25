using System;
using System.Collections.Generic;
using System.Collections;

namespace VLCDriver
{
    public class VlcJobCollection : ICollection<VlcJob>
    {
        private Object threadLock = new Object();
        private List<VlcJob> internalList = new List<VlcJob>();

        public void Add(VlcJob item)
        {
            lock(threadLock)
            {
                internalList.Add(item);
            }
        }

        public void Clear()
        {
            lock (threadLock)
            {
                internalList.Clear();
            }
        }

        public bool Contains(VlcJob item)
        {
            lock (threadLock)
            {
                return internalList.Contains(item);
            }
        }

        public void CopyTo(VlcJob[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public int Count
        {
            get
            {
                lock (threadLock)
                {
                    return internalList.Count;
                }
            }
        }

        public bool IsReadOnly
        {
            get { throw new NotImplementedException(); }
        }

        public bool Remove(VlcJob item)
        {
            lock (threadLock)
            {
                return internalList.Remove(item);
            }
        }

        public IEnumerator<VlcJob> GetEnumerator()
        {
            lock(threadLock)
            {
                return internalList.GetEnumerator();
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
