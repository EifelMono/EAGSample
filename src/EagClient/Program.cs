#pragma warning disable IDE0067 // Dispose objects before losing scope
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using ProLog.Communication.Core;
using ProLog.Core;
using ProLog.Core.Log;
using ProLog.RowaLog;
using Table = ProLog3.Communication.ImageProcessing.Table;

namespace EagClient
{
    class Program
    {
#pragma warning disable IDE0060 // Remove unused parameter
        static async Task Main(string[] args)
#pragma warning restore IDE0060 // Remove unused parameter
        {
            Console.Title = $"{Globals.App.Name} {Globals.App.Version}";
            // C:\ProgramData\Rowa\Protocol\EagClient\EagClient
            // Log und WWi
            using var rowaLogProxy = new RowaLogProxy(Globals.App.Name, true);
            using var TableCommunication = new Table.ImageProcessingCommunication(6002, "127.0.0.1")
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

            Console.WriteLine("Make your selection\r\nreturn => ende\r\ns or S => setlights\r\nt or T => turntable\r\n");
            bool running = true;
            while (running)
            {
                while (!Console.KeyAvailable)
                    await Task.Delay(100);
                var key = Console.ReadKey(true);
                Console.WriteLine();
                var elapsedTime = Stopwatch.StartNew();
                switch (key.KeyChar)
                {
                    case '\r':
                    case 'c':
                    case 'C':
                        running = false;
                        break;
                    case 's':
                        {
                            var result = await TableCommunication.Send(new Table.Messages.SetLightsRequest
                            {
                                Top = false,
                                Bottom = false,
                                SideLeft = false,
                                SideMiddle = false,
                                SideRight = false
                            }).WaitAsync<Table.Messages.SetLightsResponse>();
                            if (result.IsOk()) // No Timeout
                            {
                                if (result.Message.IsOk())
                                    Console.WriteLine("SetLight Ok".LogInfo());
                                else
                                    Console.WriteLine("SetLight Server Error".LogError());
                            }
                            else
                                Console.WriteLine("SetLight Client TimeOut".LogError());
                        }
                        break;
                    case 'S':
                        {
                            if (await TableCommunication.SendAndWaitAsync<Table.Messages.SetLightsResponse>(new Table.Messages.SetLightsRequest
                            {
                                Top = true,
                                Bottom = true,
                                SideLeft = true,
                                SideMiddle = true,
                                SideRight = true
                            }) is var result && result.IsMessageOk())
                                Console.WriteLine("message Ok");
                            else
                                Console.WriteLine(@"error infos in the logs C:\ProgramData\Rowa\Protocol\EagClient\EagClient");
                        }
                        break;

                    case 't':
                        {
                            var mark = TableCommunication.Send(new Table.Messages.TurnRelativeRequest
                            {
                                Angle = 90.0,
                                Velocity = 100,
                            });

                            var result = await mark.WaitAsync<Table.Messages.TurnRelativeResponse>(TimeSpan.FromSeconds(5));
                            if (result.IsOk()) // No Timeout
                            {
                                if (result.Message.IsOk())
                                    Console.WriteLine("TurnRelative Ok".LogInfo());
                                else
                                    Console.WriteLine("TurnRelative Server Error".LogError());
                            }
                            else
                                Console.WriteLine("TurnRelative Client TimeOut".LogError());
                        }
                        break;
                    case 'T':
                        {
                            if (await TableCommunication.SendAndWaitAsync<Table.Messages.TurnRelativeResponse>(
                                new Table.Messages.TurnRelativeRequest
                                {
                                    Angle = 0.0,
                                    Velocity = 0,
                                }) is var result && result.IsOk())
                            {
                                if (result.Message.IsOk())
                                    Console.WriteLine("Message Ok");
                                else
                                {
                                    Console.WriteLine("Message Not Ok");
                                    Console.WriteLine(result.Message.Result.MessagesAsString);
                                }
                            }
                            else
                                Console.WriteLine(@"error infos in the logs C:\ProgramData\Rowa\Protocol\EagClient\EagClient");
                        }
                        break;
                }
                elapsedTime.Stop();
                Console.WriteLine($"time {elapsedTime.ElapsedMilliseconds} milliseconds".LogInfo());
            }
            Console.WriteLine($"{Globals.App.Name} finshed".LogInfo());
        }
    }
}
