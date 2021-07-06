using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace BattleDyzx
{
    public class NetDriver
    {
        const int DEFAULT_PORT = 6050;

        public NetConnection serverConnection { get; private set; }
        public IList<NetConnection> clientConnections { get; private set; }
        public bool isHost { get; private set; }

        private UdpClient udpClient;
        private int connectionPort;
        private Thread netThread;
        private bool waitingReceive;
        private bool netThreadRunning;

        public void Listen(int port = DEFAULT_PORT)
        {
            udpClient = new UdpClient(port);

            connectionPort = port;
            isHost = true;

            netThreadRunning = true;
            netThread = new Thread(new ThreadStart(NetworkThread));
            netThread.IsBackground = true;
            netThread.Start();
        }

        public void Shutdown()
        {
            netThreadRunning = false;
            isHost = false;
            serverConnection = null;
            clientConnections.Clear();
            udpClient = null;
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
            }
        }

        private void ReceiveAsync(IAsyncResult result)
        {
            IPEndPoint senderAddress = new IPEndPoint(IPAddress.Any, DEFAULT_PORT);
            byte[] payload = udpClient.EndReceive(result, ref senderAddress);

            if (serverConnection != null && serverConnection.address != senderAddress)
            {
                NetLogger.Log("Received data from unknown source '" + senderAddress.ToString() + "' while connected to a server '" + serverConnection.address.ToString() + "'");
                waitingReceive = false;
                return;
            }

            if (payload.Length == 0)
            {
                NetLogger.LogWarning("Received zero size data");
                waitingReceive = false;
                return;
            }

            NetPacket packet = NetPacket.Construct(payload);
            if (!packet.IsValid())
            {
                NetLogger.LogWarning("Received invalid packet");
                waitingReceive = false;
                // check if header is valid, should we ask for resend?
                return;
            }
            
            while (!packet.dataStream.endOfStream)
            {
                NetMessage mesage = NetMessage.Construct(senderAddress, packet.dataStream);
            }
        }
    }
}