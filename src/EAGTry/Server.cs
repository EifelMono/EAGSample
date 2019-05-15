#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
using System;
using System.Threading;
using System.Threading.Tasks;
using ProLog.Communication.Core;
using ProLog.Core;
using ProLog.Core.Log;
using Table = ProLog3.Communication.ImageProcessing.Table;

namespace EAGTry
{
    public class Server
    {
        public Table.ImageProcessingCommunication ServerTableCommunication { get; set; }
        public async Task RunAsync(CancellationToken cancelationToken)
        {
            Console.WriteLine($"Server RunAsync started".LogInfo());

            using var TableCommunication = new Table.ImageProcessingCommunication(4711);
            ServerTableCommunication = TableCommunication;

            await TableCommunication.WaitConnectedAsync(cancelationToken);
            Console.WriteLine($"Server RunAsync Connected".LogInfo());

            TableCommunication.OnConnect.Add((communication, socket) =>
            {
                Console.WriteLine($"ServerTable new connection {socket.RemoteEndPoint.ToString()}".LogInfo());
            });
            TableCommunication.OnDisconnect.Add((communication, socket) =>
            {
                Console.WriteLine($"ServerTable removed connection {socket.RemoteEndPoint.ToString()}".LogInfo());
            });

            TableCommunication.OnReceived.Add(async (communication, message) =>
            {
                Console.WriteLine($"Table recevied {message?.GetType().Name ?? "unkown"}".LogInfo());
                switch (message)
                {
                    case Table.Messages.SetLightsRequest setLights:
                        if (AppGlobals.Server.WaitSetLightsInSeconds > 0)
                        {
                            Console.WriteLine($"ServerSetLightsRequest start and wait {AppGlobals.Server.WaitSetLightsInSeconds} seconds".LogInfo());
                            await Task.Delay(TimeSpan.FromSeconds(AppGlobals.Server.WaitSetLightsInSeconds));
                            Console.WriteLine($"Server SetLightsRequest ready".LogInfo());
                        }
                        communication.Send(new Table.Messages.SetLightsResponse().SetStateOk());
                        break;
                    case Table.Messages.TurnRelativeRequest turnRelative:
                        if (AppGlobals.Server.WaitTurnTableInSeconds > 0)
                        {
                            Console.WriteLine($"Server TurnRelativeRequest start and wait {AppGlobals.Server.WaitTurnTableInSeconds} seconds".LogInfo());
                            await Task.Delay(TimeSpan.FromSeconds(AppGlobals.Server.WaitTurnTableInSeconds));
                            Console.WriteLine($"Server TurnRelativeRequest ready".LogInfo());
                        }
                        communication.Send(new Table.Messages.TurnRelativeResponse().SetStateOk());
                        break;
                    default:
                        Console.WriteLine($"Server unhandled message {message?.GetType().Name ?? "unkown"}".LogError());
                        break;
                }
            });

            await Task.Delay(-1, cancelationToken);
            Console.WriteLine($"Server RunAsync finshed".LogInfo());
        }
    }
}
