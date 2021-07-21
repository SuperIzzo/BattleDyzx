using System;

namespace BattleDyzx
{
    public struct NetPacketId
    {
        private UInt32 id;

        public NetPacketId(UInt32 id)
        {
            this.id = id;
        }

        public override int GetHashCode()
        {
            return (int)id;
        }

        public override bool Equals(object obj)
        {
            return obj is NetPacketId && id == ((NetPacketId)obj).id;
        }

        public override string ToString()
        {
            return id.ToString();
        }

        public static explicit operator UInt32(NetPacketId packet)
        {
            return packet.id;
        }

        public static explicit operator NetPacketId(UInt32 id)
        {
            return new NetPacketId(id);
        }

        public static bool operator ==(NetPacketId a, NetPacketId b)
        {
            return a.id == b.id;
        }

        public static bool operator !=(NetPacketId a, NetPacketId b)
        {
            return a.id != b.id;
        }

        public static bool operator >(NetPacketId a, NetPacketId b)
        {
            return a.id - b.id <= UInt32.MaxValue / 2 + 1;
        }

        public static bool operator <(NetPacketId a, NetPacketId b)
        {
            return a.id - b.id > UInt32.MaxValue / 2 + 1;
        }

        public static bool operator >=(NetPacketId a, NetPacketId b)
        {
            return a.id == b.id || a.id - b.id < UInt32.MaxValue / 2;
        }

        public static bool operator <=(NetPacketId a, NetPacketId b)
        {
            return a.id == b.id || a.id - b.id > UInt32.MaxValue / 2;
        }
    }
}