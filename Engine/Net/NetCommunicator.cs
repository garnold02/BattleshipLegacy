using System;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BattleshipClient.Engine.Net
{
    class NetCommunicator
    {
        public bool IsConnected { get; private set; }
        public TaskCompletionSource<bool> IsConnectedTCS { get; private set; }

        private TcpClient tcpClient;
        private NetworkStream tcpStream;
        public NetCommunicator(string hostname, int port)
        {
            IsConnectedTCS = new TaskCompletionSource<bool>();

            ConnectionData connectionData = new ConnectionData()
            {
                hostname = hostname,
                port = port
            };

            Thread connectorThread = new Thread(ConnectorThreadMethod)
            {
                Name = "netConnector"
            };
            connectorThread.Start(connectionData);
        }
        public void SendPacket(Packet packet)
        {
            if (IsConnected)
            {
                try
                {
                    tcpStream.Write(packet.Data, 0, Packet.BufferSize);
                    Log("Sent: {0}", Enum.GetName(typeof(PacketType), packet.Type));
                }
                catch (Exception e)
                {
                    Log("Failed to send request: {0}", e.Message);
                }
            }
            else
            {
                Log("Failed to send request: Not connected to server");
            }
        }
        public Queue<Packet> FetchPackets()
        {
            Queue<Packet> packetQueue = new Queue<Packet>();

            if (IsConnected)
            {
                while (tcpStream.DataAvailable)
                {
                    byte[] rawData = new byte[Packet.BufferSize];
                    tcpStream.Read(rawData, 0, Packet.BufferSize);

                    Packet packet = new Packet(rawData);
                    packetQueue.Enqueue(packet);
                }
            }

            return packetQueue;
        }

        private void ConnectorThreadMethod(object parameter)
        {
            ConnectionData connectionData = (ConnectionData)parameter;

            Log("Attempting to connect to server...");
            try
            {
                tcpClient = new TcpClient();
                IAsyncResult asyncResult = tcpClient.BeginConnect(IPAddress.Parse(connectionData.hostname), connectionData.port, null, null);
                asyncResult.AsyncWaitHandle.WaitOne(5000);
                bool connectionResult = asyncResult.IsCompleted;
                if (connectionResult)
                {
                    tcpStream = tcpClient.GetStream();
                    Success();
                }
                else
                {
                    Fail();
                }
            }
            catch (Exception e)
            {
                Log("ERROR: {0}", e.Message);
                Fail();
            }

            void Success()
            {
                IsConnected = true;
                IsConnectedTCS.SetResult(true);
                Log("Connected to {0}", connectionData.hostname);
            }
            void Fail()
            {
                IsConnected = false;
                Log("Failed to connect to server.");
            }
        }
        private void Log(string message, params object[] parameters)
        {
            Console.WriteLine("[NETCOM] {0}", string.Format(message, parameters));
        }

        private struct ConnectionData
        {
            public string hostname;
            public int port;
        }
    }
}
