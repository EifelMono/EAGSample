#pragma warning disable IDE0067 // Dispose objects before losing scope
using System;
using System.Threading.Tasks;
using ProLog.Communication.Core;
using ProLog.Core;
using ProLog.Core.Log;
using ProLog.RowaLog;
using Table = ProLog3.Communication.ImageProcessing.Table;

namespace ProLog3Server
{
    class Program
    {
#pragma warning disable IDE0060 // Remove unused parameter
        static async Task Main(string[] args)
#pragma warning restore IDE0060 // Remove unused parameter
        {
            Console.Title = $"{Globals.App.Name} {Globals.App.Version}";
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
                        int wait = 2;
                        if (turnRelative.Velocity < 100)
                        {
                            communication.Send(new Table.Messages.TurnRelativeResponse().SetStateError($"Velocity {turnRelative.Velocity} less 100"));
                            return;
                        }

                        Console.WriteLine($"TurnRelativ start and wait={wait} seconds".LogInfo());
                        await Task.Delay(TimeSpan.FromSeconds(wait));
                        Console.WriteLine($"TurnRelativ ready".LogInfo());
                        communication.Send(new Table.Messages.TurnRelativeResponse().SetStateOk());
                        break;
                    default:
                        Console.WriteLine($"unhandled message {message?.GetType().Name ?? "unkown"}".LogError());
                        break;
                }
            });

            Console.WriteLine("End with return");
            Console.ReadLine();
            Console.WriteLine($"{Globals.App.Name} finshed".LogInfo());
        }
    }
}
