namespace BattleDyzx
{
    public class NetMessage_Handshake : NetMessage
    {        
        public static readonly NetMessageType TYPE = new NetMessageType(1, typeof(NetMessage_Handshake));

        public NetMessage_Handshake()
            : base(TYPE)
        {
            isReliable = true;
        }
    }
}