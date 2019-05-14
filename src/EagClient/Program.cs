using System;
using System.Threading.Tasks;
using ProLog.Communication.Core;
using ProLog.Core;
using ProLog.RowaLog;
using ProLog.Core.Log;
using Table = ProLog3.Communication.ImageProcessing.Table;

namespace EagClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.Title = Globals.App.Name;
            // C:\ProgramData\Rowa\Protocol\EagClient\EagClient
            // Log und WWi
            using var rowaLogProxy = new RowaLogProxy(Globals.App.Name, false);
            using var TableCommunication = new Table.ImageProcessingCommunication(4711, "127.0.0.1")
                .DoRun();

            TableCommunication.OnConnect.Add((communication, socket) =>
            {
                Console.WriteLine($"Table new connection {socket.RemoteEndPoint.ToString()}".LogInfo());
            });
            TableCommunication.OnDisconnect.Add((communication, socket) =>
            {
                Console.WriteLine($"Table removed connection {socket.RemoteEndPoint.ToString()}".LogInfo());
            });

            TableCommunication.OnReceived.Add((communication, message) =>
            {
                Console.WriteLine($"Table recevied {message?.GetType().Name ?? "unkown"}".LogInfo());
            });

            Console.WriteLine("c => ende\r\nl => setlights\r\nt => turntable\r\n");
            bool running = true;
            while (running)
            {
                while (!Console.KeyAvailable)
                    await Task.Delay(100);

                var key = Console.ReadKey();
                switch (key.KeyChar)
                {
                    case 'c':
                    case 'C':
                        running = false;
                        break;

                    case 'l':
                    case 'L':
                        {
                            var result = await TableCommunication.Send(new Table.Messages.SetLightsRequest
                            {
                                Top = true,
                                Bottom = true,
                                SideLeft = false,
                                SideMiddle = false,
                                SideRight = false
                            }).WaitAsync<Table.Messages.SetLightsResponse>();
                            if (result.Ok) // No Timeout
                            {
                                if (result.Value.IsOk())
                                    Console.WriteLine("SetLight Ok".LogInfo());
                                else
                                    Console.WriteLine("SetLight Server Error".LogError());
                            }
                            else
                                Console.WriteLine("SetLight Client TimeOut".LogError());
                        }
                        break;

                    case 't':
                    case 'T':
                        {
                            var mark = TableCommunication.Send(new Table.Messages.TurnRelativeRequest
                            {
                                Angle = 0.0,
                                Velocity = 100,
                            });
                            
                            var result= await mark.WaitAsync<Table.Messages.TurnRelativeResponse>(TimeSpan.FromSeconds(5));
                            if (result.Ok) // No Timeout
                            {
                                if (result.Value.IsOk())
                                    Console.WriteLine("TurnRelative Ok".LogInfo());
                                else
                                    Console.WriteLine("TurnRelative Server Error".LogError());
                            }
                            else
                                Console.WriteLine("TurnRelative Client TimeOut".LogError());
                        }
                        break;
                }
            }
            Console.WriteLine($"{Globals.App.Name} finshed".LogInfo());
        }
    }
}
