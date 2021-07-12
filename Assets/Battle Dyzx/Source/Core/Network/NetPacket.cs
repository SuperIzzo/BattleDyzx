using System;
using System.Net;


namespace BattleDyzx
{
    public enum NetPacketType : byte
    {        
        Unreliable,         // A continuous stream of data, fine to loose or receive packets out of order
        Reliable,           // An ordered sequence of information, receipt must be acknowledged
        Acknowledgement,    // Acknowledgement for received data
        QuickSync,          // Handled immediately without a adding to a queue or other delays (unreliable)

        Invalid = 255,
    }

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
            return a.id - b.id < UInt32.MaxValue / 2;
        }        

        public static bool operator <(NetPacketId a, NetPacketId b)
        {
            return a.id - b.id > UInt32.MaxValue / 2;
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

    public class NetPacket
    {
        public NetStream dataStream { get; private set; }
        public NetPacketType type { get; private set; }
        public IPEndPoint senderAddress { get; private set; }
        public NetPacketId packetId { get; private set; }
        

        public int dataOffset { get { return 0; } }

        NetPacket()
        {
            type = NetPacketType.Invalid;
        }

        public NetPacket CreateAck()
        {
            var packet = new NetPacket();
            packet.type = NetPacketType.Acknowledgement;
            packet.packetId = packetId;
            packet.dataStream = new NetStream(10);

            packet.Serialize();

            return packet;
        }

        public static NetPacket Create(IPEndPoint sender, byte[] data)
        {
            var packet = new NetPacket();
            packet.dataStream = new NetStream(data);
            packet.senderAddress = sender;

            packet.Deserialize();

            return packet;
        }

        public static NetPacket Create(NetPacketType type, int maxDataSize = 500)
        {
            var packet = new NetPacket();
            packet.type = type;
            packet.packetId = GeneratePacketId();
            packet.dataStream = new NetStream(maxDataSize);            

            packet.Serialize();

            return packet;
        }

        public bool IsValid()
        {
            // TODO checksums etc...
            return IsNetPacketTypeValid();
        }

        public bool IsNetPacketTypeValid()
        {
            switch (type)
            {
                case NetPacketType.Unreliable:
                case NetPacketType.Reliable:
                case NetPacketType.Acknowledgement:
                case NetPacketType.QuickSync:
                    return true;
                default:
                    return false;
            }
        }

        public bool IsReliablePacket()
        {
            switch (type)
            {
                case NetPacketType.Reliable:
                case NetPacketType.Acknowledgement:
                    return true;
                default:
                    return false;
            }
        }

        public void AddMessage(NetMessage message)
        {
            message.Serialize(dataStream);
            message.isReliable = IsReliablePacket();
        }

        public bool TryGetMessage(out NetMessage message)
        {
            if (dataStream.endOfStream)
            {
                message = null;
                return false;
            }

            message = NetMessage.Construct(senderAddress, dataStream, IsReliablePacket());
            return true;
        }

        public byte[] GetData()
        {
            return dataStream.GetData();
        }

        public int GetBytesWritten()
        {
            return dataStream.position;
        }

        private void Serialize()
        {
            dataStream.WriteByte((byte)type);
            dataStream.WriteUInt32((UInt32) packetId);
        }

        private void Deserialize()
        {
            if (dataStream == null || dataStream.length == 0)
            {
                type = NetPacketType.Invalid;
                return;
            }

            type = (NetPacketType)dataStream.ReadByte();
            packetId = (NetPacketId) dataStream.ReadUInt32();
        }

        private static UInt32 currentPacketId = 0;
        private static NetPacketId GeneratePacketId()
        {
            return new NetPacketId(++currentPacketId);
        }
    }
}