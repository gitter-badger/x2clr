﻿// Copyright (c) 2013 Jae-jun Kang
// See the file COPYING for license details.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace x2
{
    public class Sequence<T> : Cell, IList<T> where T : Cell, new()
    {
        private readonly List<T> store;

        public int Count { get { return store.Count; } }
        public bool IsReadOnly { get { return false; } }

        /// <summary>
        /// Gets the value of the item at the specified index.
        /// </summary>
        public T this[int index]
        {
            get { return store[index]; }
            set { store[index] = value; }
        }

        public Sequence()
            : base(0)
        {
            store = new List<T>();
        }

        public void Add(T item)
        {
            store.Add(item);
        }

        public void AddRange(IEnumerable<T> collection)
        {
            store.AddRange(collection);
        }

        public Sequence<T> Append(T item)
        {
            store.Add(item);
            return this;
        }

        public Sequence<T> AppendRange(IEnumerable<T> collection)
        {
            store.AddRange(collection);
            return this;
        }

        public void Clear()
        {
            store.Clear();
        }

        public bool Contains(T item)
        {
            return store.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            store.CopyTo(array, arrayIndex);
        }

        public int IndexOf(T item)
        {
            return store.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            store.Insert(index, item);
        }

        public bool Remove(T item)
        {
            return store.Remove(item);
        }

        public void RemoveAt(int index)
        {
            store.RemoveAt(index);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return store.GetEnumerator();
        }

        // Explicit implementation for non-generic System.Collections.IEnumerable
        IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override void Load(Buffer buffer)
        {
            int numItems;
            buffer.Read(out numItems);

            for (int i = 0; i < numItems; ++i)
            {
                T item = new T();
                ((Cell)item).Load(buffer);
                store.Add(item);
            }
        }

        protected override void Dump(Buffer buffer)
        {
            base.Dump(buffer);
            int numItems = (int)store.Count;
            buffer.Write(numItems);

            foreach (T item in store)
            {
                ((Cell)item).Serialize(buffer);
            }
        }

        protected override void Describe(StringBuilder stringBuilder)
        {
            Type itemType = typeof(T);
            stringBuilder.Append(" {");
            foreach (T item in store)
            {
                stringBuilder.Append(" ");
                stringBuilder.Append(item.ToString());
            }
            stringBuilder.Append(" }");
            return;
        }
    }

}
