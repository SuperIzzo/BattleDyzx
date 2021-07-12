using System.Collections;
using System.Collections.Generic;


namespace BattleDyzx
{    
    public class NetMessageHandlers
    {
        public delegate void HandlerDelegate(NetMessage netMessage);

        private Dictionary<NetMessageType, HandlerDelegate> messageHandlers = new Dictionary<NetMessageType, HandlerDelegate>();

        public void RegisterHandler(NetMessageType messageId, HandlerDelegate handler)
        {
            messageHandlers.Add(messageId, handler);
        }

        public bool TryHandle(NetMessage message)
        {
            HandlerDelegate handler;
            if (messageHandlers.TryGetValue(message.typeId, out handler))
            {
                if (handler!=null)
                {
                    handler.Invoke(message);
                    return true;
                }
            }

            return false;
        }
    }
}
