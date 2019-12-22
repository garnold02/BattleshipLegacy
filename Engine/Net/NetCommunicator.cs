using System;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Text;
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
        public void SendRequest(string request, params object[] parameters)
        {
            if (IsConnected)
            {
                try
                {
                    string combined = string.Format(request, parameters);
                    byte[] packet = ToPacket(combined);
                    tcpStream.Write(packet, 0, packet.Length);
                    Log("Sent: [{0}]", combined);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Failed to send request: {0}", e.Message);
                }
            }
            else
            {
                Log("Failed to send request: Not connected to server");
            }
        }
        public Queue<string> FetchCommands()
        {
            Queue<string> commandQueue = new Queue<string>();

            if (IsConnected)
            {
                while (tcpStream.DataAvailable)
                {
                    byte[] packet = new byte[128];
                    tcpStream.Read(packet, 0, 128);

                    string command = Encoding.ASCII.GetString(packet).Trim('\0');
                    commandQueue.Enqueue(command);
                }
            }

            return commandQueue;
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
        private byte[] ToPacket(string text)
        {
            byte[] packet = new byte[128];
            for (int i = 0; i < packet.Length; i++)
            {
                if (i < text.Length)
                {
                    packet[i] = (byte)text[i];
                }
                else
                {
                    break;
                }
            }

            return packet;
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
