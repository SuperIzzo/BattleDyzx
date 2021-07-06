

namespace BattleDyzx
{
    public enum NetPacketType : byte
    {        
        Stream,             // A continuous stream of data, fine to loose or receive packets out of order
        Sequence,           // An ordered sequence of information, receipt must be acknowledged
        Acknowledgement,    // Acknowledgement for received data

        Invalid = 255,
    }

    public class NetPacket
    {
        public NetStream dataStream { get; private set; }
        public NetPacketType type { get; private set; }

        public int dataOffset { get { return 0; } }

        NetPacket()
        {
            type = NetPacketType.Invalid;
        }

        public static NetPacket Construct(byte[] data)
        {
            var packet = new NetPacket();
            packet.dataStream = new NetStream(data);

            packet.Deserialize(packet.dataStream);

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
                case NetPacketType.Stream:
                case NetPacketType.Sequence:
                case NetPacketType.Acknowledgement:
                    return true;
                default:
                    return false;
            }
        }

        public void Serialize(NetStream data)
        {
            if (dataStream == null)
            {
                return;
            }

            data.WriteByte((byte)type);
        }

        public void Deserialize(NetStream data)
        {
            if (dataStream == null || dataStream.length == 0)
            {
                type = NetPacketType.Invalid;
                return;
            }

            type = (NetPacketType)data.ReadByte();
        }
    }
}