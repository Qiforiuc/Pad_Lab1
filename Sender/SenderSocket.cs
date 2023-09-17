using Common;
using System.ComponentModel;
using System.Net;
using System.Net.Sockets;

namespace Sender
{
    internal class SenderSocket
    {
        private Socket _socket;
        public bool IsConnected;

        public SenderSocket()
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public void Connect(string ipAddress, int port)
        {
            _socket.BeginConnect(new IPEndPoint(IPAddress.Parse(ipAddress), port), ConnectedCallback, null);
            Thread.Sleep(2000);
        }

        public void Send(byte[] data)
        {
            try
            {
                _socket.Send(data);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Could not send data. {ex.Message}");
            }
        }

        private void ConnectedCallback(IAsyncResult asyncResult)
        {
            if(_socket.Connected)
            {
                Console.WriteLine("Sender connected to Broker.");
            }
            else
            {
                Console.WriteLine("Sender not connected to Broker.");
            }

            IsConnected = _socket.Connected;
        }
    }
}
