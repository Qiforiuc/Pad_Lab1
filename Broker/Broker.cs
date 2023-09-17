using Common;

namespace Broker
{
    public class Broker
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("___BROKER___");

            BrokerSocket socket = new BrokerSocket();
            socket.Start(Settings.BROKER_IP, Settings.BROKER_PORT);

            Console.ReadLine();
        }
    }
}
