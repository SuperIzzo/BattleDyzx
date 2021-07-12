using System.Net;
using System.Collections.Concurrent;

namespace BattleDyzx
{
    public class NetConnection
    {
        public NetDriver driver { get; private set; }
        public IPEndPoint address { get; private set; }        
        public NetMessageHandlers messageHandlers = new NetMessageHandlers();
        public ConcurrentQueue<NetMessage> unreliableSentMessageQueue = new ConcurrentQueue<NetMessage>();
        public ConcurrentQueue<NetMessage> reliableSentMessageQueue = new ConcurrentQueue<NetMessage>();

        public NetConnection(NetDriver driver, IPEndPoint address)
        {
            this.driver = driver;
            this.address = address;
        }

        public void Send(NetMessage message)
        {
            if (message.isReliable)
            {
                reliableSentMessageQueue.Enqueue(message);
            }
            else
            {
                unreliableSentMessageQueue.Enqueue(message);
            }
        }
    }
}