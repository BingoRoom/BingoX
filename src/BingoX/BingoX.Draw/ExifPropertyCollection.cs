using System;
using System.Collections;
using System.Collections.Generic;

namespace BingoX.Draw
{
    /// <summary>
    ///
    /// </summary>
    public class ExifPropertyCollection : ICollection
    {
        private readonly ICollection list;

        /// <summary>
        ///
        /// </summary>
        /// <param name="enumerable"></param>
        public ExifPropertyCollection(IEnumerable<ExifProperty> enumerable)
        {
            list = new List<ExifProperty>(enumerable);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public IEnumerator GetEnumerator()
        {
            return list.GetEnumerator();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="array"></param>
        /// <param name="index"></param>
        public void CopyTo(Array array, int index)
        {
            list.CopyTo(array, index);
        }

        /// <summary>
        ///
        /// </summary>
        public int Count
        {
            get { return list.Count; }
        }

        /// <summary>
        ///
        /// </summary>
        public object SyncRoot
        {
            get { return list.SyncRoot; }
        }

        /// <summary>
        ///
        /// </summary>
        public bool IsSynchronized
        {
            get { return list.IsSynchronized; }
        }
    }
}
