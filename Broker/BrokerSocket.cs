using Common;
using System.Net;
using System.Net.Sockets;

namespace Broker
{
    public class BrokerSocket
    {
        private Socket _socket;

        public BrokerSocket()
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public void Start(string ip, int port)
        {
            _socket.Bind(new IPEndPoint(IPAddress.Parse(ip), port));
            _socket.Listen(Settings.MAX_CONNECTIONS);
            AcceptConnection();
        }

        private void AcceptConnection()
        {
            _socket.BeginAccept(AcceptedCallback, null);
        }

        private void AcceptedCallback(IAsyncResult asyncResult)
        {
            ConnectionInfo connection  = new ConnectionInfo();
            try
            {
                connection.Socket = _socket.EndAccept(asyncResult);
                connection.Address = connection.Socket.RemoteEndPoint.ToString();
                connection.Socket.BeginReceive(connection.Data, 0, connection.Data.Length,
                    SocketFlags.None, ReceiveCallback, connection);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Can't accept. {ex.Message}");
            }
            finally
            {
                AcceptConnection();
            }
        }

        private void ReceiveCallback(IAsyncResult asyncResult)
        {
            ConnectionInfo connection = asyncResult.AsyncState as ConnectionInfo;
            try
            {
                Socket senderSocket = connection.Socket;
                SocketError response;

                senderSocket.EndReceive(asyncResult, out response);
                if(response == SocketError.Success)
                {
                    byte[] payload = new byte[Settings.BUFF_SIZE];
                    Array.Copy(connection.Data, payload, payload.Length);

                    PayloadHandler.Handle(payload, connection);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Can't receive data: {ex.Message}");
            }
            finally
            {
                try
                {
                    connection.Socket.BeginReceive(connection.Data, 0, connection.Data.Length, SocketFlags.None, ReceiveCallback, connection);
                }
                catch(Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    var address = connection.Socket.RemoteEndPoint.ToString();

                    connection.Socket.Close();

                }
            }
        }
    }
}
