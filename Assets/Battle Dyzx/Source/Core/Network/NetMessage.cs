using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;


namespace BattleDyzx
{
    public class NetMessage
    {
        public IPEndPoint sender { get; private set; }
        public byte typeId { get; private set; }

        public static NetMessage Construct(IPEndPoint sender, NetStream stream)
        {
            Type messageType;
            NetMessage newMessage;
            if (messageTypes.TryGetValue(stream.PeekByte(), out messageType))
            {
                newMessage = (NetMessage)Activator.CreateInstance(messageType);
            }
            else
            {
                newMessage = new NetMessage();
            }

            newMessage.sender = sender;
            newMessage.Deserialize(stream);

            return newMessage;
        }

        public virtual void Serialize(NetStream stream)
        {
            stream.WriteByte(typeId);
        }

        public virtual void Deserialize(NetStream stream)
        {
            typeId = stream.ReadByte();
        }

        protected static void RegisterType(byte typeId, Type messageType)
        {
            messageTypes[typeId] = messageType;
        }

        private static Dictionary<byte, Type> messageTypes;
    }
}