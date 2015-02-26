﻿// Copyright (c) 2013, 2014 Jae-jun Kang
// See the file COPYING for license details.

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace x2
{
    // Serializer.Read
    public sealed partial class Serializer
    {
        // Overloaded Read for primitive types

        /// <summary>
        /// Decodes a boolean value out of the underlying stream.
        /// </summary>
        public void Read(out bool value)
        {
            CheckLengthToRead(1);
            value = (stream.ReadByte() != 0);
        }

        /// <summary>
        /// Decodes a single byte out of the underlying stream.
        /// </summary>
        public void Read(out byte value)
        {
            CheckLengthToRead(1);
            value = (byte)stream.ReadByte();
        }

        /// <summary>
        /// Decodes an 8-bit signed integer out of the underlying stream.
        /// </summary>
        public void Read(out sbyte value)
        {
            CheckLengthToRead(1);
            value = (sbyte)stream.ReadByte();
        }

        /// <summary>
        /// Decodes a 16-bit signed integer out of the underlying stream.
        /// </summary>
        public void Read(out short value)
        {
            CheckLengthToRead(2);
            value = (short)stream.ReadByte();
            value = (short)((value << 8) | stream.ReadByte());
        }

        /// <summary>
        /// Decodes a 32-bit signed integer out of the underlying stream.
        /// </summary>
        public int Read(out int value)
        {
            // Zigzag decoding
            uint u;
            int bytes = ReadVariable(out u);
            value = (int)(u >> 1) ^ -((int)u & 1);
            return bytes;
        }

        /// <summary>
        /// Decodes a 64-bit signed integer out of the underlying stream.
        /// </summary>
        public int Read(out long value)
        {
            // Zigzag decoding
            ulong u;
            int bytes = ReadVariable(out u);
            value = (long)(u >> 1) ^ -((long)u & 1);
            return bytes;
        }

        /// <summary>
        /// Decodes a 32-bit floating-point number out of the underlying stream.
        /// </summary>
        public void Read(out float value)
        {
            int i;
            ReadFixedBigEndian(out i);
            value = BitConverter.ToSingle(System.BitConverter.GetBytes(i), 0);
        }

        /// <summary>
        /// Decodes a 64-bit floating-point number out of the underlying stream.
        /// </summary>
        public void Read(out double value)
        {
            long l;
            ReadFixedBigEndian(out l);
            value = BitConverter.ToDouble(System.BitConverter.GetBytes(l), 0);
        }

        /// <summary>
        /// Decodes a text string out of the underlying stream.
        /// </summary>
        public void Read(out string value)
        {
            // UTF-8 decoding
            int length;
            ReadVariableNonnegative(out length);
            if (length == 0)
            {
                value = String.Empty;
                return;
            }
            CheckLengthToRead(length);
            char c, c2, c3;
            int bytesRead = 0;
            var stringBuilder = new StringBuilder(length);
            while (bytesRead < length)
            {
                c = (char)stream.ReadByte();
                switch (c >> 4)
                {
                    case 0:
                    case 1:
                    case 2:
                    case 3:
                    case 4:
                    case 5:
                    case 6:
                    case 7:
                        // 0xxxxxxx
                        ++bytesRead;
                        stringBuilder.Append(c);
                        break;
                    case 12:
                    case 13:
                        // 110x xxxx  10xx xxxx
                        bytesRead += 2;
                        if (bytesRead > length)
                        {
                            throw new Exception("Invalid UTF-8 stream");
                        }
                        c2 = (char)stream.ReadByte();
                        if ((c2 & 0xc0) != 0x80)
                        {
                            throw new Exception("Invalid UTF-8 stream");
                        }
                        stringBuilder.Append((char)(((c & 0x1f) << 6) | (c2 & 0x3f)));
                        break;
                    case 14:
                        // 1110 xxxx  10xx xxxx  10xx xxxx
                        bytesRead += 3;
                        if (bytesRead > length)
                        {
                            throw new Exception("Invalid UTF-8 stream");
                        }
                        c2 = (char)stream.ReadByte();
                        c3 = (char)stream.ReadByte();
                        if (((c2 & 0xc0) != 0x80) || ((c3 & 0xc0) != 0x80))
                        {
                            throw new Exception("Invalid UTF-8 stream");
                        }
                        stringBuilder.Append((char)(((c & 0x0f) << 12) |
                          ((c2 & 0x3f) << 6) | ((c3 & 0x3f) << 0)));
                        break;
                    default:
                        // 10xx xxxx  1111 xxxx
                        throw new Exception("Invalid UTF-8 stream");
                }
            }
            value = stringBuilder.ToString();
        }

        /// <summary>
        /// Decodes a datetime value out of the underlying stream.
        /// </summary>
        public void Read(out DateTime value)
        {
            long usecs;
            ReadFixedBigEndian(out usecs);
            DateTime unixEpoch = new DateTime(621355968000000000);
            value = unixEpoch.AddTicks(usecs * 10);
        }

        // Overloaded Read for composite types

        /// <summary>
        /// Decodes an array of bytes out of the underlying stream.
        /// </summary>
        public void Read(out byte[] value)
        {
            int length;
            ReadVariableNonnegative(out length);
            CheckLengthToRead(length);
            value = new byte[length];
            stream.Read(value, 0, length);
        }

        /// <summary>
        /// Decodes an ordered list of Cell-derived objects out of the
        /// underlying stream.
        /// </summary>
        public void Read<T>(out IList<T> value) where T : Cell, new()
        {
            int count;
            ReadVariableNonnegative(out count);
            value = new List<T>();
            for (int i = 0; i < count; ++i)
            {
                T element;
                Read(out element);
                value.Add(element);
            }
        }

        /// <summary>
        /// Decodes a Cell-derived objects out of the underlying stream.
        /// </summary>
        public void Read<T>(out T value) where T : Cell, new()
        {
            value = null;
            int length;
            ReadVariableNonnegative(out length);
            if (length == 0) { return; }

            long markerSaved = marker;
            marker = stream.Position + length;

            // try
            Cell.Load(this, out value);
            // catch

            if (stream.Position != marker)
            {
                stream.Position = marker;
            }
            marker = markerSaved;
        }

        // Read helper methods

        /// <summary>
        /// Decodes a 32-bit signed integer out of the underlying stream,
        /// by fixed-width big-endian byte order.
        /// </summary>
        private void ReadFixedBigEndian(out int value)
        {
            CheckLengthToRead(4);
            value = stream.ReadByte();
            value = (value << 8) | stream.ReadByte();
            value = (value << 8) | stream.ReadByte();
            value = (value << 8) | stream.ReadByte();
        }

        /// <summary>
        /// Decodes a 64-bit signed integer out of the underlying stream,
        /// by fixed-width big-endian byte order.
        /// </summary>
        private void ReadFixedBigEndian(out long value)
        {
            CheckLengthToRead(8);
            value = stream.ReadByte();
            value = (value << 8) | (byte)stream.ReadByte();
            value = (value << 8) | (byte)stream.ReadByte();
            value = (value << 8) | (byte)stream.ReadByte();
            value = (value << 8) | (byte)stream.ReadByte();
            value = (value << 8) | (byte)stream.ReadByte();
            value = (value << 8) | (byte)stream.ReadByte();
            value = (value << 8) | (byte)stream.ReadByte();
        }

        /// <summary>
        /// Decodes a 32-bit signed integer out of the underlying stream,
        /// by fixed-width little-endian byte order.
        /// </summary>
        private void ReadFixedLittleEndian(out int value)
        {
            CheckLengthToRead(4);
            value = stream.ReadByte();
            value |= stream.ReadByte() << 8;
            value |= stream.ReadByte() << 16;
            value |= stream.ReadByte() << 24;
        }

        /// <summary>
        /// Decodes a 64-bit signed integer out of the underlying stream,
        /// by fixed-width little-endian byte order.
        /// </summary>
        private void ReadFixedLittleEndian(out long value)
        {
            CheckLengthToRead(8);
            value = stream.ReadByte();
            value |= (long)stream.ReadByte() << 8;
            value |= (long)stream.ReadByte() << 16;
            value |= (long)stream.ReadByte() << 24;
            value |= (long)stream.ReadByte() << 32;
            value |= (long)stream.ReadByte() << 40;
            value |= (long)stream.ReadByte() << 48;
            value |= (long)stream.ReadByte() << 56;
        }

        /// <summary>
        /// Decodes a 32-bit unsigned integer out of the underlying stream,
        /// with unsigned LEB128 decoding.
        /// </summary>
        public int ReadVariable(out uint value)
        {
            return ReadVariableInternal(stream, out value);
        }

        internal static int ReadVariableInternal(Buffer buffer, out uint value)
        {
            // Unsigned LEB128 decoding
            value = 0U;
            int i, shift = 0;
            for (i = 0; i < 5; ++i)
            {
                buffer.CheckLengthToRead(1);
                byte b = (byte)buffer.ReadByte();
                value |= (((uint)b & 0x7fU) << shift);
                if ((b & 0x80) == 0)
                {
                    break;
                }
                shift += 7;
            }
            return (i < 5 ? (i + 1) : 5);
        }

        /// <summary>
        /// Decodes a 64-bit unsigned integer out of the underlying stream,
        /// with unsigned LEB128 decoding.
        /// </summary>
        public int ReadVariable(out ulong value)
        {
            // Unsigned LEB128 decoding
            value = 0UL;
            int i, shift = 0;
            for (i = 0; i < 10; ++i)
            {
                CheckLengthToRead(1);
                byte b = (byte)stream.ReadByte();
                value |= (((ulong)b & 0x7fU) << shift);
                if ((b & 0x80) == 0)
                {
                    break;
                }
                shift += 7;
            }
            return (i < 10 ? (i + 1) : 10);
        }

        /// <summary>
        /// Decodes a 32-bit non-negative integer out of the underlying stream.
        /// </summary>
        public int ReadVariableNonnegative(out int value)
        {
            uint unsigned;
            int result = ReadVariable(out unsigned);
            if (unsigned > Int32.MaxValue) { throw new OverflowException(); }
            value = (int)unsigned;
            return result;
        }
    }

    public static class SerializerReadExtensions
    {
        public static int ReadVariable(this Buffer self, out uint value)
        {
            return Serializer.ReadVariableInternal(self, out value);
        }
    }
}
