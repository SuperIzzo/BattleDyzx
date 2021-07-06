using System;
using System.Net;

namespace BattleDyzx
{
    // Serializes and deserializes data
    // Uses Network byte order (i.e. big endian)
    public class NetStream
    {
        byte[] data;
        public int position { get; set; }

        public int length => data.Length;

        public bool endOfStream => (position >= length);

        public NetStream(int bufferSize)
        {
            data = new byte[bufferSize];
        }

        public NetStream(byte[] data)
        {
            this.data = data;
        }

        public byte PeekByte()
        {
            return data[position];
        }

        public byte ReadByte()
        {
            return data[position++];
        }

        public void WriteByte(byte value)
        {
            data[position++] = value;
        }

        public Int16 ReadInt16()
        {
            int b1 = data[position++];
            int b2 = data[position++];

            return (Int16)((b1 << 0x8) | (b2 << 0x0));
        }

        public void WriteInt16(Int16 value)
        {
            data[position++] = (byte)((value & 0xFF00) >> 0x8);
            data[position++] = (byte)((value & 0x00FF) >> 0x0);
        }

        public UInt16 ReadUInt16()
        {
            uint b1 = data[position++];
            uint b2 = data[position++];

            return (UInt16)((b1 << 0x8) | (b2 << 0x0));
        }

        public void WriteUInt16(UInt16 value)
        {
            data[position++] = (byte)((value & 0xFF00u) >> 0x8);
            data[position++] = (byte)((value & 0x00FFu) >> 0x0);
        }

        public Int32 ReadInt32()
        {
            int b1 = data[position++];
            int b2 = data[position++];
            int b3 = data[position++];
            int b4 = data[position++];

            return (b1 << 0x18) | (b2 << 0x10) | (b3 << 0x08) | (b4 << 0x00);
        }

        public void WriteInt32(Int32 value)
        {
            data[position++] = (byte)((value & 0xFF000000) >> 0x18);
            data[position++] = (byte)((value & 0x00FF0000) >> 0x10);
            data[position++] = (byte)((value & 0x0000FF00) >> 0x08);
            data[position++] = (byte)((value & 0x000000FF) >> 0x00);
        }

        public UInt32 ReadUInt32()
        {
            uint b1 = data[position++];
            uint b2 = data[position++];
            uint b3 = data[position++];
            uint b4 = data[position++];

            return (b1 << 0x18) | (b2 << 0x10) | (b3 << 0x08) | (b4 << 0x00);
        }

        public void WriteUInt32(UInt32 value)
        {
            data[position++] = (byte)((value & 0xFF000000u) >> 0x18);
            data[position++] = (byte)((value & 0x00FF0000u) >> 0x10);
            data[position++] = (byte)((value & 0x0000FF00u) >> 0x08);
            data[position++] = (byte)((value & 0x000000FFu) >> 0x00);
        }

        public float ReadFloat()
        {
            FloatIntUnion fi;
            fi.f = 0.0f;
            fi.i = ReadInt32();
            return fi.f;
        }

        public void WriteFloat(float value)
        {
            FloatIntUnion fi;
            fi.i = 0;
            fi.f = value;
            WriteInt32(fi.i);
        }
    }
}
