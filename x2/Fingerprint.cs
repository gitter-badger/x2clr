﻿// Copyright (c) 2013 Jae-jun Kang
// See the file COPYING for license details.

using System;
using System.Collections.Generic;
using System.Threading;

namespace x2
{
    /// <summary>
    /// Manages a fixed-length compact array of bit values.
    /// </summary>
    public class Fingerprint : IComparable<Fingerprint>, IIndexable<bool>
    {
        private readonly int[] blocks;
        private readonly int length;

        /// <summary>
        /// Gets the number of bits contained in this Fingerprint.
        /// </summary>
        public int Length
        {
            get { return length; }
        }

        /// <summary>
        /// Gets the minimum number of bytes required to hold all the bits in
        /// this Fingerprint.
        /// </summary>
        private int LengthInBytes
        {
            get { return ((length - 1) >> 3) + 1; }
        }

        /// <summary>
        /// Initializes a new Fingerprint object that can hold the specified
        /// number of bit values, which are initially set to <b>false</b>.
        /// </summary>
        public Fingerprint(int length)
        {
            if (length < 0)
            {
                throw new ArgumentOutOfRangeException();
            }
            blocks = new int[((length - 1) >> 5) + 1];
            this.length = length;
        }

        /// <summary>
        /// Initializes a new Fingerprint object that contains bit values
        /// copied from the specified Fingerprint.
        /// </summary>
        public Fingerprint(Fingerprint other)
        {
            blocks = (int[])other.blocks.Clone();
            length = other.length;
        }

        /// <summary>
        /// Clears all the bits in the Fingerprint, setting them as <b>false</b>.
        /// </summary>
        public void Clear()
        {
            for (int i = 0; i < blocks.Length; ++i)
            {
                blocks[i] = 0;
            }
        }

        /// <summary>
        /// Compares this Fingerprint with the specified Fingerprint object.
        /// </summary>
        /// Implements IComparable(T).CompareTo interface.
        public int CompareTo(Fingerprint other)
        {
            if (Object.ReferenceEquals(this, other))
            {
                return 0;
            }
            if (length < other.length)
            {
                return -1;
            }
            else if (length > other.length)
            {
                return 1;
            }
            for (int i = (blocks.Length - 1); i >= 0; --i)
            {
                uint block = (uint)blocks[i];
                uint otherBlock = (uint)other.blocks[i];
                if (block < otherBlock)
                {
                    return -1;
                }
                else if (block > otherBlock)
                {
                    return 1;
                }
            }
            return 0;
        }

        /// <summary>
        /// Determines whether the specified object is equal to this Fingerprint.
        /// </summary>
        public override bool Equals(object obj)
        {
            if (Object.ReferenceEquals(this, obj))
            {
                return true;
            }
            var other = obj as Fingerprint;
            if (other == null || length != other.length)
            {
                return false;
            }
            for (int i = 0; i < blocks.Length; ++i)
            {
                if (blocks[i] != other.blocks[i])
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        public override int GetHashCode()
        {
            var hash = new Hash(Hash.Seed);
            hash.Update(length);
            for (int i = 0; i < blocks.Length; ++i)
            {
                hash.Update(blocks[i]);
            }
            return hash.Code;
        }

        /// <summary>
        /// Determines whether the specified Fingerprint object is equivalent to 
        /// this Fingerprint.
        /// </summary>
        /// A Fingerprint is said to be <i>equivalent</i> to the other when it 
        /// covers all the bits set in the other.
        /// <remarks>
        /// Given two Fingerprint objects x and y, x.IsEquivalent(y) returns
        /// <b>true</b> if:
        ///   <list type="bullet">
        ///     <item>x.Length is less than or equal to y.Length</item>
        ///     <item>All the bits set in x are also set in y</item>
        ///   </list>
        /// </remarks>
        /// <param name="other">
        /// A Fingerprint object to compare with this Fingerprint.
        /// </param>
        /// <returns>
        /// <b>true</b> if <c>other</c> is equivalent to this Fingerprint;
        /// otherwise, <b>false</b>.
        /// </returns>
        public bool IsEquivalent(Fingerprint other)
        {
            if (Object.ReferenceEquals(this, other))
            {
                return true;
            }
            if (length > other.length)
            {
                return false;
            }
            for (int i = 0; i < blocks.Length; ++i)
            {
                int block = blocks[i];
                if ((block & other.blocks[i]) != block)
                {
                    return false;
                }
            }
            return true;
        }

        public void Dump(Buffer buffer)
        {
            int count = 0;
            foreach (int block in blocks)
            {
                for (int j = 0; (j < 4) && (count < LengthInBytes); ++j, ++count)
                {
                    buffer.Write((byte)(block >> (j << 3)));
                }
            }
        }

        public void Load(Buffer buffer)
        {
            int count = 0;
            for (int i = 0; i < blocks.Length; ++i)
            {
                blocks[i] = 0;
                for (int j = 0; (j < 4) && (count < LengthInBytes); ++j, ++count)
                {
                    blocks[i] |= ((int)buffer.ReadByte() << (j << 3));
                }
            }
        }

        #region Accessors/indexer

        /// <summary>
        /// Gets the bit value at the specified index.
        /// </summary>
        public bool Get(int index)
        {
            if (index < 0 || length <= index)
            {
                throw new IndexOutOfRangeException();
            }
            return ((blocks[index >> 5] & (1 << index)) != 0);
        }

        /// <summary>
        /// Sets the bit at the specified index.
        /// </summary>
        public void Touch(int index)
        {
            if (index < 0 || length <= index)
            {
                throw new IndexOutOfRangeException();
            }
            blocks[index >> 5] |= (1 << index);
        }

        /// <summary>
        /// Clears the bit at the specified index.
        /// </summary>
        public void Wipe(int index)
        {
            if (index < 0 || length <= index)
            {
                throw new IndexOutOfRangeException();
            }
            blocks[index >> 5] &= ~(1 << index);
        }

        /// <summary>
        /// Gets the bit value at the specified index.
        /// </summary>
        public bool this[int index]
        {
            get { return Get(index); }
        }

        #endregion
    }
}
