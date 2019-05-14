using System;
using System.Threading.Tasks;
using ProLog.Communication.Core;
using ProLog.Core;
using ProLog.RowaLog;
using ProLog.Core.Log;
using Table = ProLog3.Communication.ImageProcessing.Table;

namespace ProLog3Server
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.Title = Globals.App.Name;
            await Task.Delay(1);
            // C:\ProgramData\Rowa\Protocol\ProLog3Server\ProLog3Server
            // Log und WWi
            using var rowaLogProxy = new RowaLogProxy(Globals.App.Name, false);
            using var TableCommunication = new Table.ImageProcessingCommunication(4711)
                .DoRun();

            TableCommunication.OnConnect.Add((communication, socket) =>
            {
                Console.WriteLine($"Table new connection {socket.RemoteEndPoint.ToString()}".LogInfo());
            });
            TableCommunication.OnDisconnect.Add((communication, socket) =>
            {
                Console.WriteLine($"Table removed connection {socket.RemoteEndPoint.ToString()}".LogInfo());
            });

            TableCommunication.OnReceived.Add(async (communication, message) =>
            {
                Console.WriteLine($"Table recevied {message?.GetType().Name ?? "unkown"}".LogInfo());
                switch (message)
                {
                    case Table.Messages.SetLightsRequest setLights:
                        communication.Send(new Table.Messages.SetLightsResponse().SetStateOk());
                        break;
                    case Table.Messages.TurnRelativeRequest turnRelative:
                        Console.WriteLine($"TurnRelativ start".LogInfo());
                        await Task.Delay(TimeSpan.FromSeconds(2));
                        Console.WriteLine($"TurnRelativ ready".LogInfo());
                        communication.Send(new Table.Messages.TurnRelativeResponse().SetStateOk());
                        break;
                    default:
                        Console.WriteLine($"unhandled message {message?.GetType().Name ?? "unkown"}".LogError());
                        break;
                }
            });

            Console.WriteLine("Stop With Return");
            Console.ReadLine();
            Console.WriteLine($"{Globals.App.Name} finshed".LogInfo());
        }
    }
}
