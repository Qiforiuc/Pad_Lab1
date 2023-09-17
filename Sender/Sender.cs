using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Newtonsoft.Json;

namespace Sender
{
    class Sender
    {

        static void Main(string[] args)
        {
            Console.WriteLine("___SENDER___");

            var senderSocket = new SenderSocket();

            senderSocket.Connect(Settings.BROKER_IP, Settings.BROKER_PORT);

            if(senderSocket.IsConnected)
            {
                while(true)
                {
                    var payload = new Payload();

                    Console.Write("Enter the channel: ");
                    payload.Channel = Console.ReadLine().ToLower();

                    Console.WriteLine("Enter the message: ");
                    payload.Message = Console.ReadLine();

                    var payloadString = JsonConvert.SerializeObject(payload);
                    byte[] data = Encoding.UTF8.GetBytes(payloadString);

                    senderSocket.Send(data);
                }
            }

            Console.ReadLine();
        }

    }
}
