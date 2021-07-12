using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;


namespace BattleDyzx
{
    public struct NetMessageType
    {
        public readonly byte id;

        private NetMessageType(byte id)
        {
            this.id = id;
        }

        public NetMessageType(byte id, Type classType)
        {
            this.id = id;
            if (classType != null)
            {
                NetMessage.Register(this, classType);
            }
        }

        public override int GetHashCode()
        {
            return id;
        }

        public override bool Equals(object obj)
        {
            return obj is NetMessageType && id == ((NetMessageType)obj).id;
        }

        public static explicit operator byte(NetMessageType type)
        {
            return type.id;
        }

        public static explicit operator NetMessageType(byte typeId)
        {
            return new NetMessageType(typeId);
        }
    }

    public class NetMessage
    {
        static readonly NetMessageType INVALID_TYPE = new NetMessageType(255,null);

        public IPEndPoint sender { get; private set; }
        public NetMessageType typeId { get; private set; }
        public bool isReliable { get; set; }

        public NetMessage(NetMessageType tyepId)
        {
            this.typeId = tyepId;
        }

        public static NetMessage Construct(IPEndPoint sender, NetStream stream, bool isReliable)
        {
            Type messageType;
            NetMessage newMessage;
            if (messageTypes.TryGetValue((NetMessageType)stream.PeekByte(), out messageType))
            {
                newMessage = (NetMessage)Activator.CreateInstance(messageType);
            }
            else
            {
                newMessage = new NetMessage(INVALID_TYPE);
            }

            newMessage.sender = sender;
            newMessage.isReliable = isReliable;
            newMessage.Deserialize(stream);

            return newMessage;
        }

        public virtual void Serialize(NetStream stream)
        {
            stream.WriteByte((byte)typeId);
        }

        public virtual void Deserialize(NetStream stream)
        {
            typeId = (NetMessageType)stream.ReadByte();
        }

        public static void Register(NetMessageType typeId, Type messageType)
        {
            if (messageTypes == null)
            {
                messageTypes = new Dictionary<NetMessageType, Type>();
            }    

            messageTypes[typeId] = messageType;
        }

        private static Dictionary<NetMessageType, Type> messageTypes;
    }
}