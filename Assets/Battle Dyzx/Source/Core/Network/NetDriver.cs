using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace BattleDyzx
{
    public class NetDriver
    {
        const int DEFAULT_LISTEN_PORT = 6050;
        const int DEFAULT_CLIENT_PORT = 6060;

        private class PacketHistory
        {
            public Queue<NetPacketId> receivedReliablePackageIds = new Queue<NetPacketId>();
            public NetPacketId lastReceivedPackagaId;
        }

        private class AsyncState
        {
            public NetPacket packet;
            public IPEndPoint address;
        }

        public NetConnection serverConnection { get; private set; }
        public ConcurrentDictionary<IPEndPoint, NetConnection> clientConnections { get; private set; }
        public NetMessageHandlers messageHandlers = new NetMessageHandlers();
        public NetMessageHandlers quickSyncMessageHandlers = new NetMessageHandlers();
        
        public bool isHost { get; private set; }

        private Dictionary<IPEndPoint, PacketHistory> packetHistoryByEndpoint = new Dictionary<IPEndPoint, PacketHistory>();

        private UdpClient udpClient;
        private int connectionPort;
        private Thread netThread;
        private bool waitingReceive;
        private bool netThreadRunning;
        private float serverTime;

        private ConcurrentQueue<NetMessage> receivedMessageQueue = new ConcurrentQueue<NetMessage>();        

        public void Listen(int port = -1)
        {
            if (port < 0)
            {
                port = DEFAULT_LISTEN_PORT;
            }

            udpClient = new UdpClient(port);

            clientConnections = new ConcurrentDictionary<IPEndPoint, NetConnection>();
            messageHandlers.RegisterHandler(NetMessage_Handshake.TYPE, OnHandshakeMsg);

            NetLog.Log(LogLevel.Info, "Listening on port: " + port);

            connectionPort = port;
            isHost = true;
            serverTime = 0.0f;

            netThreadRunning = true;
            netThread = new Thread(new ThreadStart(NetworkThread));
            netThread.IsBackground = true;
            netThread.Start();
        }

        public NetConnection Connect(string remoteIp, int remotePort = -1, int localPort = -1)
        {
            if (remotePort < 0)
            {
                remotePort = DEFAULT_LISTEN_PORT;
            }

            IPAddress ipAddress = IPAddress.Parse(remoteIp);
            IPEndPoint endPoint = new IPEndPoint(ipAddress, remotePort);
            return Connect(endPoint, localPort);
        }

        public NetConnection Connect(IPEndPoint endPoint, int localPort = -1)
        {
            if (localPort < 0)
            {
                localPort = DEFAULT_CLIENT_PORT;
            }

            udpClient = new UdpClient(localPort);
            udpClient.Connect(endPoint);

            NetLog.Log(LogLevel.Info, "Connecting to remote host: " + endPoint.ToString());

            serverConnection = new NetConnection(this, endPoint);
            serverConnection.Send(new NetMessage_Handshake());

            connectionPort = localPort;
            isHost = false;
            serverTime = 0.0f;

            netThreadRunning = true;
            netThread = new Thread(new ThreadStart(NetworkThread));
            netThread.IsBackground = true;
            netThread.Start();

            return serverConnection;
        }

        public void Shutdown()
        {
            if (udpClient != null)
            {
                NetLog.Log(LogLevel.Info, "Closing remote connections");
                udpClient.Close();
            }

            netThreadRunning = false;
            isHost = false;
            serverConnection = null;
            clientConnections = null;
            udpClient = null;
        }

        public void Update(float deltaSeconds)
        {
            serverTime += deltaSeconds;

            NetMessage message;
            while (receivedMessageQueue.TryDequeue(out message))
            {
                HandleMessage(message);
            }
        }

        private void NetworkThread()
        {
            var receiveAsyncCallback = new AsyncCallback(ReceiveAsync);            

            while (netThreadRunning)
            {
                if (!waitingReceive)
                {
                    waitingReceive = true;
                    udpClient.BeginReceive(receiveAsyncCallback, null);
                };

                if (serverConnection != null)
                {
                    SendConnectionQueue(serverConnection.address, NetPacketType.Reliable, serverConnection.reliableSentMessageQueue);
                    SendConnectionQueue(serverConnection.address, NetPacketType.Unreliable, serverConnection.unreliableSentMessageQueue);
                }

                if (clientConnections != null)
                {
                    foreach (var addressConnectionPair in clientConnections)
                    {
                        NetConnection clientConnection = addressConnectionPair.Value;
                        SendConnectionQueue(clientConnection.address, NetPacketType.Reliable, clientConnection.reliableSentMessageQueue);
                        SendConnectionQueue(clientConnection.address, NetPacketType.Unreliable, clientConnection.unreliableSentMessageQueue);
                    }
                }
            }
        }

        private void SendConnectionQueue(IPEndPoint address, NetPacketType packetType, ConcurrentQueue<NetMessage> messageQueue)
        {
            if (messageQueue == null || messageQueue.IsEmpty)
            {
                return;
            }

            NetPacket packet = NetPacket.Create(packetType);

            NetMessage message;
            while (messageQueue.TryDequeue(out message))
            {
                packet.AddMessage(message);
            }

            AsyncState asyncState = new AsyncState();
            asyncState.address = address;
            asyncState.packet = packet;

            if (serverConnection != null)
            {
                udpClient.BeginSend(packet.GetData(), packet.GetBytesWritten(), SendAsync, asyncState);
            }
            else
            {
                udpClient.BeginSend(packet.GetData(), packet.GetBytesWritten(), address, SendAsync, asyncState);
            }
        }

        private void SendAsync(IAsyncResult result)
        {
            AsyncState asyncState = (AsyncState)result.AsyncState;
            NetLog.Log(LogLevel.Detail, "Sent " + asyncState.packet.type + " packet [" + asyncState.packet.packetId + "] to " + asyncState.address);
        }

        private void ReceiveAsync(IAsyncResult result)
        {
            waitingReceive = false;

            IPEndPoint senderAddress = new IPEndPoint(IPAddress.Any, DEFAULT_CLIENT_PORT);
            byte[] payload = udpClient.EndReceive(result, ref senderAddress);

            if (serverConnection != null && !serverConnection.address.Equals(senderAddress))
            {
                NetLog.Log(LogLevel.Info, "Received data from unknown source '" + senderAddress.ToString() + "' while connected to a server '" + serverConnection.address.ToString() + "'");                
                return;
            }

            if (payload.Length == 0)
            {
                NetLog.Log(LogLevel.Warning, "Received zero size data");                
                return;
            }

            NetPacket packet = NetPacket.Create(senderAddress, payload);
            if (!packet.IsValid())
            {
                NetLog.Log(LogLevel.Warning, "Received invalid packet");                
                // check if header is valid, should we ask for resend?
                return;
            }

            PacketHistory packetHistory;
            if (!packetHistoryByEndpoint.TryGetValue(senderAddress, out packetHistory))
            {
                packetHistory = new PacketHistory();
                packetHistoryByEndpoint.Add(senderAddress, packetHistory);
            }

            if (packet.type == NetPacketType.Reliable)
            {                
                NetPacket ackPacket = packet.CreateAck();
                if (serverConnection != null)
                {
                    udpClient.BeginSend(ackPacket.GetData(), ackPacket.GetBytesWritten(), SendAsync, null);
                }
                else
                {
                    udpClient.BeginSend(ackPacket.GetData(), ackPacket.GetBytesWritten(), senderAddress, SendAsync, null);
                }

                if (packetHistory.receivedReliablePackageIds.Contains(packet.packetId))
                {
                    // Received a duplicate reliable message
                    NetLog.Log(LogLevel.Warning, "Received duplicate packet");
                    return;
                }

                packetHistory.receivedReliablePackageIds.Enqueue(packet.packetId);
                if (packetHistory.receivedReliablePackageIds.Count > 10)
                {
                    packetHistory.receivedReliablePackageIds.Dequeue();
                }
            }
            else
            {
                if (packet.packetId <= packetHistory.lastReceivedPackagaId)
                {
                    NetLog.Log(LogLevel.Warning, "Received old packet");
                    return;
                }
            }

            if (packet.packetId > packetHistory.lastReceivedPackagaId)
            {
                packetHistory.lastReceivedPackagaId = packet.packetId;
            }

            NetLog.Log(LogLevel.Detail, "Received " + packet.type + " packet [" + packet.packetId + "] from " + packet.senderAddress);

            NetMessage message;
            while (packet.TryGetMessage(out message))
            {
                if (packet.type == NetPacketType.QuickSync)
                {
                    // Handle immediately on the net thread - used for time sensitive messaging
                    quickSyncMessageHandlers.TryHandle(message);
                }
                else
                {
                    // Add to the message queue to be handled on the main thread
                    receivedMessageQueue.Enqueue(message);
                }
            }
        }

        private void HandleMessage(NetMessage message)
        {            
            if (serverConnection != null && serverConnection.address.Equals(message.sender))
            {
                serverConnection.messageHandlers.TryHandle(message);
                return;
            }

            NetConnection clientConnection;
            if (clientConnections != null && clientConnections.TryGetValue(message.sender, out clientConnection) && clientConnection != null)
            {
                clientConnection.messageHandlers.TryHandle(message);
                return;
            }
            
            messageHandlers.TryHandle(message);
        }

        private void OnHandshakeMsg(NetMessage netMessage)
        {
            if (isHost)
            {
                NetLog.Log(LogLevel.Info, "Accepted new connection from: " + netMessage.sender.ToString());
                clientConnections[netMessage.sender] = new NetConnection(this, netMessage.sender);
            }
        }
    }
}